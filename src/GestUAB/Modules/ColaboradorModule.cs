//
// ColaboradorModule.cs
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
namespace GestUAB.Modules
{
    using System;
    using System.Linq;
    using FluentValidation;
    using GestUAB.DataAccess;
    using GestUAB.Models;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Responses;
    using Nancy.TinyIoc;
    using Nancy.Validation;
    using Raven.Client.Linq;

    /// <summary>
    /// colaboradores Module
    /// </summary>
    public class ColaboradorModule : BaseModule
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="GestUAB.Modules.ColaboradorModule"/> class.
        /// </summary>
        public ColaboradorModule() : base("/colaboradores")
        {
            this.Get["/"] = _ => 
            {
                var all = ColaboradorManager.GetAll();
                return View["index", all];
            };

            this.Get["/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                var m = ColaboradorManager.Get(id);
                if (m == null)
                {
                    return new NotFoundResponse();
                }

                return View["show", m];
            };

            this.Get["/new"] = x => 
            {
                return View["new", new Colaborador()];
            };

            this.Post["/new"] = x => 
            {
                var colaborador = this.Bind<Colaborador>();
                var result = new ColaboradorValidator().Validate(colaborador);
                if (!result.IsValid)
                {
                    return Response.AsJson(result.Errors)
                        .WithStatusCode(HttpStatusCode.BadRequest)
                            .WithHeader("X-Status-Reason", "A validação falhou.".ToHtmlEncode());
                }

                ColaboradorManager.Insert(colaborador);
                return Response.AsJson("Redirecionando para a página do colaborador", HttpStatusCode.Created)
                    .WithHeader("Location", string.Format("/colaboradores/{0}", colaborador.Id));
            };

            this.Get["/edit/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                var colaborador = ColaboradorManager.Get(id);
                if (colaborador == null)
                {
                    return new NotFoundResponse();
                }

                return View["edit", colaborador];
            };

            this.Put["/edit/{Id}"] = x => 
            {
                var colaborador = this.Bind<Colaborador>();
                var result = new ColaboradorValidator().Validate(colaborador, ruleSet: "Update");
                if (!result.IsValid)
                {
                    return Response.AsJson(result.Errors, HttpStatusCode.BadRequest)
                        .WithHeader("X-Status-Reason", "A validação falhou.".ToHtmlEncode());
                }

                if (ColaboradorManager.Update(colaborador))
                {
                    return Response.AsJson("Redirecionando para a página do colaborador", HttpStatusCode.Accepted)
                        .WithHeader("Location", string.Format("/colaboradores/{0}", colaborador.Id));
                }

                return Response.AsJson("Ocorreu um erro ao atualizar o colaborador.") 
                    .WithHeader("X-Status-Reason", "Ocorreu um erro ao atualizar o colaborador.");
            };

            this.Delete["/delete/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);

                if (ColaboradorManager.Delete(id))
                {
                    return Response.AsJson("Redirecionando para a página a lista de colaboradores.\t", HttpStatusCode.NoContent)
                        .WithHeader("Location", string.Format("/colaboradores"));
                }

                return Response.AsJson("Ocorreu um erro ao excluir o colaborador.") 
                    .WithHeader("X-Status-Reason", "Ocorreu um erro ao excluir o colaborador.");
            };
        }
    }
}