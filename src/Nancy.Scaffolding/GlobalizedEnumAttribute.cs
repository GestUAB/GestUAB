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
namespace Nancy.Scaffolding
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Globalized enum attribute.
    /// </summary>
    public class GlobalizedEnumAttribute : Attribute
    {
        /// <summary>
        /// The _names.
        /// </summary>
        Dictionary<string, string> _names = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Nancy.Scaffolding.GlobalizedEnumAttribute"/> class.
        /// </summary>
        /// <param name="type">The Type.</param>
        /// <param name="names">The Names.</param>
        public GlobalizedEnumAttribute(Type type, params string[] names)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException("The type parameter must be of type Enum.", "type");
            }

            var fields = type.GetFields();
            for (int i = 1; i < fields.Length; i++)
            {
                var f = fields[i];
                _names.Add(f.Name, names[i - 1]);
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="name">The Name.</param>
        public string GetName(string name)
        {
            return _names[name];
        }
    }
}
