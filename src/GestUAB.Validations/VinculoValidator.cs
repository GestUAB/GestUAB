//
// VinculoValidator.cs
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

    /// <summary>
    /// Vinculo validator
    /// </summary>
    public class VinculoValidator : AbstractValidator<Vinculo>
    {
        /// <summary>
        /// Inicia uma nova instância de <see cref="GestUAB.Models.VinculoValidator"/> class.
        /// </summary>
        public VinculoValidator()
        {

            SetRules();
            SetCreateRules();

            RuleSet("Update", () =>
            {
                SetRules();
            });

        }

        /// <summary>
        /// Sets the create rules.
        /// </summary>
        private void SetCreateRules()
        {

        }

        /// <summary>
        /// Sets the rules.
        /// </summary>
        private void SetRules()
        {
            RuleFor(x => x.Banco)
                    .NotEmpty().WithMessage("O conta da instituição é obrigatório.")
                        .Length(0, 100).WithMessage("O conta da instituição deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O conta da instituição deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.Conta)
                    .NotEmpty().WithMessage("O conta é obrigatório.")
                        .Length(0, 100).WithMessage("O conta deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\s]*$").WithMessage("O conta do bolsista deve conter somente caracteres alfabéticos.");

            RuleFor(x => x.DataInicio)
                    .NotEmpty().WithMessage("A data de nascimento do bolsista é obrigatória.")
                        .InclusiveBetween(DateTime.Today.AddYears(-130), DateTime.Today.AddYears(-12))
                        .WithMessage("Não é possível o bolsista ter nascido nesta data.");

            RuleFor(x => x.UltimoCursoTitulacao).NotEmpty().WithMessage("O documento de identificação é obrigatório.")
                    .Length(0, 20).WithMessage(@"O documento deve conter no máximo 20 caracteres alfanúmericos, ' ', '.', '-', '/'.")
                        .Matches(@"^[\d\w\s-\.\/]*$").WithMessage(@"O documento deve conter somente caracteres alfanúmericos, ' ', '.', '-', '/'.");

            RuleFor(x => x.InstituicaoTitulacao)
                    .NotEmpty().WithMessage("O orgão emissor é obrigatório.")
                        .Length(0, 100).WithMessage("O orgão emissor deve conter no máximo 100 caracteres alfabéticos.")
                        .Matches(@"^[a-zA-Z\u00C0-\u00ff\-\/\s]*$").WithMessage("O orgão emissor deve conter somente caracteres alfabéticos.");
        }
    }
}
