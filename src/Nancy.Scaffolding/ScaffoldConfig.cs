//
// FluentConfig.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2013 Tony Alexander Hild
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
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    //    /// <summary>
    //    /// Config builder.
    //    /// </summary>
    //    public class ConfigBuilder<T>
    //    {
    //        /// <summary>
    //        /// The _configs.
    //        /// </summary>
    //        private List<ScaffoldConfig> configs = new List<ScaffoldConfig>();
    //
    //        public Container<T> Build()
    //        {
    //            var container = new Container<T>();
    //            foreach (var config in configs)
    //            {
    //                container.Register(config.MemberInfo, config);
    //            }
    //
    //            return container;
    //        }
    //
    //        /// <summary>
    //        /// For the specified expression.
    //        /// </summary>
    //        /// <param name="expression">The Expression.</param>
    //        /// <typeparam name="T">The 1st type parameter.</typeparam>
    //        /// <typeparam name="TProperty">The 2nd type parameter.</typeparam>
    //        /// <returns>Return a scaffolding config.</returns>
    //        public ScaffoldConfig.ScaffoldConfigFluentInterface Register<TProperty>(Expression<Func<T, TProperty>> expression)
    //        { 
    //
    //            MemberInfo member = expression.GetMember<T, TProperty>();
    //            Func<T, TProperty> func = expression.Compile();
    //
    //            var config = new ScaffoldConfig
    //            {   
    //                MemberInfo = member, 
    //                LambdaExpression = func
    //            };
    //
    //            configs.Add(config);
    //
    //            return config.FluentInterface;
    //        }
    //    }


    // <summary>
    /// Config builder.
    /// </summary>
    public class ConfigContainer
    {
        /// <summary>
        /// The _configs.
        /// </summary>
        private Dictionary<string, ScaffoldConfig> configs = new Dictionary<string, ScaffoldConfig>();
        //        internal void Register(MemberInfo memberInfo, ScaffoldConfig config)
        //        {
        //            configs[memberInfo] = config;
        //        }
        /// <summary>
        /// For the specified expression.
        /// </summary>
        /// <param name="expression">The Expression.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="TProperty">The 2nd type parameter.</typeparam>
        /// <returns>Return a scaffolding config.</returns>
        public ScaffoldConfig.ScaffoldConfigFluentInterface Register<T, TProperty>(Expression<Func<T, TProperty>> property)
        { 

            MemberInfo memberInfo = property.GetMember<T, TProperty>();
            Func<T, TProperty> func = property.Compile();

            var config = new ScaffoldConfig
            {   
                MemberInfo = memberInfo, 
                LambdaExpression = func
            };
            configs.Add(memberInfo.ReflectedType.FullName + "." + memberInfo.Name, config);

            return config.FluentInterface;
        }

        internal ScaffoldConfig Register(PropertyInfo propertyInfo)
        { 
            var config = new ScaffoldConfig
            {   
                MemberInfo = propertyInfo, 
            };

            configs.Add(propertyInfo.ReflectedType.FullName + "." + propertyInfo.Name, config);

            return config;
        }

        public ScaffoldConfig GetConfig <T, TProperty>(Expression<Func<T, TProperty>> property)
        { 
            MemberInfo memberInfo = property.GetMember<T, TProperty>();

            ScaffoldConfig config = null;

            if (configs.TryGetValue(memberInfo.ReflectedType.FullName + "." + memberInfo.Name, out config))
            {
                return config;  
            } 
            return null;
        }

        internal ScaffoldConfig GetConfig (string property)
        { 
            ScaffoldConfig config = null;

            if (configs.TryGetValue(property, out config))
            {
                return config;  
            } 
            return null;
        }

    }

    /// <summary>
    /// Scaffolding config.
    /// </summary>
    public class ScaffoldConfig
    {

        public ScaffoldConfig()
        {
            this.InputType = InputType.Auto;
            this.VisibilityConfig = new VisibilityConfig();
            this.FluentInterface = new ScaffoldConfigFluentInterface(this);
        }

        public ScaffoldConfigFluentInterface FluentInterface { get; private set; }

        internal MemberInfo MemberInfo { get; set; }

        internal object LambdaExpression { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the input.
        /// </summary>
        /// <value>The type of the input.</value>
        public InputType InputType { get; set; }

        /// <summary>
        /// Gets or sets the scaffold select.
        /// </summary>
        /// <value>The scaffold select.</value>
        public SelectConfig SelectConfig { get; set; }

        /// <summary>
        /// Gets or sets the scaffold visibility.
        /// </summary>
        /// <value>The scaffold visibility.</value>
        public VisibilityConfig VisibilityConfig { get; set; }

        /// <summary>
        /// Gets or sets the object list config.
        /// </summary>
        /// <value>The object list config.</value>
        public ObjectListConfig ObjectListConfig { get; set; }

        /// <summary>
        /// http://randypatterson.com/2007/09/how-to-design-a-fluent-interface/
        /// </summary>
        public class ScaffoldConfigFluentInterface : IScaffoldFluentConfig
        {
            private readonly ScaffoldConfig config;

            public ScaffoldConfigFluentInterface(ScaffoldConfig config)
            {
                this.config = config;
            }
            public IScaffoldFluentConfig Named(string name)
            {
                config.Name = name;
                return this;
            }
            

            public IScaffoldFluentConfig Described(string description)
            {
                config.Description = description;
                return this;
            }

            public IScaffoldFluentConfig WithInputType(InputType inputType)
            {
                config.InputType = inputType;
                return this;
            }

            public IScaffoldFluentConfig WithSelectConfig(SelectConfig selectConfig)
            {
                config.SelectConfig = selectConfig;
                return this;
            }

            public IScaffoldFluentConfig WithEnumAsSelect(SelectType selectTpe)
            {
                config.SelectConfig = new SelectConfig {EnumAsSelect = true, 
                                                        Type = selectTpe};
                return this;
            }

            public IScaffoldFluentConfig WithVisibilityConfig(Visibility create = Visibility.None, 
                                                              Visibility read = Visibility.None,
                                                              Visibility update = Visibility.None,
                                                              Visibility delete = Visibility.None,
                                                              Visibility all = Visibility.None)
            {
                if (all != Visibility.None) {
                    config.VisibilityConfig.Create = all;
                    config.VisibilityConfig.Read = all;
                    config.VisibilityConfig.Update = all;
                    config.VisibilityConfig.Delete = all;
                } else {
                    config.VisibilityConfig.Create = create;
                    config.VisibilityConfig.Read = read;
                    config.VisibilityConfig.Update = update;
                    config.VisibilityConfig.Delete = delete;
                }
                return this;
            }

            public IScaffoldFluentConfig WithObjectListConfig(ObjectListConfig objectListConfig)
            {
                config.ObjectListConfig = objectListConfig;
                return this;
            }


        }
    }
}
