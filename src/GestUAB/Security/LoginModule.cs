//
// LoginModule.cs
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


namespace GestUAB.Modules
{
    using Nancy.Authentication.Forms;
    using Nancy.Extensions;
    using Nancy.Security;
    using System;
    using System.Dynamic;

    public class LoginModule : BaseModule
    {
        public LoginModule()
        {
            Get ["/login"] = parameters => {
                // Called when the user visits the login page or is redirected here because
                // and attempt was made to access a restricted resource. It should return
                // the view that contains the login form
                dynamic model = new ExpandoObject();
                model.Errored = this.Request.Query.error.HasValue;

                return View ["login", model];        
            };

            Get ["/logout"] = parameters => {
                // Called when the user clicks the sign out button in the application. Should
                // perform one of the Logout actions (see below)
                return this.LogoutAndRedirect("~/");
            };

            Post ["/login"] = parameters => {
                // Called when the user submits the contents of the login form. Should
                // validate the user based on the posted form data, and perform one of the
                // Login actions (see below)
                var userGuid = Membership.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);            
            };
        }
    }
}

