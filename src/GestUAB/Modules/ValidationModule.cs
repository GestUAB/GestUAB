//
// ValidationModule.cs
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
using Nancy;
using GestUAB.Models;
using FluentValidation;

namespace GestUAB.Modules
{
    public class ValidationModule : BaseModule
    {

        public ValidationModule ()
        {
            Get ["/validation/user/validate-exists-username"] = x => { 
                var user = new User();
                var q = DeserializeQueryString(Request.Query["*"]);
                user.UserName = q.Username;
                user.FirstName =  q.FirstName;
                user.LastName =  q.LastName;
                user.Email =  q.Email;
                var result = new UserValidator().Validate(user, y => y.Email);
                if (result.IsValid) {
                    return Response.AsJson<bool>(true);
                }
                return Response.AsJson<bool>(false);
            };
            Get ["/validation/user/validate-exists-email"] = x => { 
                var user = new User();
                var q = DeserializeQueryString(Request.Query["*"]);
                user.UserName = q.Username;
                user.FirstName =  q.FirstName;
                user.LastName =  q.LastName;
                user.Email =  q.Email;
                var result = new UserValidator().Validate(user, y => y.Email);
                if (result.IsValid) {
                    return Response.AsJson<bool>(true);
                }
                return Response.AsJson<bool>(false);
            };

            Get ["/validation/colaborador/validate-exists-cpf"] = x => { 
                return Response.AsJson<bool>(true);
            };
        }

        dynamic DeserializeQueryString(string queryString)
        {
            queryString = Nancy.Helpers.HttpUtility.UrlDecode(queryString);
            dynamic obj = new DynamicDictionary();
            var pairs = queryString.Split('&');
            foreach (var pair in pairs) {
                var kv = pair.Split('=');
                obj[kv[0]] = kv[1];
            }
            return obj;
        }
    }
}

