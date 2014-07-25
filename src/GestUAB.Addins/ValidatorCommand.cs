//
// ValidatorCommand.cs
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

namespace GestUAB.Addins
{
    using System;
    using FluentValidation.Validators;
    using GestUAB.Models;
    using HtmlTags;
    using Mono.Addins;
    using Nancy.Scaffolding;

    /// <summary>
    /// Validator command.
    /// </summary>
    [Extension]
    public class ValidatorCommand : ICommand
    {
        #region ICommand implementation
        /// <summary>
        /// Run the specified validator, tag and formattedMessage.
        /// </summary>
        /// <param name="validator">A validator.</param>
        /// <param name="tag">A tag.</param>
        /// <param name="formattedMessage">Formatted message.</param>
        public void Run(FluentValidation.Validators.IPropertyValidator validator, HtmlTag tag, string formattedMessage)
        {
            if (validator is CpfValidator)
            {
                tag.Data("val-cpf", formattedMessage);
            }
            else if (validator is ValidDate)
            {
                tag.Data("val-dateBR", formattedMessage);
            }
            else if (validator is ValidNomeConjuge)
            {
                tag.Data("val-nomeConjuge", formattedMessage);
            }
        }
        #endregion
    }
}
