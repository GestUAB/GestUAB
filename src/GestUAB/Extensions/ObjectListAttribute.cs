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
using System;
using System.Collections.Generic;

namespace GestUAB
{
    public interface IObjectList
    {
        Func<IEnumerable<object>> GetList{ get; }
    }

    public class ObjectListAttribute : Attribute
    {

        public Func<IEnumerable<object>> GetList {
            get;
            private set;
        }

        public ObjectListAttribute (Type listType, Type objectType, string valueMember, SelectType selectType = SelectType.Single)
        {
            if (!listType.IsAssignableFrom (typeof(IObjectList))) {
                throw new ArgumentException ("type", "The type parameter must be of type IObjectList.");
            }
            GetList = listType.GetProperty ("GetList").GetValue (null) as Func<IEnumerable<object>>;
            SelectType = selectType;
            ValueMember = valueMember;
            ObjectType = objectType;
        }

        public SelectType SelectType { get; set; }

        public string ValueMember { get; set; }

        public Type ObjectType  { get; set; }

    }
}

