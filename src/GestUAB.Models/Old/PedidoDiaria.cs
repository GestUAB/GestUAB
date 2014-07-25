//
// PedidoDiaria.cs
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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using FluentValidation;
    using GestUAB.Models;
    using Nancy.Scaffolding;
    using Raven.Client;
    using Raven.Client.Linq;

    /// <summary>
    /// Pedido diaria.
    /// </summary>
    public class PedidoDiaria : IModel
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="GestUAB.Models.PedidoDiaria"/> class.
        /// </summary>
        public PedidoDiaria()
        {
        }
        #region IModel implementation
        /// <summary>
        /// Obtém ou define o Id.
        /// </summary>
        /// <value>The identifier.</value>
        [Display(Name = "Código",
                 Description = "Código do memorando.")]
        [ScaffoldVisibility(all: Visibility.Hidden)] 
        public System.Guid Id { get; set; }
        #endregion
        /// <summary>
        /// Obtém ou define a observação.
        /// </summary>
        /// <value>The observacao.</value>
        [Display(Name = "Referente",
                 Description = "Texto \"Referente\" a.")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public string Observacao { get; set; }

        /// <summary>
        /// Obtém ou define o destino.
        /// </summary>
        /// <value>The destino.</value>
        [Display(Name = "Destino",
                 Description = "Local de destino.")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public string Destino { get; set; }

        /// <summary>
        /// Obtém ou define a data de saída.
        /// </summary>
        /// <value>The saida.</value>
        [Display(Name = "Data de saída",
                 Description = "Data de saída.")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public DateTime Saida { get; set; }

        /// <summary>
        /// Obtém ou define a data de retorno.
        /// </summary>
        /// <value>The retorno.</value>
        [Display(Name = "Data de retorno.",
                 Description = "Data de retorno.")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public DateTime Retorno { get; set; }

        /// <summary>
        /// Obtém ou define o solicitatne.
        /// </summary>
        /// <value>The solicitatne.</value>
        [Display(Name = "Solicitante",
                 Description = "Nome do solicitante.")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public Departamento Solicitatne { get; set; }

        /// <summary>
        /// Obtém ou define o convênio.
        /// </summary>
        /// <value>The convenio.</value>
        [Display(Name = "Convênio",
                 Description = "Numero do cônvenio.")]
        [ScaffoldVisibility(all: Visibility.Show)] 
        public string Convenio { get; set; }
    }
}
