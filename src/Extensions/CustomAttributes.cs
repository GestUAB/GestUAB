//
// CustomAttributes.cs
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
using System.Collections.Generic;

namespace GestUAB
{
    public enum ScaffoldVisibilityType
    {
        Hidden,
        Show,
        None
    }

    public enum SelectType
    {
        Single,
        Multiple
    }

    public class ScaffoldSelectPropertiesAttribute : Attribute
    {
        public ScaffoldSelectPropertiesAttribute (string valueMember, SelectType type = SelectType.Single)
        {
            Type = type;
            ValueMember = valueMember;
        }

        public SelectType Type { get; set; }

        public string ValueMember { get; set; }
    }

    public class ScaffoldVisibilityAttribute : Attribute
    {
        public ScaffoldVisibilityType All { get; private set; }

        public ScaffoldVisibilityType Create { get; private set; }

        public ScaffoldVisibilityType Read { get; private set; }

        public ScaffoldVisibilityType Update { get; private set; }

        public ScaffoldVisibilityType Delete { get; private set; }

        public ScaffoldVisibilityAttribute (ScaffoldVisibilityType create = ScaffoldVisibilityType.None, 
                                            ScaffoldVisibilityType read = ScaffoldVisibilityType.None,
                                            ScaffoldVisibilityType update = ScaffoldVisibilityType.None,
                                            ScaffoldVisibilityType delete = ScaffoldVisibilityType.None,
                                            ScaffoldVisibilityType all = ScaffoldVisibilityType.None)
        {
            if (all != ScaffoldVisibilityType.None) {
                Create = all;
                Read = all;
                Update = all;
                Delete = all;
            } else {
                Create = create;
                Read = read;
                Update = update;
                Delete = delete;
            }
            All = all;
        }
    }

    public class GlobalizedEnumAttribute : Attribute {

        Dictionary<string, string> _names = new Dictionary<string, string>();

        public GlobalizedEnumAttribute (Type type, params string[] names){
            if (!type.IsEnum) {
                throw new ArgumentException("type", "The type parameter must be of type Enum.");
            }
            var fields = type.GetFields();
            for (int i = 1; i < fields.Length; i++) {
                var f = fields[i];
                _names.Add(f.Name, names[i - 1]);
            }
        }

        public string GetName(string name) {
            return _names[name];
        }
    }
}

