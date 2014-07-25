// //-----------------------------------------------------------------------
// // <copyright file="GrandeAreaConhecimentoType.cs" company="Tony Alexander Hild">
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
    [GlobalizedEnum(typeof(GrandeAreaConhecimentoType), 
                    "Ciências Exatas e da Terra",
                    "Ciências Biológicas",
                    "Engenharias",
                    "Ciências da Saúde",
                    "Ciências Agrárias",
                    "Ciências Sociais Aplicadas",
                    "Ciências Humanas",
                    "Linguística, Letras e Artes")]
    public enum GrandeAreaConhecimentoType : short
    {
        /// <summary>
        /// The ciencias exatas terra.
        /// </summary>
        CienciasExatasTerra,

        /// <summary>
        /// The ciencias biologicas.
        /// </summary>
        CienciasBiologicas,

        /// <summary>
        /// The engenharias.
        /// </summary>
        Engenharias,

        /// <summary>
        /// The ciencias saude.
        /// </summary>
        CienciasSaude,

        /// <summary>
        /// The ciencias agrarias.
        /// </summary>
        CienciasAgrarias,

        /// <summary>
        /// The ciencias sociais aplicadas.
        /// </summary>
        CienciasSociaisAplicadas,

        /// <summary>
        /// The ciencias humanas.
        /// </summary>
        CienciasHumanas,

        /// <summary>
        /// The linguística letras artes.
        /// </summary>
        LinguísticaLetrasArtes
    }
}
