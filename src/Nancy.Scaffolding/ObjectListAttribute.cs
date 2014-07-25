//
// ObjectListAttribute.cs
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
    using System.Collections.Generic;
    using System.Reflection;

    public class ObjectListAttribute : Attribute
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="Nancy.Scaffolding.ScaffoldObjectList"/> class.
        /// </summary>
        /// <param name="listContainer">The type that contains the list of objects.</param>
        /// <param name="objectType">The list type of objects.</param>
        /// <param name="property">Property that returns the list.</param>
        /// <param name="valueMember">Value member of object which will be displayed.</param>
        /// <param name="selectType">Select type.</param>
        public ObjectListAttribute(Type listContainer, Type objectType, string methodName, string valueMember, SelectType selectType = SelectType.Single)
        {
//            if (!objectType.IsAssignableFrom (typeof(IObjectList))) {
//                throw new ArgumentException ("type", "The type parameter must be of type IObjectList.");
//            }
            var member = listContainer.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);

            if (member == null)
            {
                throw new ArgumentException("methodName", string.Format("There is no method {0} in type {1}.", 
                                                                      methodName, listContainer.Name));
            }

//            if (!member.GetGetMethod().ReturnType.IsAssignableFrom(objectType))
//            {
//                throw new ArgumentException("property", string.Format("The property {0} must return the type IEnumerable<{1}>.", 
//                                                                      memberName, objectType.Name));
//            }

            if (listContainer.IsStatic())
            {
                GetList = member.Invoke(null, null) as IEnumerable<object>;
            }
            else
            {
                var ctor = listContainer.GetConstructor(BindingFlags.Public, null, null, null);
                var ctors = listContainer.GetConstructors();
                
                if (ctors.Length == 0 || ctor == null)
                {
                    throw new ArgumentException("listType", string.Format("The type {0} must have a puclic constructor.", 
                                                                                       listContainer.Name));
                }

                var obj = Activator.CreateInstance(listContainer);
                GetList = member.Invoke(obj, null) as IEnumerable<object>;
            }

            SelectType = selectType;
            ValueMember = valueMember;
            ObjectType = objectType;
        }

        public IEnumerable<object> GetList
        {
            get;
            private set;
        }

        public SelectType SelectType { get; set; }

        public string ValueMember { get; set; }

        public Type ObjectType  { get; set; }
    }
}

