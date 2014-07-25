//
// UserValidator.cs
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

using System;
using FluentValidation;
using System.Linq;
using System.ComponentModel;
using GestUAB.Models;
using Nancy.Scaffolding;
using Nancy.Scaffolding.Validators;
using Nancy.Security;
using System.Collections.Generic;

namespace GestUAB.Models
{

    /// <summary>
    /// User validator
    /// </summary>
    /// 
    public class UserValidator : AbstractValidator<User>
    {
        /// <summary>
        /// Method that validates the creation of or a change in an object.
        /// </summary>
        /// 
        public UserValidator ()
        {
//            using (var session = DocumentSession) {
//                RuleFor (user => user.UserName)
//                    .NotEmpty ().WithMessage ("O nome de usuário é obrigatório.")
//                    .Length (5, 30).WithMessage ("O nome de usuário deve conter entre 5 e 15 caracteres.")
//                    .Matches (@"^[a-zA-Z][a-zA-Z0-9_]*\.?[a-zA-Z0-9_]*$").WithMessage ("Insira somente letras.")
//                    .Must ((user, username) => !session.Query<User> ()
//                        .Where (n => n.UserName == username).Any ()
//                ).WithMessage (@"Já existe um usuário com o nome de usuário ""{0}""", user => user.UserName)
//                     .Remote ("Usuário já existe.", "/validation/user/validate-exists-username", "GET", "*");
//
//                RuleFor (user => user.Email)
//                    .NotEmpty ().WithMessage ("O e-mail é obrigatório.")
//                    .EmailAddress ().WithMessage ("E-mail inválido.")
//                    .Must ((user, email) => !session.Query<User> ()
//                        .Where (n => n.UserName != user.UserName && n.Email == email).Any ()
//                ).WithMessage (@"Já existe um usuário com o e-mail ""{0}""", user => user.Email)
//                    .Remote ("E-mail já cadastrado.", "/validation/user/validate-exists-email", "GET", "*");
//
//                RuleFor (user => user.Password)
//                    .NotEmpty ().WithMessage ("O e-mail é obrigatório.")
//                    .EmailAddress ().WithMessage ("E-mail inválido.")
//                    .Must ((user, email) => !session.Query<User> ()
//                        .Where (n => n.UserName != user.UserName && n.Email == email).Any ()
//                ).WithMessage (@"Já existe um usuário com o e-mail ""{0}""", user => user.Email)
//                    .Remote ("E-mail já cadastrado.", "/validation/user/validate-exists-email", "GET", "*");
//
//            }
//            RuleSet ("Update", () => {
//                RuleFor (user => user.FirstName)
//                    .NotEmpty ().WithMessage ("O campo nome é obrigatório.")
//                    .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
//                        .Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");
//                RuleFor (user => user.LastName)
//                    .NotEmpty ().WithMessage ("O campo sobrenome é obrigatório.")
//                    .Matches (@"^[a-zA-Z\u00C0-\u00ff\s]*$").WithMessage ("Insira somente letras.")
//                    .Length (2, 30).WithMessage ("O nome deve conter entre 2 e 30 caracteres.");
//            }
//            );
        }
    }
}
