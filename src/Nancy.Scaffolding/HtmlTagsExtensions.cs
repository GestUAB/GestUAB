//
// HtmlTagsExtensions.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2012 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Nancy.Scaffolding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using FluentValidation;
    using FluentValidation.Internal;
    using FluentValidation.Validators;
    using HtmlTags;
    using Mono.Addins;
    using Nancy.TinyIoc;
    using Nancy.ViewEngines.Razor;
    using Nancy.Scaffolding.Validators;

    /// <summary>
    /// Html tags extensions class.
    /// This class is capable to render html elements in a razor page.
    /// Basically you can scaffold any model to an input or list of inputs. 
    /// </summary>
    public static class HtmlTagsExtensions
    {

        static HtmlTagsExtensions()
        {
            AddinManager.Initialize();
            AddinManager.Registry.Update();
        }

        /// <summary>
        /// Render an entire form from model.
        /// </summary>
        /// <returns>
        /// An <see cref="IHtmlString"/> object.
        /// </returns>
        /// <param name='model'>
        /// The model that refers to form.
        /// </param>
        /// <param name='formAction'>
        /// The type of form. <see cref="FormType"/>
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString FormFor<TModel>(TModel model, FormAction formAction)
        {
            var tag = new FormTag();
            GenerateFields(model, formAction, tag);
            return new NonEncodedHtmlString(tag == null ? "" : tag.ToString());
        }


        /// <summary>
        /// Render a list of inputs from a model.
        /// </summary>
        /// <returns>
        /// An <see cref="IHtmlString"/> object.
        /// </returns>
        /// <param name='model'>
        /// Model.
        /// </param>
        /// <param name='formAction'>
        /// Form type.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString FieldsFor<TModel>(TModel model, FormAction formAction)
        {
            var tag = new DivTag();
            GenerateFields(model, formAction, tag);
            return new NonEncodedHtmlString(tag == null ? "" : tag.ToString());
        }

        static void GenerateFields<TModel>(TModel model, FormAction formAction, HtmlTag tag)
        {
            foreach (var prop in model.GetType().GetProperties())
            {
                var config = GetConfig(model, prop);
                Visibility visibility = config.VisibilityConfig.GetFromFormAction(formAction);
                if (visibility == Visibility.None) 
                {
                    continue;
                }

                var htmlTag = GenerateFieldTag(model, config, visibility, formAction, prop);
                if (htmlTag != null)
                {
                    tag.Append(htmlTag);
                }
            }
        }

        /// <summary>
        /// Generates the inner tag.
        /// </summary>
        /// <returns>The inner tag.</returns>
        /// <param name="model">Model.</param>
        /// <param name="tag">Tag.</param>
        /// <param name="config">Config.</param>
        /// <param name="visibility">Visibility.</param>
        /// <param name="formAction">Form action.</param>
        /// <param name="prop">Property.</param>
        /// <typeparam name="TModel">The 1st type parameter.</typeparam>
        static HtmlTag GenerateFieldTag<TModel>(TModel model,
                                               ScaffoldConfig config, 
                                               Visibility visibility,
                                               FormAction formAction, 
                                               PropertyInfo prop)
        {
            if (config.InputType == InputType.Auto)
            {
                //TODO Code when input type is not auto
            }

            if (visibility == Visibility.Show)
            {
                var type = prop.PropertyType;
                if (type == typeof(string))
                {
                    return GenerateInputText<TModel>(model, prop, formAction);
                }
                else if (type == typeof(bool))
                {
                    return GenerateInputCheck<TModel>(model, prop, formAction);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    return GenerateSelect<TModel>(model, prop, formAction);
                }
                else if (type.IsEnum)
                {
                    return GenerateSelect<TModel>(model, prop, formAction);
                }
                else if (typeof(DateTime).IsAssignableFrom(type) || typeof(DateTimeOffset).IsAssignableFrom(type))
                {
                    return GenerateCalendar<TModel>(model, prop, formAction);
                }
                else if (config.ObjectListConfig != null)
                {
                    return GenerateObjectList<TModel>(model, prop, config.ObjectListConfig, formAction);
                }
            }
            else if (visibility == Visibility.Hidden)
            {
                return GenerateInputHidden<TModel>(model, prop, formAction);
            }

            return null;
        }

        static ScaffoldConfig GetConfig(object model, PropertyInfo prop)
        {
            var container = TinyIoCContainer.Current.Resolve<ConfigContainer>();
            var propertyName = prop.ReflectedType.FullName + "." + prop.Name;
            var config = container.GetConfig(propertyName);
            if (config != null)
            {
                return config;
            }
            config = container.Register(prop);
            config.Name = prop.Name;

            var visibility = model.GetAttribute<VisibilityAttribute>(prop);

            if (visibility != null)
            {
                config.VisibilityConfig = new VisibilityConfig
                {
                    Create = visibility.Create,
                    Delete =  visibility.Delete,
                    Read = visibility.Read,
                    Update = visibility.Update
                };
            }

            var objList = model.GetAttribute<ObjectListAttribute>(prop);

            if (objList != null)
            {
                config.ObjectListConfig = new ObjectListConfig(() => objList.GetList)
                {
                    SelectType = objList.SelectType,
                    ValueMember = objList.ValueMember
                };
            }

            return container.GetConfig(propertyName);
        }

        /// <summary>
        /// Render an input from a model.
        /// </summary>
        /// <returns>
        /// An <see cref="IHtmlString"/> object.
        /// </returns>
        /// <param name='model'>
        /// Model.
        /// </param>
        /// <param name='objectProperty'>
        /// Object property.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString InputTextFor<TModel>(TModel model, 
                                                       Expression<Func<TModel, object>> objectProperty,
                                                       FormAction formAction)
        {
            var member = GetMember<TModel>(objectProperty);
            if (member == null)
                return new NonEncodedHtmlString("");
            var tag = GenerateInputText(model, member, formAction);
            return new NonEncodedHtmlString(tag == null ? "" : tag.ToString());
        }

        /// <summary>
        /// Render an input from a model.
        /// </summary>
        /// <returns>
        /// An <see cref="IHtmlString"/> object.
        /// </returns>
        /// <param name='model'>
        /// Model.
        /// </param>
        /// <param name='propertyName'>
        /// Property name.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString InputTextFor<TModel>(TModel model, 
                                                       string propertyName, 
                                                       FormAction formAction)
        {
            var member = typeof(TModel).GetProperty(propertyName, 
                                                    BindingFlags.Public | BindingFlags.Instance);
            if (member == null)
                return new NonEncodedHtmlString("");
            var tag = GenerateInputText(model, member, formAction);
            return new NonEncodedHtmlString(tag == null ? "" : tag.ToString());
        }

        static HtmlTag GenerateInputText<TModel>(TModel model, PropertyInfo propertyInfo, FormAction formAction)
        {
            var modelValue = formAction == FormAction.Create ? "" : propertyInfo.GetValue(model, null).ToString();

            if (modelValue == null)
            {
                return null;
            }

            //<div class="control-group">
            //  <label class="control-label" for="Email">Email:</label>
            //  <div class="controls">
            //    <input type="text" class="input-xlarge" data-val="true" 
            //        data-val-email="O e-mail digitado não é válido." 
            //        data-val-required="O campo E-mail é obrigatório." 
            //        id="Email" name="Email" value="@Model.Email">
            //    <span class="field-validation-valid error" data-valmsg-for="Email" 
            //        data-valmsg-replace="true"></span>
            //    <p class="help-block">Ex.: jose@unicentro.br</p>
            //  </div>
            //</div>

            var attr = model.GetAttribute<InputTypeAttribute>(propertyInfo);

            var inputType = InputType.Text;

            if (attr != null)
            {
                inputType = attr.Type;
            }

            var cg = CreateControlGroup(model, propertyInfo);

            var input = new TextboxTag(propertyInfo.Name, modelValue)
                .Id(propertyInfo.Name).Attr("type", inputType.ToString().ToLower());

            FillValidation<TModel>(input, propertyInfo);

            cg.Children[1].InsertFirst(input);

            return cg;
        }

        static HtmlTag GenerateInputCheck<TModel>(TModel model, PropertyInfo member, FormAction formAction)
        {
            var modelValue = formAction == FormAction.Create ? false : (bool)member.GetValue(model, null);

            //<div class="control-group">
            //  <label class="control-label" for="Email">Email:</label>
            //  <div class="controls">
            //    <input type="checkbox" class="input-xlarge" data-val="true" 
            //        data-val-email="O e-mail digitado não é válido." 
            //        data-val-required="O campo E-mail é obrigatório." 
            //        id="Email" name="Email" value="@Model.Email">
            //    <span class="field-validation-valid error" data-valmsg-for="Email" 
            //        data-valmsg-replace="true"></span>
            //    <p class="help-block">Ex.: jose@unicentro.br</p>
            //  </div>
            //</div>
          
            var cg = CreateControlGroup(model, member);

            var input = new CheckboxTag(modelValue)
                .Attr("name", member.Name)
                .Id(member.Name);

            FillValidation<TModel>(input, member);

            cg.Children[1].InsertFirst(input);

            return cg;
        }


        static HtmlTag GenerateSelect<TModel>(TModel model, PropertyInfo propertyInfo, FormAction formAction)
        {
            var modelValue = propertyInfo.GetValue(model, null);

            if (modelValue == null)
            {
                return null;
            }

            //<div class="control-group">
            //  <label class="control-label" for="Email">Email:</label>
            //  <div class="controls">
            //    <select type="checkbox" class="input-xlarge" data-val="true" 
            //        data-val-email="O e-mail digitado não é válido." 
            //        data-val-required="O campo E-mail é obrigatório." 
            //        id="Email" name="Email" value="@Model.Email">
            //       <option value="volvo">Volvo</option>
            //       <option value="saab">Saab</option>
            //    </select>
            //    <span class="field-validation-valid error" data-valmsg-for="Email" 
            //        data-valmsg-replace="true"></span>
            //    <p class="help-block">Ex.: jose@unicentro.br</p>
            //  </div>
            //</div>
          
            var cg = CreateControlGroup(model, propertyInfo);

            var selectType = model.GetAttribute<SelectAttribute>(propertyInfo);

            var selekt = new SelectTag()
                .Attr("name", propertyInfo.Name)
                .Id(propertyInfo.Name);

            if (selectType.Type == SelectType.Multiple)
            {
                selekt.Attr("multiple", "multiple");
            }

            if (propertyInfo.PropertyType.IsEnum)
            {
                var ge = (GlobalizedEnumAttribute)propertyInfo.PropertyType.GetCustomAttributes(typeof(GlobalizedEnumAttribute), false).SingleOrDefault();

                var fields = modelValue.GetType().GetFields();

                for (int i = 1; i < fields.Length; i++)
                {
                    var f = fields[i];
                    string name = string.Empty;
                    if (ge == null)
                    {
                        name = f.GetValue(f.Name).ToString();
                    }
                    else
                    {
                        name = ge.GetName(f.Name);
                    }
                    var tag = new HtmlTag("option")
                        .Text(name)
                        .Attr("value", f.Name);
                    if (formAction != FormAction.Create && f.Name == modelValue.ToString())
                    {
                        tag.Attr("selected", "selected");
                    }
                    selekt.Append(tag);

                }
            }
            else
            {
                foreach (var i in (modelValue as IEnumerable))
                {
                    var tag = new HtmlTag("option")
                        .Text(i.ToString())
                        .Attr("value", i.GetValue(selectType.ValueMember));
                    if (formAction != FormAction.Create && i.ToString() == modelValue.ToString())
                    {
                        tag.Attr("selected", "selected");
                    }
                    selekt.Append(tag);
                }
            }

            FillValidation<TModel>(selekt, propertyInfo);

            cg.Children[1].InsertFirst(selekt);

            return cg;
        }

        static HtmlTag GenerateObjectList<TModel>(TModel model, PropertyInfo member, ObjectListConfig objList, FormAction formAction)
        {
            var modelValue = member.GetValue(model, null);

           
            //<div class="control-group">
            //  <label class="control-label" for="Email">Email:</label>
            //  <div class="controls">
            //    <select type="checkbox" class="input-xlarge" data-val="true" 
            //        data-val-email="O e-mail digitado não é válido." 
            //        data-val-required="O campo E-mail é obrigatório." 
            //        id="Email" name="Email" value="@Model.Email">
            //       <option value="volvo">Volvo</option>
            //       <option value="saab">Saab</option>
            //    </select>
            //    <span class="field-validation-valid error" data-valmsg-for="Email" 
            //        data-valmsg-replace="true"></span>
            //    <p class="help-block">Ex.: jose@unicentro.br</p>
            //  </div>
            //</div>

            var cg = CreateControlGroup(model, member);

            var selekt = new SelectTag()
                .Attr("name", member.Name)
                    .Id(member.Name);

            if (objList.SelectType == SelectType.Multiple)
            {
                selekt.Attr("multiple", "multiple");
            }

            foreach (var i in objList.Objects)
            {
                var value = i.GetValue(objList.ValueMember);
                var tag = new HtmlTag("option")
                    .Text(i.ToString())
                        .Attr("value", value);
                if (formAction != FormAction.Create && 
                    modelValue != null && 
                    value.ToString() == modelValue.ToString())
                {
                    tag.Attr("selected", "selected");
                }
                selekt.Append(tag);
            }

            FillValidation<TModel>(selekt, member);

            cg.Children[1].InsertFirst(selekt);

            return cg;
        }

        static HtmlTag GenerateCalendar<TModel>(TModel model, PropertyInfo member, FormAction formAction)
        {
            var modelValue = formAction == FormAction.Create ? "" : member.GetValue(model, null).ToString();

            if (modelValue == null)
            {
                return null;
            }
          
            var cg = CreateControlGroup(model, member);

            var textbox = new TextboxTag(member.Name, modelValue)
                .AddClass("datepicker")
                .Id(member.Name);

           

            FillValidation<TModel>(textbox, member);

            cg.Children[1].InsertFirst(textbox);

            return cg;
        }

        static HtmlTag CreateControlGroup<TModel>(TModel model, PropertyInfo member)
        {
            //<div class="control-group">
            //  <label class="control-label" for="Email">Email:</label>
            //  <div class="controls">
            //    <input type="checkbox" class="input-xlarge" data-val="true" 
            //        data-val-email="O e-mail digitado não é válido." 
            //        data-val-required="O campo E-mail é obrigatório." 
            //        id="Email" name="Email" value="@Model.Email">
            //    <span class="field-validation-valid error" data-valmsg-for="Email" 
            //        data-valmsg-replace="true"></span>
            //    <p class="help-block">Ex.: jose@unicentro.br</p>
            //  </div>
            //</div>

            var controlGroup = new DivTag().AddClass("control-group");

            controlGroup.Children.Add(
                new HtmlTag("label")
                    .AddClass("control-label")
                    .Attr("for", member.Name)
                    .Text(model.GetAttributeValue(member.Name, 
                                                  "Display", 
                                                  "Name") + ":"
            )
            );
            var controls = new DivTag().AddClass("controls");
            controlGroup.Children.Add(controls);

            controls.Children.Add(
                new HtmlTag("span")
                    .AddClasses("field-validation-valid help-inline")
                    .Data("valmsg-for", member.Name)
                    .Data("valmsg-replace", true)
            );
            controls.Children.Add(
                new HtmlTag("p")
                    .AddClass("help-block")
                    .Text(model.GetAttributeValue(member.Name, 
                                                  "Display", 
                                                  "Description").ToString()
            )
            );
            return controlGroup;
        }

        /// <summary>
        /// Fills the validation.
        /// All the validators class must folows the pattern ModelName + Validator
        /// </summary>
        /// <param name='tag'>
        /// Tag.
        /// </param>
        static void FillValidation<TModel>(HtmlTag tag, PropertyInfo member)
        {

            var fullName = member.Module.Assembly.FullName;  //Assembly.GetExecutingAssembly ().FullName;
            var typeFullName = typeof(TModel).FullName;
            var validator = 
                (AbstractValidator<TModel>)Activator.CreateInstance(fullName, 
                                                                    typeFullName + "Validator").Unwrap();
            var descriptor = validator.CreateDescriptor();
            var members = descriptor.GetMembersWithValidators();
            foreach (var m in members)
            {
                if (m.Key != member.Name)
                    continue;
                var rules = descriptor.GetRulesForMember(m.Key);
                foreach (PropertyRule r in rules)
                {
                    if (string.IsNullOrEmpty(r.RuleSet))
                    {
                        FillValidation<TModel>(tag, r);
                    }
                }
            }
        }

        static void FillValidation<TModel>(HtmlTag tag, PropertyRule r)
        {
            tag.Data("val", "true");
            foreach (var v in r.Validators)
            {
                var message = v.ErrorMessageSource;
                var messageFormatter = new MessageFormatter();
                messageFormatter.AppendPropertyName(r.DisplayName.GetString());
                var formattedMessage = messageFormatter.BuildMessage(message.GetString());
                if (v is NotEmptyValidator)
                {
                    tag.Data("val-required", formattedMessage);
                }
                else if (v is EmailValidator)
                {
                    tag.Data("val-email", formattedMessage);
                }
                else if (v is InclusiveBetweenValidator)
                {
                    var ibv = v as InclusiveBetweenValidator;
                    if (r.TypeToValidate == typeof(DateTime))
                    {
                        tag.Data("val-daterange", formattedMessage);
                        tag.Data("val-daterange-min", ((DateTime)ibv.From).ToShortDateString());
                        tag.Data("val-daterange-max", ((DateTime)ibv.To).ToShortDateString());
                    }
                    else
                    {
                        tag.Data("val-range", formattedMessage);
                        tag.Data("val-range-min", ibv.From);
                        tag.Data("val-range-max", ibv.To);
                    }
                }
                else if (v is RegularExpressionWithMaskValidator)
                {
                    tag.Data("val-regexwithmask", formattedMessage);
                    tag.Data("val-regexwithmask-pattern", (v as RegularExpressionWithMaskValidator).Expression);
                    tag.Data("val-regexwithmask-mask", (v as RegularExpressionWithMaskValidator).Mask);
                    tag.Data("mask", (v as RegularExpressionWithMaskValidator).Mask);
                }
                else if (v is RegularExpressionValidator)
                {
                    tag.Data("val-regex", formattedMessage);
                    tag.Data("val-regex-pattern", (v as RegularExpressionValidator).Expression);
                }
                else if (v is LengthValidator)
                {
                    var lv = v as LengthValidator;
                    var length = lv.Max > 30 ? 30 : lv.Max;
                    tag.Style("width", length + "em");
                    tag.Attr("maxlength", lv.Max);
                    tag.Data("val-length", formattedMessage
                                .Replace("PropertyName", "0")
                                .Replace("MinLength", "1")
                                .Replace("MaxLength", "2")
                                .Replace("TotalLength", "3")
                    );
                    tag.Data("val-length-max", lv.Max);
                    tag.Data("val-length-min", lv.Min);
                }
                else if (v is RemoteValidator)
                {
                    var rv = v as RemoteValidator;
                    tag.Data("val-remote", formattedMessage);
                    tag.Data("val-remote-additionalfields", rv.AdditionalFields);
                    tag.Data("val-remote-type", rv.HttpMethod);
                    tag.Data("val-remote-url", rv.Action);
                }
                else
                {
                    foreach (ICommand cmd in AddinManager.GetExtensionObjects<ICommand> ())
                    {
                        cmd.Run(v, tag, formattedMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Render an input hidden from a model.
        /// </summary>
        /// <returns>
        /// An <see cref="IHtmlString"/> object.
        /// </returns>
        /// <param name='model'>
        /// Model.
        /// </param>
        /// <param name='objectProperty'>
        /// Object property.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString InputHiddenFor<TModel>(TModel model, 
                                                         Expression<Func<TModel, object>> objectProperty,
                                                         FormAction formAction)
        {
            var member = GetMember<TModel>(objectProperty);
            if (member == null)
                return null;
            var tag = GenerateInputText(model, member, formAction);
            return new NonEncodedHtmlString(tag == null ? "" : tag.ToString());
        }

        /// <summary>
        /// Render an input hidden from a model.
        /// </summary>
        /// <returns>
        /// An <see cref="IHtmlString"/> object.
        /// </returns>
        /// <param name='model'>
        /// Model.
        /// </param>
        /// <param name='propertyName'>
        /// Property name.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString InputHiddenFor<TModel>(TModel model, 
                                                         string propertyName, 
                                                         FormAction formAction)
        {
            var member = typeof(TModel).GetProperty(propertyName, 
                                                    BindingFlags.Public | BindingFlags.Instance);
            if (member == null)
                return new NonEncodedHtmlString("");
            var tag = GenerateInputText(model, member, formAction);
            return new NonEncodedHtmlString(tag == null ? "" : tag.ToString());
        }

        static HtmlTag GenerateInputHidden<TModel>(TModel model, PropertyInfo member, 
                                                   FormAction formAction)
        {
            var modelValue = member.GetValue(model, null);

            if (modelValue == null)
            {
                return null;
            }

            //<input type="hidden" name="member.Name" id="member.Name" value="modelValue.ToString ()" />
            var input = new HiddenTag()
                .Attr("name", member.Name)
                .Attr("value", modelValue.ToString())
                .Id(member.Name);

            FillValidation<TModel>(input, member);

            return input;
        }

        /// <summary>
        /// Render a list of labels from a model.
        /// </summary>
        /// <returns>
        /// An <see cref="IHtmlString"/> object.
        /// </returns>
        /// <param name='model'>
        /// Model.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString LabelsFor<TModel>(TModel model)
        {
            var div = GenerateLabels(model);
            var sb = new StringBuilder();
            foreach (var i in div.Children)
            {
                sb.Append(i.ToHtmlString());
            }
            return new NonEncodedHtmlString(div == null ? "" : sb.ToString());
        }

        static HtmlTag GenerateLabels<TModel>(TModel model)
        {
            var ul = new HtmlTag("div").AddClass("fields");
            
            foreach (var prop in model.GetType().GetProperties())
            {
                var scaffold = model.GetAttribute<VisibilityAttribute>(prop);
                if (scaffold == null)
                { 
                    continue;
                }
                if (scaffold.Read == Visibility.Show)
                {
                    var p = GenerateLabel(model, prop);
                    if (p != null)
                    {
                        ul.Append(p);
                    }
                }
            }
            if (ul.Children.Count == 0)
            {
                return null;
            }
            return ul;
            
        }

        static HtmlTag GenerateLabel<TModel>(TModel model, PropertyInfo member)
        {
            var modelValue = string.Empty;

            if (member.PropertyType.IsEnum)
            {
                var ge = (GlobalizedEnumAttribute)member.PropertyType.GetCustomAttributes(typeof(GlobalizedEnumAttribute), false).SingleOrDefault();
                if (ge == null)
                {
                    modelValue = member.GetValue(model, null).ToString();
                }
                else
                {
                    modelValue = ge.GetName(member.GetValue(model, null).ToString());
                }
            }
            else
            {
                modelValue = member.GetValue(model, null).ToString();
            }

            var p = new HtmlTag("p").AddClass("field");

            var labelText = model.GetAttributeValue(member.Name, 
                                                    "Display", 
                                                    "Name") + ": ";
            p.Append(new HtmlTag("span")
                .AddClass("field-label")
                .Text(labelText ?? member.Name)
            );
            p.Append(new HtmlTag("span")
                .AddClass("field-value")
                 .Text(modelValue)
            );
            return p;
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <returns>The member.</returns>
        /// <param name="objectProperty">Object property.</param>
        /// <typeparam name="TModel">The 1st type parameter.</typeparam>
        static PropertyInfo GetMember<TModel>(Expression<Func<TModel, object>> objectProperty)
        {
            var lambda = objectProperty as LambdaExpression;
            if (lambda == null)
            {
                return null;
            }

            var property = lambda.Body as MemberExpression;
            if (property == null)
            {
                return null;
            }
            var member = property.Member as PropertyInfo;
            if (member == null)
            {
                return null;
            }
            return member;
        }
    }
}
