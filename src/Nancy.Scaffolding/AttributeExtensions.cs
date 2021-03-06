//
// AttributeExtensions.cs
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
    using System.Reflection;

    public static class ReflectionExtensions
    {

        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        public static object GetValue<T> (this T obj, 
                                          string propertyName)
        {
            if (obj == null)
                return null;
            var attrProperty = obj.GetType ().GetProperty (propertyName);
            if (attrProperty == null)
                return null;
            return attrProperty.GetValue (obj, null);
        }

    }

    public static class AttributeExtensions
    {


        public static TAttribute GetAttribute<TAttribute>  (this object obj, 
                                                 string propertyName)
        {
            return GetAttributeImpl <TAttribute>(obj, obj.GetType().GetProperty(propertyName));
        }

        public static TAttribute GetAttribute<TAttribute> (this object obj, 
                                                 PropertyInfo property)
        {
            return GetAttributeImpl <TAttribute>(obj, property);
        }

        static TAttribute GetAttributeImpl<TAttribute> (object obj, PropertyInfo property)
        {
            if (obj == null)
                return default(TAttribute);
            var atts = property.GetCustomAttributes (true);
            if (atts.Length == 0) {
                return default(TAttribute);
            }
            foreach (var a in atts) {
                if (a.GetType () == typeof(TAttribute)) {
                    return (TAttribute)a;
                }
            }
            return default(TAttribute);
        }


        //-------------------


        public static object GetAttributeValue<T> (this T obj, 
                                                   string propertyName, 
                                                   string attribute,
                                                   string attributeProperty)
        {
            if (obj == null)
                return null;
            return GetAttributeValue (obj.GetType (), propertyName, 
                                      attribute, attributeProperty);
        }

        public static object GetAttributeValue<T> (this T obj, 
                                                   PropertyInfo property, 
                                                   Type attribute,
                                                   string attributeProperty)
        {
            if (obj == null)
                return null;
            return GetAttributeValue (property, attribute, attributeProperty);
        }

        static object GetAttributeValue (Type type, string propertyName, 
                                         string attribute,
                                         string attributeProperty)
        {
            var property = type.GetProperty (propertyName);
            if (property == null)
                return null;

            return GetAttributeValue (property, 
                                     attribute, 
                                     attributeProperty);
        }

        static object GetAttributeValue (PropertyInfo property, 
                                        string attribute,
                                        string attributeProperty)
        {
            var atts = property.GetCustomAttributes (true);
            if (atts.Length == 0)
                return null;
            foreach (var a in atts) {
                var attrType = (a as Attribute).GetType ();
                if (attrType.Name == attribute + "Attribute") {
                    var attrProperty = attrType.GetProperty (attributeProperty);
                    if (attrProperty == null)
                        return null;
                    return attrProperty.GetValue (a, null);
                }
            }
            return null;
        }

        static object GetAttributeValue (PropertyInfo property, 
                                        Type attribute,
                                        string attributeProperty)
        {
            var atts = property.GetCustomAttributes (true);
            if (atts.Length == 0)
                return null;
            foreach (var a in atts) {
                if (a.GetType () == attribute) {
                    var attrProperty = attribute.GetProperty (attributeProperty);
                    if (attrProperty == null)
                        return null;
                    return attrProperty.GetValue (a, null);
                }
            }
            return null;
        }
    }
}

