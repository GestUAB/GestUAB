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

namespace GestUAB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Nancy.Scaffolding;
    using Nancy.Security;
    using GestUAB.Models;

    /// <summary>
    /// User class.
    /// </summary>
    /// 
    public class User : IModel, IUserIdentity
    {
        #region Builder
        /// <summary>
        /// User class builder.
        /// </summary>
        public User ()
        {
            Id = Guid.Empty;
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Claims = new List<string>();
        }
        #endregion

        #region IModel implementation
        [Display(Name = "Código",
                 Description= "Código do usuário.")]
        [Visibility(all:Visibility.Hidden)] 
        public System.Guid Id { get ; set ; }
        #endregion

        #region IUserIdentity implementation
        [Display(Name = "Nome do usuário",
                 Description= "Nome do usuário. Ex.: jsilva.")]
        [Visibility(create:Visibility.Show,
                            read:Visibility.Hidden,
                            update:Visibility.Hidden,
                            delete:Visibility.Hidden)] 
        public string UserName { get; set; }

        [Display(Name = "Grupos",
                 Description= "Grupos quais o usuário participa.")]
        [Visibility(read:Visibility.Show)] 
        public IEnumerable<string> Claims { get ; set ; }
        #endregion

        #region Variables
       

        [Display(Name = "Nome completo",
                 Description= "Nome completo do usuário. Ex.: João da Silva.")]
        [Visibility(read:Visibility.Show)] 
        public string Name { get { return FirstName + " " + LastName; } }

        [Display(Name = "Nome",
                 Description= "Nome do usuário. Ex.: João.")]
        [Visibility(all:Visibility.Show)] 
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome",
                 Description= "Sobrenome do usuário. Ex.: da Silva.")]
        [Visibility(all:Visibility.Show)] 
        public string LastName { get; set; }

        [Display(Name = "Senha",
                 Description= "Senha do usuário. Ex.: Adsfd!13.")]
        [Visibility(create:Visibility.Show)] 
        [InputType(InputType.Password)]
        public string Password { get; set; }

        [Display(Name = "E-mail",
                 Description= "E-mail do usuário. Ex.: joao@gestuab.com.br.")]
        [Visibility(all:Visibility.Show)] 
        public string Email { get; set; }
        #endregion
    }

}
