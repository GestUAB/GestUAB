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
    using Nancy.Scaffolding;

    /// <summary>
    /// Tipo documento type.
    /// </summary>
    [GlobalizedEnum(typeof(CursoType), 
                    "Aperfeiçoamento", 
                    "Bacharelado", 
                    "Lato Sensu", 
                    "Licenciatura", 
                    "Extensão",
                    "Sequencial", 
                    "Tecnólogo", 
                    "Mestrado", 
                    "Doutorado",
                    "Outro")]
    public enum CursoType : short
    {
        /// <summary>
        /// The aperfeicoamento.
        /// </summary>
        Aperfeicoamento, 

        /// <summary>
        /// The bacharelado.
        /// </summary>
        Bacharelado, 

        /// <summary>
        /// The lato sensu.
        /// </summary>
        LatoSensu, 

        /// <summary>
        /// The licenciatura.
        /// </summary>
        Licenciatura, 

        /// <summary>
        /// The extensao.
        /// </summary>
        Extensao,

        /// <summary>
        /// The sequencial.
        /// </summary>
        Sequencial, 

        /// <summary>
        /// The tecnologo.
        /// </summary>
        Tecnologo, 

        /// <summary>
        /// The mestrado.
        /// </summary>
        Mestrado, 

        /// <summary>
        /// The doutorado.
        /// </summary>
        Doutorado,

        /// <summary>
        /// The outro.
        /// </summary>
        Outro
    }
}
