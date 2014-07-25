//
// ColaboradorValidator.cs
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
    using FluentValidation;
    using Nancy.Scaffolding.Validators;

    /// <summary>
    /// Colaborador validator
    /// </summary>
    public class ColaboradorValidator : AbstractValidator<Colaborador>
    {
        /// <summary>
        /// Inicia uma nova instância de <see cref="GestUAB.Models.ColaboradorValidator"/> class.
        /// </summary>
        public ColaboradorValidator()
        {

            SetRules();
            SetCreateRules();

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Cpf)
                    .NotEmpty()
                    .WithMessage("O CPF é obrigatório.")
                    .Length(14)
                    .WithMessage(@"CPF inválido.")
                    .Matches(@"^(\d{3}\.\d{3}\.\d{3}-\d{2})$")
                    .WithMessage("O CPF deve seguir o padrão xxx.xxx.xxx-xx.")
                    .ValidCpf()
                    .WithMessage(@"CPF inválido.");
                SetRules();
            });

        }

        /// <summary>
        /// Sets the create rules.
        /// </summary>
        private void SetCreateRules()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Cpf)
                .NotEmpty()
                    .WithMessage("O CPF é obrigatório.")
                    .Length(14)
                    .WithMessage(@"CPF inválido.")
                    .Matches(@"^(\d{3}\.\d{3}\.\d{3}-\d{2})$")
                    .WithMessage("O CPF deve seguir o padrão xxx.xxx.xxx-xx.")
                    .ValidCpf()
                    .WithMessage(@"CPF inválido.")
                    .Must(x => !ColaboradorManager.CpfExists(x))
                    .WithMessage(@"CPF já cadastrado", x => x.Cpf)
                    .Remote(@"CPF já cadastrado", "/validation/colaborador/validate-exists-cpf", "GET");

        }

        /// <summary>
        /// Sets the rules.
        /// </summary>
        private void SetRules()
        {
            RuleFor(x => x.Instituicao)
                    .NotEmpty().WithMessage("O nome da instituição é obrigatório.")
                        .Length(0, 100).WithMessage("O nome da instituição deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O nome da instituição deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.Nome)
                    .NotEmpty().WithMessage("O nome é obrigatório.")
                        .Length(0, 100).WithMessage("O nome deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O nome do colaborador deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.Profissao)
                    .Length(0, 100).WithMessage("A profissão deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("A profissão deve conter somente caracteres alfabéticos."); //\\p{L}

            RuleFor(x => x.DataNascimento)
                    .NotEmpty().WithMessage("A data de nascimento do colaborador é obrigatória.")
                        .InclusiveBetween(DateTime.Today.AddYears(-130), DateTime.Today.AddYears(-12))
                        .WithMessage("Não é possível o colaborador ter nascido nesta data.");

            RuleFor(x => x.Documento).NotEmpty().WithMessage("O documento de identificação é obrigatório.")
                    .Length(0, 20).WithMessage(@"O documento deve conter no máximo 20 caracteres alfanúmericos, ' ', '.', '-', '/'.")
                        .Matches(@"^[\d\w\s-\.\/]*$").WithMessage(@"O documento deve conter somente caracteres alfanúmericos, ' ', '.', '-', '/'.");

            RuleFor(x => x.OrgaoEmissor)
                    .NotEmpty().WithMessage("O orgão emissor é obrigatório.")
                        .Length(0, 100).WithMessage("O orgão emissor deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\/\s]*$").WithMessage("O orgão emissor deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.MunicipioNascimento).NotEmpty().WithMessage("O município de nascimento é obrigatório.")
                    .Length(0, 100).WithMessage("O município de nascimento deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O município de nascimento deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.NomeConjuge)
                    .ValidNomeConjuge()
                        .Length(0, 100).WithMessage("O nome do cônjuge deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O nome do colaborador deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.NomePai)
                    .Length(0, 100).WithMessage("O nome do pai deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O nome do pai deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.NomeMae)
                    .NotEmpty().WithMessage("O nome da mãe é obrigatório.")
                        .Length(0, 100).WithMessage("O nome da mãe deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\d\-\s]*$").WithMessage("O nome do colaborador deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.Logradouro)
                    .NotEmpty().WithMessage("O logradouro de endereço é obrigatório.")
                        .Length(0, 100).WithMessage("O logradouro de endereço deve conter no máximo 100 caracteres alfanuméricos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\d\-\s\.]*$").WithMessage("O nome do colaborador deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.Complemento)
                    .Length(0, 100).WithMessage("O complemento de endereço deve conter no máximo 100 caracteres alfanuméricos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\d\-\s\.]*$").WithMessage("O complemento de endereço deve conter somente caracteres alfanuméricos.");

            RuleFor(x => x.Numero)
                    .NotEmpty().WithMessage("O número de endereço é obrigatório.")
                        .Length(0, 6).WithMessage("O número de endereço deve conter no máximo 6 caracteres numéricos.")
                        .Matches(@"^(\d*)$").WithMessage("O número de endereço deve conter somente caracteres numéricos.");

            RuleFor(x => x.Cep).NotEmpty().WithMessage("O CEP é obrigatório.")
                    .Length(9)
                        .WithMessage("O CEP deve conter 8 caracteres numéricos.")
                        .Matches(@"^(\d{5}-\d{3})$") // .Matches ("^(\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2})|(\\d{11})$")
                        .WithMessage("O CEP deve seguir o padrão xxxxx-xxx.");

            RuleFor(x => x.Bairro).NotEmpty().WithMessage("O bairro é obrigatório.")
                    .Length(0, 100).WithMessage("O bairro deve conter no máximo 100 caracteres alfanuméricos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\d\-\s]*$").WithMessage("O bairro deve conter somente caracteres alfanuméricos.");

            RuleFor(x => x.Municipio).NotEmpty().WithMessage("O município é obrigatório.")
                    .Length(0, 100).WithMessage("O município deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O município deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.Telefone).NotEmpty()
                    .WithMessage("O telefone é obrigatório.")
                        .Length(14)
                        .WithMessage("O telefone deve conter 10 caracteres numéricos.")
                        .Matches(@"^(\([0-9]{2}\)\s[0-9]{4}-[0-9]{4})$")
                        .WithMessage("O telefone deve seguir o padrão (xx) xxxx-xxxx");

            RuleFor(x => x.Celular)
                    .Length(0, 14)
                        .WithMessage("O celular deve conter 10 caracteres numéricos.")
                        .Matches(@"^(\([0-9]{2}\)\s[0-9]{4}-[0-9]{4})|()$")
                        .WithMessage("O celular deve seguir o padrão (xx) xxxx-xxxx");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .Length(0, 50)
                .WithMessage("O e-mail deve conter no máximo 50 caracteres.")
                .EmailAddress()
                .WithMessage("E-mail inválido");
        }
    }
}
