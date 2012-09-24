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
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HtmlTags;
using Nancy.ViewEngines.Razor;
using FluentValidation;
using FluentValidation.Validators;
using FluentValidation.Internal;
using GestUAB.Validators;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace GestUAB
{
    /// <summary>
    /// Html tags extensions class.
    /// This class is capable to render html elements in a razor page.
    /// Basically you can scaffold any model to an input or list of inputs. 
    /// </summary>
    public static class HtmlTagsExtensions
    {
        static PropertyInfo GetMember<TModel> (Expression<Func<TModel, object>> objectProperty)
        {
            var lambda = objectProperty as LambdaExpression;
            if (lambda == null) {
                return null;
            }
            var property = lambda.Body as MemberExpression;
            if (property == null) {
                return null;
            }
            var member = property.Member as PropertyInfo;
            if (member == null) {
                return null;
            }
            return member;
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
        /// <param name='formType'>
        /// The type of form. <see cref="FormType"/>
        /// </param>
        /// <param name='formTag'>
        /// An outter form tag which will enclose the inputs.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString FormFor<TModel> (TModel model, FormType formType, FormTag formTag)
        {
            var form = GenerateForm (model, formType, formTag);
            return new NonEncodedHtmlString (form == null ? "" : form.ToString ());
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
        /// <param name='formType'>
        /// The type of form. <see cref="FormType"/>
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString FormFor<TModel> (TModel model, FormType formType)
        {
            var form = GenerateForm (model, formType, new FormTag ());
            return new NonEncodedHtmlString (form == null ? "" : form.ToString ());
        }

        static HtmlTag GenerateForm<TModel> (TModel model, FormType formType, FormTag formTag)
        {
            foreach (var prop in model.GetType().GetProperties()) {
                //var value = prop.GetValue (model, null);
                var visibility = (ScaffoldVisibilityType)model.GetAttributeValue (prop, typeof(ScaffoldVisibilityAttribute), formType.ToString ());
                if (visibility == ScaffoldVisibilityType.None) {
                    continue;
                }
                if (visibility == ScaffoldVisibilityType.Show) {
                    var type = prop.PropertyType;
                    if (type == typeof(string)) {
                        formTag.Append (GenerateInputText<TModel> (model, prop));
                    } else if (type == typeof(bool)) {
                        formTag.Append (GenerateInputCheck<TModel> (model, prop));
                    } else if (typeof(IEnumerable).IsAssignableFrom (type)) {
                        formTag.Append (GenerateSelect<TModel> (model, prop));
                    } else if (type.IsEnum) {
                        formTag.Append (GenerateSelect<TModel> (model, prop));
                    }
                } else {
                    formTag.Append (GenerateInputHidden<TModel> (model, prop));
                }
            }
            if (formTag.Children.Count == 0) {
                return null;
            }
            return formTag;
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
        /// <param name='formType'>
        /// Form type.
        /// </param>
        /// <typeparam name='TModel'>
        /// The 1st type parameter.
        /// </typeparam>
        public static IHtmlString FieldsFor<TModel> (TModel model, FormType formType)
        {
            var form = GenerateFields (model, formType);
            return new NonEncodedHtmlString (form == null ? "" : form.ToString ());
        }

//        public static IHtmlString FildsFor<TModel> (TModel model, FormType formType)
//        {
//            var form = GenerateForm (model, formType, new FormTag());
//            return new NonEncodedHtmlString (form == null ? "" : form.ToString()) ;
//        }

        static HtmlTag GenerateFields<TModel> (TModel model, FormType formType)
        {
            var tag = new DivTag ();
            foreach (var prop in model.GetType().GetProperties()) {
                //var value = prop.GetValue (model, null);
                var visibility = (ScaffoldVisibilityType)model.GetAttributeValue (prop, typeof(ScaffoldVisibilityAttribute), formType.ToString ());
                if (visibility == ScaffoldVisibilityType.None) {
                    continue;
                }
                if (visibility == ScaffoldVisibilityType.Show) {
                    var type = prop.PropertyType;
                    if (type == typeof(string)) {
                        tag.Append (GenerateInputText<TModel> (model, prop));
                    } else if (type == typeof(bool)) {
                        tag.Append (GenerateInputCheck<TModel> (model, prop));
                    } else if (typeof(IEnumerable).IsAssignableFrom (type)) {
                        tag.Append (GenerateSelect<TModel> (model, prop));
                    } else if (type.IsEnum) {
                        tag.Append (GenerateSelect<TModel> (model, prop));
                    }

                } else {
                    tag.Append (GenerateInputHidden<TModel> (model, prop));
                }
            }
            if (tag.Children.Count == 0) {
                return null;
            }
            return tag;
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
        public static IHtmlString InputTextFor<TModel> (TModel model, 
            Expression<Func<TModel, object>> objectProperty)
        {
            var member = GetMember<TModel> (objectProperty);
            if (member == null)
                return new NonEncodedHtmlString ("");
            var tag = GenerateInputText (model, member);
            return new NonEncodedHtmlString (tag == null ? "" : tag.ToString ());
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
        public static IHtmlString InputTextFor<TModel> (TModel model, 
            string propertyName)
        {
            var member = typeof(TModel).GetProperty (propertyName, 
                BindingFlags.Public | BindingFlags.Instance);
            if (member == null)
                return new NonEncodedHtmlString ("");
            var tag = GenerateInputText (model, member);
            return new NonEncodedHtmlString (tag == null ? "" : tag.ToString ());
        }

        static HtmlTag GenerateInputText<TModel> (TModel model, PropertyInfo member)
        {
            var modelValue = member.GetValue (model, null);

            if (modelValue == null) {
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

            var cg = CreateControlGroup (model, member);

            var input = new TextboxTag (member.Name, modelValue.ToString ())
                .Id (member.Name);

            FillValidation<TModel> (input, member);

            cg.Children [1].InsertFirst (input);

            return cg;
        }

        static HtmlTag GenerateInputCheck<TModel> (TModel model, PropertyInfo member)
        {
            var modelValue = member.GetValue (model, null);

            if (modelValue == null) {
                return null;
            }

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
          
            var cg = CreateControlGroup (model, member);

            var input = new CheckboxTag ((bool)modelValue)
                .Attr ("name", member.Name)
                .Id (member.Name);

            FillValidation<TModel> (input, member);

            cg.Children [1].InsertFirst (input);

            return cg;
        }

        static Dictionary<string, string> _enumCache = new Dictionary<string, string> ();

        static HtmlTag GenerateSelect<TModel> (TModel model, PropertyInfo member)
        {
            var modelValue = member.GetValue (model, null);

            if (modelValue == null) {
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
          
            var cg = CreateControlGroup (model, member);

            var selectType = model.GetAttribute (member, 
                typeof(ScaffoldSelectPropertiesAttribute)) as ScaffoldSelectPropertiesAttribute;

            var selekt = new SelectTag ()
                .Attr ("name", member.Name)
                .Id (member.Name);

            if (selectType.Type == SelectType.Multiple) {
                selekt.Attr ("multiple", "multiple");
            }

            if (member.PropertyType.IsEnum) {
                var atts = modelValue.GetType ().GetCustomAttributes (true);
                GlobalizedEnumAttribute ge = null;
                if (atts.Length == 0)
                    ge = null;
                foreach (var a in atts) {
                    if (a.GetType () == typeof(GlobalizedEnumAttribute)) {
                        ge = (GlobalizedEnumAttribute)a;
                    }
                }
                var fields = modelValue.GetType().GetFields();
                for (int i = 1; i < fields.Length; i++) {
                    var f = fields[i];
                    string name = string.Empty;
                    if (ge == null) {
                        name = f.GetValue (f.Name).ToString ();
                    } else {
                        name = ge.GetName (f.Name);
                    }
                    var tag = new HtmlTag ("option").Text (name);
                    selekt.Append (tag);
                    tag.Attr ("value", f.Name);

                }
            } else {
                foreach (var i in (modelValue as IEnumerable)) {
                    var tag = new HtmlTag ("option").Text (i.ToString ());
                    selekt.Append (tag);
                    tag.Attr ("value", i.GetValue (selectType.ValueMember));
                }
            }


            FillValidation<TModel> (selekt, member);

            cg.Children [1].InsertFirst (selekt);

            return cg;
        }

        static HtmlTag CreateControlGroup<TModel> (TModel model, PropertyInfo member)
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

            var controlGroup = new DivTag ().AddClass ("control-group");

            controlGroup.Children.Add (
                new HtmlTag ("label")
                    .AddClass ("control-label")
                    .Attr ("for", member.Name)
                    .Text (model.GetAttributeValue (member.Name, 
                                                  "Display", 
                                                  "Name") + ":"
            )
            );
            var controls = new DivTag ().AddClass ("controls");
            controlGroup.Children.Add (controls);

            controls.Children.Add (
                new HtmlTag ("span")
                    .AddClasses ("field-validation-valid help-inline", "error")
                    .Data ("valmsg-for", member.Name)
                    .Data ("valmsg-replace", true)
            );
            controls.Children.Add (
                new HtmlTag ("p")
                    .AddClass ("help-block")
                    .Text (model.GetAttributeValue (member.Name, 
                                                  "Display", 
                                                  "Description").ToString ()
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
        static void FillValidation<TModel> (HtmlTag tag, PropertyInfo member)
        {
            var fullName = Assembly.GetExecutingAssembly ().FullName;
            var typeFullName = typeof(TModel).FullName;
            var validator = 
                (AbstractValidator<TModel>)Activator.CreateInstance (fullName, 
                                        typeFullName + "Validator").Unwrap ();
            var descriptor = validator.CreateDescriptor ();
            var members = descriptor.GetMembersWithValidators ();
            foreach (var m in members) {
                if (m.Key != member.Name)
                    continue;
                var rules = descriptor.GetRulesForMember (m.Key);
                foreach (var r in rules) {
                    FillValidation<TModel> (tag, r as PropertyRule);
                }
            }
        }

        static void FillValidation<TModel> (HtmlTag tag, PropertyRule r)
        {
            tag.Data ("val", "true");
            foreach (var v in r.Validators) {
                var message = v.ErrorMessageSource;
                var messageFormatter = new MessageFormatter ();
                messageFormatter.AppendPropertyName (r.DisplayName.GetString ());
                var formattedMessage = messageFormatter.BuildMessage (message.GetString ());
                if (v is NotEmptyValidator) {
                    tag.Data ("val-required", formattedMessage);
                }
                if (v is EmailValidator) {
                    tag.Data ("val-email", formattedMessage);
                }
                if (v is RegularExpressionValidator) {
                    tag.Data ("val-regex", formattedMessage);
                    tag.Data ("val-regex-pattern", (v as RegularExpressionValidator).Expression);
                }
                if (v is LengthValidator) {
                    var lv = v as LengthValidator;
                    tag.Data ("val-length", formattedMessage
                                .Replace ("PropertyName", "0")
                                .Replace ("MinLength", "1")
                                .Replace ("MaxLength", "2")
                                .Replace ("TotalLength", "3")
                    );
                    tag.Data ("val-length-max", lv.Max);
                    tag.Data ("val-length-min", lv.Min);
                }
                if (v is RemoteValidator) {
                    var rv = v as RemoteValidator;
                    tag.Data ("val-remote", formattedMessage);
                    tag.Data ("val-remote-additionalfields", rv.AdditionalFields);
                    tag.Data ("val-remote-type", rv.HttpMethod);
                    tag.Data ("val-remote-url", rv.Action);
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
        public static IHtmlString InputHiddenFor<TModel> (TModel model, 
            Expression<Func<TModel, object>> objectProperty)
        {
            var member = GetMember<TModel> (objectProperty);
            if (member == null)
                return null;
            var tag = GenerateInputText (model, member);
            return new NonEncodedHtmlString (tag == null ? "" : tag.ToString ());
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
        public static IHtmlString InputHiddenFor<TModel> (TModel model, 
            string propertyName)
        {
            var member = typeof(TModel).GetProperty (propertyName, 
                BindingFlags.Public | BindingFlags.Instance);
            if (member == null)
                return new NonEncodedHtmlString ("");
            var tag = GenerateInputText (model, member);
            return new NonEncodedHtmlString (tag == null ? "" : tag.ToString ());
        }

        static HtmlTag GenerateInputHidden<TModel> (TModel model, PropertyInfo member)
        {
            var modelValue = member.GetValue (model, null);

            if (modelValue == null) {
                return null;
            }

            //<input type="hidden" name="member.Name" id="member.Name" value="modelValue.ToString ()" />
            var input = new HiddenTag ()
                .Attr ("name", member.Name)
                .Attr ("value", modelValue.ToString ())
                .Id (member.Name);

            FillValidation<TModel> (input, member);

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
        public static IHtmlString LabelsFor<TModel> (TModel model)
        {
            var div = GenerateLabels (model);
            var sb = new StringBuilder ();
            foreach (var i in div.Children) {
                sb.Append (i.ToHtmlString ());
            }
            return new NonEncodedHtmlString (div == null ? "" : sb.ToString ());
        }

        static HtmlTag GenerateLabels<TModel> (TModel model)
        {
            var ul = new HtmlTag ("div").AddClass ("fields");
            
            foreach (var prop in model.GetType().GetProperties()) {
                var scaffold = model.GetAttribute (prop, typeof(ScaffoldVisibilityAttribute)) as ScaffoldVisibilityAttribute;
                if (scaffold.Read == ScaffoldVisibilityType.Show) {
                    var p = GenerateLabel (model, prop);
                    if (p != null) {
                        ul.Append (p);
                    }
                }
            }
            if (ul.Children.Count == 0) {
                return null;
            }
            return ul;
            
        }

        static HtmlTag GenerateLabel<TModel> (TModel model, PropertyInfo member)
        {
            var modelValue = member.GetValue (model, null);

            if (modelValue == null) {
                return null;
            }
            var p = new HtmlTag ("p").AddClass ("field");

            //<input type="hidden" name="member.Name" id="member.Name" value="modelValue.ToString ()" />
            var labelText = model.GetAttributeValue (member.Name, 
                                               "Display", 
                                                "Name") + ": ";
            p.Append (new HtmlTag ("span")
                .AddClass ("field-label")
                .Text (labelText ?? member.Name)
            );
            p.Append (new HtmlTag ("span")
                .AddClass ("field-value")
                 .Text (modelValue.ToString ())
            );
            return p;
        }

    }
}

