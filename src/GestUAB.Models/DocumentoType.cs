// //-----------------------------------------------------------------------
// // <copyright file="TipoDocumentoType.cs" company="Tony Alexander Hild">
// //     Tony Alexander Hild <tony_hild@yahoo.com>
// // </copyright>
// //-----------------------------------------------------------------------
//
// TipoDocumentoType.cs
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
    using System.ComponentModel.DataAnnotations;
    using Nancy.Scaffolding;
    using Nancy.Scaffolding.Validators;

    /// <summary>
    /// Tipo documento type.
    /// </summary>
    [GlobalizedEnum(typeof(DocumentoType), 
                    "Carteira de identidade",
                    "Carteira de trabalho",
                    "Carteira profissional",
                    "Passaporte",
                    "Carteria de identificação funcional",
                    "Outro")]
    public enum DocumentoType : short
    {
        /// <summary>
        /// The carteira identidade.
        /// </summary>
        CarteiraIdentidade,

        /// <summary>
        /// The carteira trabalho.
        /// </summary>
        CarteiraTrabalho,

        /// <summary>
        /// The carteira profissional.
        /// </summary>
        CarteiraProfissional,

        /// <summary>
        /// The passaporte.
        /// </summary>
        Passaporte,

        /// <summary>
        /// The carteira identificacao funcional.
        /// </summary>
        CarteiraIdentificacaoFuncional,

        /// <summary>
        /// The outro.
        /// </summary>
        Outro
    }
}
