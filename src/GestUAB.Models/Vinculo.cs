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
using Nancy.TinyIoc;


namespace GestUAB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Nancy.Scaffolding;
    using Nancy.Scaffolding.Validators;

    /// <summary>
    /// Um Vinculo.
    /// </summary>
    public class Vinculo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestUAB.Models.Vinculo"/> class.
        /// </summary>
        public Vinculo()
        {
            this.Agencia = string.Empty;
            this.DataFim = DateTime.MinValue;
            this.DataInicio = DateTime.MinValue;
            this.UltimoCursoTitulacao = string.Empty;
            this.Banco = string.Empty;
            this.Conta = string.Empty;
            this.Observacoes = string.Empty;
            this.InstituicaoTitulacao = string.Empty;
            this.AreaUltimoCursoSuperiorConcluido = GrandeAreaConhecimentoType.CienciasAgrarias;
        }

        //[ScaffoldObjectList (typeof(ColaboradorManager),    
        //             typeof(Colaborador), "GetList", "Id", SelectType.Single)]
        public Colaborador Colaborador { get; set; }

        //[ScaffoldObjectList (typeof(ColaboradorManager),    
        //             typeof(Colaborador), "GetList", "Id", SelectType.Single)]
        public Setor Setor { get; set; }

        /// <summary>
        /// Gets or sets the data nascimento.
        /// </summary>
        /// <value>The data nascimento.</value>
        [Display(Name = "Data de início do vínculo",
                 Description = "Data de início do vínculo.")]
        [Visibility(all: Visibility.Show)]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Gets or sets the data nascimento.
        /// </summary>
        /// <value>The data nascimento.</value>
        [Display(Name = "Data de fim do vínculo",
                 Description = "Data de fim do vínculo.")]
        [Visibility(all: Visibility.Show)]
        public DateTime DataFim { get; set; }

        public FuncaoType Funcao { get; set; }

        //[ScaffoldObjectList (typeof(ColaboradorManager),    
        //             typeof(Colaborador), "GetList", "Id", SelectType.Single)]
        public Polo Polo { get; set; }

        public int BolsasPrevistas { get; set; }

        public int BolsasPagas { get; set; }

        [Display(Name = "Banco",
                 Description= "Código do banco para depósito.")]
        [Visibility(all: Visibility.Show)]
        public string  Banco { get ; set ; }

        [Display(Name = "Agência",
                 Description= "Agência para depósito.")]
        [Visibility(all: Visibility.Show)]
        public string Agencia { get ; set ; }

        [Display(Name = "Conta bancária",
                 Description= "Conta bancária para depósito.")]
        [Visibility(all: Visibility.Show)]
        public string Conta { get ; set ; }

        /// <summary>
        /// Gets or sets the profissao.
        /// </summary>
        /// <value>The profissao.</value>
        [Display(Name = "Área do último curso superior concluído",
                 Description = "Área do último curso superior concluído do bolsista. Ex.: Professor.")]
        [Visibility(all: Visibility.Show)]
        public GrandeAreaConhecimentoType AreaUltimoCursoSuperiorConcluido { get; set; }

        /// <summary>
        /// Gets or sets the documento.
        /// </summary>
        /// <value>The documento.</value>
        [Display(Name = "UltimoCursoTitulacao",
                 Description = "UltimoCursoTitulacao de identificação do bolsista.")]
        [Visibility(all: Visibility.Show)]
        public string UltimoCursoTitulacao { get; set; }

        /// <summary>
        /// Gets or sets the orgao emissor.
        /// </summary>
        /// <value>The orgao emissor.</value>
        [Display(Name = "Instituição de titulação",
                 Description = "Instituição de titulação do documento. SSP-PR")]
        [Visibility(all: Visibility.Show)]
        public string InstituicaoTitulacao { get; set; }

        /// <summary>
        /// Gets or sets the observacoes.
        /// </summary>
        /// <value>The observacoes.</value>
        [Display(Name = "Observações",
                 Description = "Observação sobre o colaborador.")]
        [Visibility(all: Visibility.Show)]
        public string Observacoes { get; set; }

    }

}
