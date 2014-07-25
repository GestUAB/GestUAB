//
// EstadoCivilType.cs
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

namespace GestUAB.Models
{
    using System;
    using Nancy.Scaffolding;

    /// <summary>
    /// Estado civil type.
    /// </summary>
    [GlobalizedEnum(typeof(EstadoCivilType), 
                    "Solteiro", "Casado", "Separado", 
                    "Divorciado", "Viúvo", "União Estável")]
    public enum EstadoCivilType : short
    {
        /// <summary>
        /// The solteiro.
        /// </summary>
        Solteiro, 

        /// <summary>
        /// The casado.
        /// </summary>
        Casado, 

        /// <summary>
        /// The separado.
        /// </summary>
        Separado, 

        /// <summary>
        /// The divorciado.
        /// </summary>
        Divorciado, 

        /// <summary>
        /// The viuvo.
        /// </summary>
        Viuvo, 

        /// <summary>
        /// The uniao estavel.
        /// </summary>
        UniaoEstavel
    }
}
