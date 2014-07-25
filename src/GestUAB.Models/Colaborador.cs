//
// Colaborador.cs
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

    /// <summary>
    /// Um Colaborador.
    /// </summary>
    public class Colaborador : IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestUAB.Models.Colaborador"/> class.
        /// </summary>
        public Colaborador()
        {
            this.Id = Guid.NewGuid();
            this.Bairro = string.Empty;
            this.Celular = string.Empty;
            this.Cep = string.Empty;
            this.Complemento = string.Empty;
            this.Cpf = string.Empty;
            this.DataEmissao = DateTime.MinValue;
            this.DataNascimento = DateTime.MinValue;
            this.Documento = string.Empty;
            this.Email = string.Empty;
            this.EstadoCivil = EstadoCivilType.Solteiro;
            this.Id = Guid.NewGuid();
            this.Instituicao = string.Empty;
            this.Logradouro = string.Empty;
            this.TipoLogradouro =  TipoLogradouro.Rua;
            this.Municipio = string.Empty;
            this.MunicipioNascimento = string.Empty;
            this.Nome = string.Empty;
            this.NomeConjuge = string.Empty;
            this.NomeMae = string.Empty;
            this.NomePai = string.Empty;
            this.Numero = string.Empty;
            this.Observacoes = string.Empty;
            this.OrgaoEmissor = string.Empty;
            this.Profissao = string.Empty;
            this.Sexo = SexoType.Feminino;
            this.Telefone = string.Empty;
            this.TipoDocumento = DocumentoType.CarteiraIdentidade;
            this.Uf = UfType.AC;
            this.UfNascimento = UfType.AC;
        }

        /// <summary>
        /// Obtém ou define o Id.
        /// </summary>
        /// <value>The identifier.</value>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the instituicao.
        /// </summary>
        /// <value>The instituicao.</value>
        public string Instituicao { get; set; }

        /// <summary>
        /// Gets or sets the cpf.
        /// </summary>
        /// <value>The cpf.</value>
        public string Cpf
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the nome.
        /// </summary>
        /// <value>The nome.</value>
        public string Nome { get; set; }

        /// <summary>
        /// Gets or sets the profissao.
        /// </summary>
        /// <value>The profissao.</value>
        public string Profissao { get; set; }

        /// <summary>
        /// Gets or sets the sexo.
        /// </summary>
        /// <value>The sexo.</value>
        public SexoType Sexo { get; set; }

        /// <summary>
        /// Gets or sets the data nascimento.
        /// </summary>
        /// <value>The data nascimento.</value>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Gets or sets the tipo documento.
        /// </summary>
        /// <value>The tipo documento.</value>
        public DocumentoType TipoDocumento { get; set; }

        /// <summary>
        /// Gets or sets the documento.
        /// </summary>
        /// <value>The documento.</value>
        public string Documento { get; set; }

        /// <summary>
        /// Gets or sets the orgao emissor.
        /// </summary>
        /// <value>The orgao emissor.</value>
        public string OrgaoEmissor { get; set; }

        /// <summary>
        /// Gets or sets the data emissao.
        /// </summary>
        /// <value>The data emissao.</value>
        public DateTime DataEmissao { get; set; }

        /// <summary>
        /// Gets or sets the uf nascimento.
        /// </summary>
        /// <value>The uf nascimento.</value>
        public UfType UfNascimento { get; set; }

        /// <summary>
        /// Gets or sets the municipio nascimento.
        /// </summary>
        /// <value>The municipio nascimento.</value>
        public string MunicipioNascimento { get; set; }

        /// <summary>
        /// Gets or sets the estado civil.
        /// </summary>
        /// <value>The estado civil.</value>
        public EstadoCivilType EstadoCivil { get; set; }

        /// <summary>
        /// Gets or sets the nome conjuge.
        /// </summary>
        /// <value>The nome conjuge.</value>
        public string NomeConjuge { get; set; }

        /// <summary>
        /// Gets or sets the nome pai.
        /// </summary>
        /// <value>The nome pai.</value>
        public string NomePai { get; set; }

        /// <summary>
        /// Gets or sets the nome mae.
        /// </summary>
        /// <value>The nome mae.</value>
        public string NomeMae { get; set; }

        /// <summary>
        /// Gets or sets the tipo logradouro.
        /// </summary>
        /// <value>The tipo logradouro.</value>
        public TipoLogradouro TipoLogradouro { get; set; }

        /// <summary>
        /// Gets or sets the logradouro.
        /// </summary>
        /// <value>The logradouro.</value>
        public string Logradouro { get; set; }

        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        /// <value>The numero.</value>
        public string Numero { get; set; }

        /// <summary>
        /// Gets or sets the complemento.
        /// </summary>
        /// <value>The complemento.</value>
        public string Complemento { get; set; }

        /// <summary>
        /// Gets or sets the cep.
        /// </summary>
        /// <value>The cep.</value>
        public string Cep { get; set; }

        /// <summary>
        /// Gets or sets the bairro.
        /// </summary>
        /// <value>The bairro.</value>
        public string Bairro { get; set; }

        /// <summary>
        /// Gets or sets the uf.
        /// </summary>
        /// <value>The uf.</value>
        public UfType Uf { get; set; }

        /// <summary>
        /// Gets or sets the municipio.
        /// </summary>
        /// <value>The municipio.</value>
        public string Municipio { get; set; }

        /// <summary>
        /// Gets or sets the telefone.
        /// </summary>
        /// <value>The telefone.</value>
        public string Telefone { get; set; }

        /// <summary>
        /// Gets or sets the celular.
        /// </summary>
        /// <value>The celular.</value>
        public string Celular { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the observacoes.
        /// </summary>
        /// <value>The observacoes.</value>
        public string Observacoes { get; set; }

    }

}
