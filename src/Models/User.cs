//
// User.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2012 
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
using System;
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GestUAB.Validators;
using GestUAB.Models;

namespace GestUAB.Models
{
    public class User : IModel
    {
        #region Builder
        public User ()
        {

        }

        public static User DefaultUser()
        {
            return new User() {
                Id = Guid.NewGuid(),
                Username = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty
            };
        }
        #endregion

        #region IModel implementation
        public System.Guid Id { get ; set ; }
        #endregion

        [Display(Name = "Nome do usuário",
                 Description= "Nome do usuário. Ex.: jsilva.")]
        [ScaffoldVisibility(create:ScaffoldVisibilityType.Show,
                            read:ScaffoldVisibilityType.Hidden,
                            update:ScaffoldVisibilityType.Hidden,
                            delete:ScaffoldVisibilityType.Hidden)] 
        public string Username { get; set; }

        [Display(Name = "Nome completo",
                 Description= "Nome completo do usuário. Ex.: João da Silva.")]
        [ScaffoldVisibility(read:ScaffoldVisibilityType.Show)] 
        public string Name { get { return FirstName + " " + LastName; } }

        [Display(Name = "Nome",
                 Description= "Nome do usuário. Ex.: João.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome",
                 Description= "Sobrenome do usuário. Ex.: da Silva.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string LastName { get; set; }

        [Display(Name = "E-mail",
                 Description= "E-mail do usuário. Ex.: joao@gestuab.com.br.")]
        [ScaffoldVisibility(all:ScaffoldVisibilityType.Show)] 
        public string Email { get; set; }
    }

    public class UserValidator : ValidatorBase<User>
    {
        public UserValidator ()
        {
            using (var session = DocumentSession) {
                RuleFor (user => user.Username)
                    .NotEmpty ().WithMessage ("O nome de usuário é obrigatório.")
                    .Length (5, 30).WithMessage ("O nome de usuário deve conter entre 5 e 15 caracteres.")
                    .Matches (@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.")
                    .Must ((user, username) => !session.Query<User> ()
                        .Where (n => n.Username == username).Any ()
                ).WithMessage (@"Já existe um usuário com o nome de usuário ""{0}""", user => user.Username)
                     .Remote ("Usuário já existe.", "/validation/user/validate-exists-username", "GET", "*");
                RuleFor (user => user.Email)
                    .NotEmpty ().WithMessage ("O e-mail é obrigatório.")
                    .EmailAddress ().WithMessage ("E-mail inválido.")
                    .Must ((user, email) => !session.Query<User> ()
                        .Where (n => n.Username != user.Username && n.Email == email).Any ()
                ).WithMessage (@"Já existe um usuário com o e-mail ""{0}""", user => user.Email)
                    .Remote ("E-mail já cadastrado.", "/validation/user/validate-exists-email", "GET", "*");
            }
            RuleSet ("Update", () => {
                RuleFor (user => user.FirstName)
                    .NotEmpty ().WithMessage ("O campo nome é obrigatório.")
                    .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
                        .Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");
                RuleFor (user => user.LastName)
                    .NotEmpty ().WithMessage ("O campo sobrenome é obrigatório.")
                    .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
                    .Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");
            }
            );
        }
    }
}