//
// Vinculo.cs
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
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using Nancy.Scaffolding;
	using Nancy.Scaffolding.Validators;

	public class Polo : IModel
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="GestUAB.Models.Curso"/> class.
        /// </summary>
        public Polo()
        {
            this.Id = Guid.NewGuid();
            this.Nome = string.Empty;
        }

        /// <summary>
        /// Obtém ou define o Id.
        /// </summary>
        /// <value>The identifier.</value>
        [Display(Name = "Código",
                 Description = "Código do polo.")]
        [Visibility(all: Visibility.Hidden)]
        public System.Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome.
        /// </summary>
        /// <value>The nome.</value>
        [Display(Name = "Nome",
                 Description = "Nome do curso.")]
        [Visibility(all: Visibility.Show)]
        public string Nome { get; set; }

        //TODO Inserir endereço completo

//        /// <summary>
//        /// Gets or sets the uf nascimento.
//        /// </summary>
//        /// <value>The uf nascimento.</value>
//        [Display(Name = "UF",
//                 Description = "UF.")]
//        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
//        [ScaffoldSelect] 
//        public UfType Estado { get; set; }
//
//        /// <summary>
//        /// Gets or sets the municipio nascimento.
//        /// </summary>
//        /// <value>The municipio nascimento.</value>
//        [Display(Name = "Município",
//                 Description = "Município.")]
//        [ScaffoldVisibility(all: ScaffoldVisibilityType.Show)]
//        public string Municipio { get; set; }

	}
}
