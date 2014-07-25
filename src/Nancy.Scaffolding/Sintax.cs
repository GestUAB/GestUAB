//
// ISintax.cs
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
using System;
using System.ComponentModel;

namespace Nancy.Scaffolding
{

    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public interface IFluentInterface {

        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        bool Equals(object obj);

        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        string ToString();
    }

    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public interface IScaffoldFluentConfig : IFluentInterface {
        IScaffoldFluentConfig Named (string name);
        IScaffoldFluentConfig Described (string description);
        IScaffoldFluentConfig WithInputType(InputType inputType);
        IScaffoldFluentConfig WithSelectConfig(SelectConfig selectConfig);
        IScaffoldFluentConfig WithEnumAsSelect(SelectType selectType = SelectType.Single);
        IScaffoldFluentConfig WithVisibilityConfig(Visibility create = Visibility.None, 
                                                   Visibility read = Visibility.None,
                                                   Visibility update = Visibility.None,
                                                   Visibility delete = Visibility.None,
                                                   Visibility all = Visibility.None);

        IScaffoldFluentConfig WithObjectListConfig(ObjectListConfig objectListConfig);

    }

    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public interface INamed : IFluentInterface
    {
        IScaffoldFluentConfig Named (string name);
    }

    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public interface IDescribed : IFluentInterface
    {
        IScaffoldFluentConfig Described (string description);
    }
}

