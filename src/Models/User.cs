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
using FluentValidation;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;

namespace GestUAB.Models
{
    public class User : IModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Name { get { return FirstName + " " + LastName; } }
    }

    public class UserValidator : ValidatorBase<User>
    {
        public UserValidator ()
        {
            using (var session = DocumentSession) {
                RuleFor (user => user.Username).NotEmpty ()
                    .Length (5, 15)
                    .Matches ("[a-z]*")
                    .Must ((user, username) => !session.Query<User> ()
                        .Where (n => n.Username == username).Any ()
                )
                   .WithMessage ("Already exists a user with the username '{0}'", user => user.Username);
                RuleFor (user => user.Email)
                    .NotEmpty ()
                    .EmailAddress ()
                    .Must ((user, email) => !session.Query<User> ()
                        .Where (n => n.Email == email).Any ()
                )
                    .WithMessage ("Already exists a user with the email '{0}'", user => user.Email);
            }
            RuleSet ("Update", () => {
                RuleFor (user => user.FirstName).NotEmpty ();
                RuleFor (user => user.LastName).NotEmpty ();
            }
            );
        }
    }
}