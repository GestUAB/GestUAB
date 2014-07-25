//
// CourseModule.cs
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
using Nancy.ModelBinding;

namespace GestUAB.Modules
{
    using System;
    using FluentValidation;
    using GestUAB.DataAccess;
    using GestUAB.Models;
    using Nancy;
    using Nancy.TinyIoc;

    /// <summary>
    /// Course Module.
    /// </summary>
    public class SetorModule : BaseModule
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="GestUAB.Modules.SetorModule"/> class.
        /// </summary>
        public SetorModule() : base("/setores")
        {
            this.Get["/"] = _ => 
            {
                return View["index", SetorManager.GetAll()];
            };

            this.Get["/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                var m = SetorManager.Get(id);
                if (m == null)
                {
                    return new NotFoundResponse();
                }

                return View["show", m];
            };

            this.Get["/new"] = x => 
            {
                return View["new", new Setor()];
            };

            this.Post["/new"] = x => 
            {
                var setor = this.Bind<Setor>();
                var result = new SetorValidator().Validate(setor);
                if (!result.IsValid)
                {
                    return Response.AsJson(result.Errors)
                        .WithStatusCode(HttpStatusCode.BadRequest)
                            .WithHeader("X-Status-Reason", "A validação falhou.".ToHtmlEncode());
                }

                SetorManager.Insert(setor);
                return Response.AsJson("Redirecionando para a página do setor", HttpStatusCode.Created)
                .WithHeader("Location", string.Format("/setores/{0}", setor.Id));
            };

            this.Get["/edit/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                var setor = SetorManager.Get(id);
                if (setor == null)
                {
                    return new NotFoundResponse();
                }

                return View["edit", setor];
            };

            this.Put["/edit/{Id}"] = x => 
            {
                var setor = this.Bind<Setor>();
                var result = new SetorValidator().Validate(setor, ruleSet: "Update");
                if (!result.IsValid)
                {
                    return Response.AsJson(result.Errors, HttpStatusCode.BadRequest)
                        .WithHeader("X-Status-Reason", "A validação falhou.".ToHtmlEncode());
                }

                if (SetorManager.Update(setor))
                {
                    return Response.AsJson("Redirecionando para a página do setor", HttpStatusCode.Accepted)
                        .WithHeader("Location", string.Format("/setores/{0}", setor.Id));
                }

                return Response.AsJson("Ocorreu um erro ao atualizar o setor.") 
                .WithHeader("X-Status-Reason", "Ocorreu um erro ao atualizar o setor.");
            };

            this.Delete["/delete/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                if (SetorManager.Delete(id))
                {
                    if (Request.Query.ajax) {
                        return Response.AsJson("Redirecionando para a página a lista de setores.", HttpStatusCode.NoContent);
                    }
                    return Response.AsJson("Redirecionando para a página a lista de setores.", HttpStatusCode.NoContent)
                        .WithHeader("Location", string.Format("/setores"));
                }

                return Response.AsJson("Ocorreu um erro ao excluir o setor.") 
                .WithHeader("X-Status-Reason", "Ocorreu um erro ao excluir o setor.");
            };
        }
    }
}
