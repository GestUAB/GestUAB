// //-----------------------------------------------------------------------
// // <copyright file="UfType.cs" company="Tony Alexander Hild">
// //     Tony Alexander Hild <tony_hild@yahoo.com>
// // </copyright>
// //-----------------------------------------------------------------------
//
// UfType.cs
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
    /// Uf type.
    /// </summary>
    [GlobalizedEnum(typeof(UfType), 
                    "Acre",
                    "Alagoas",
                    "Amapá",
                    "Amazonas",
                    "Bahia",
                    "Ceará",
                    "Distrito Federal",
                    "Espírito Santo",
                    "Goiás",
                    "Maranhão",
                    "Mato Grosso",
                    "Mato Grosso do Sul",
                    "Minas Gerais",
                    "Paraná",
                    "Paraíba",
                    "Pará",
                    "Pernambuco",
                    "Piauí",
                    "Rio de Janeiro",
                    "Rio Grande do Norte",
                    "Rio Grande do Sul",
                    "Rondônia",
                    "Roraima",
                    "Santa Catarina",
                    "Sergipe",
                    "São Paulo",
                    "Tocantins"
                    )]
    public enum UfType : short
    {
        /// <summary>
        /// Estado do Acre
        /// </summary>
        AC, 

        /// <summary>
        /// Estado de Alagoas
        /// </summary>
        AL, 

        /// <summary>
        /// Estado de Amapá
        /// </summary>
        AP, 

        /// <summary>
        /// Estado de Amazonas
        /// </summary>
        AM, 

        /// <summary>
        /// Estado da Bahia
        /// </summary>
        BA, 

        /// <summary>
        /// Estado do Ceará
        /// </summary>
        CE, 

        /// <summary>
        /// Distrito Federal
        /// </summary>
        DF, 

        /// <summary>
        /// Estado de Espírito Santo
        /// </summary>
        ES, 

        /// <summary>
        /// Estado de Goiás
        /// </summary>
        GO, 

        /// <summary>
        /// Estado do Maranhão
        /// </summary>
        MA, 

        /// <summary>
        /// Estado de Mato Grosso
        /// </summary>
        MT, 

        /// <summary>
        /// Estado de Mato Grosso do Sul
        /// </summary>
        MS, 

        /// <summary>
        /// Estado de Minas Gerais
        /// </summary>
        MG, 

        /// <summary>
        /// Estado do Paraná
        /// </summary>
        PR, 

        /// <summary>
        /// Estado da Paraíba
        /// </summary>
        PB, 

        /// <summary>
        /// Estado do Pará
        /// </summary>
        PA, 

        /// <summary>
        /// Estado de Pernambuco
        /// </summary>
        PE, 

        /// <summary>
        /// Estado do Piauí
        /// </summary>
        PI, 

        /// <summary>
        /// Estado do Rio de Janeiro
        /// </summary>
        RJ, 

        /// <summary>
        /// Estado do Rio Grande do Norte
        /// </summary>
        RN, 

        /// <summary>
        /// Estado do Rio Grande do Sul
        /// </summary>
        RS, 

        /// <summary>
        /// Estado de Rondônia
        /// </summary>
        RO, 

        /// <summary>
        /// Estado de Roraima
        /// </summary>
        RR, 

        /// <summary>
        /// Estado de Santa Catarina
        /// </summary>
        SC, 

        /// <summary>
        /// Estado de Sergipe
        /// </summary>
        SE, 

        /// <summary>
        /// Estado de São Paulo
        /// </summary>
        SP, 

        /// <summary>
        /// Estado de Tocantins
        /// </summary>
        TO
    }
}
