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
    public class CursoModule : BaseModule
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="GestUAB.Modules.CursoModule"/> class.
        /// </summary>
        public CursoModule() : base("/cursos")
        {
            this.Get["/"] = _ => 
            {
                return View["index", CursoManager.GetAll()];
            };

            this.Get["/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                var m = CursoManager.Get(id);
                if (m == null)
                {
                    return new NotFoundResponse();
                }

                return View["show", m];
            };

            this.Get["/new"] = x => 
            {
                return View["new", new Curso()];
            };

            this.Post["/new"] = x => 
            {
                var curso = this.Bind<Curso>();
                var result = new CursoValidator().Validate(curso);
                if (!result.IsValid)
                {
                    return Response.AsJson(result.Errors)
                        .WithStatusCode(HttpStatusCode.BadRequest)
                            .WithHeader("X-Status-Reason", "A validação falhou.".ToHtmlEncode());
                }
                curso.Setor = SetorManager.Get(Guid.Parse(Request.Form["Setor"]));
                CursoManager.Insert(curso);
                return Response.AsJson("Redirecionando para a página do curso", HttpStatusCode.Created)
                .WithHeader("Location", string.Format("/cursos/{0}", curso.Id));
            };

            this.Get["/edit/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                var curso = CursoManager.Get(id);
                if (curso == null)
                {
                    return new NotFoundResponse();
                }

                return View["edit", curso];
            };

            this.Put["/edit/{Id}"] = x => 
            {
                var curso = this.Bind<Curso>();
                var result = new CursoValidator().Validate(curso, ruleSet: "Update");
                if (!result.IsValid)
                {
                    return Response.AsJson(result.Errors, HttpStatusCode.BadRequest)
                        .WithHeader("X-Status-Reason", "A validação falhou.".ToHtmlEncode());
                }

                if (CursoManager.Update(curso))
                {
                    return Response.AsJson("Redirecionando para a página do curso", HttpStatusCode.Accepted)
                        .WithHeader("Location", string.Format("/cursos/{0}", curso.Id));
                }

                return Response.AsJson("Ocorreu um erro ao atualizar o curso.") 
                .WithHeader("X-Status-Reason", "Ocorreu um erro ao atualizar o curso.");
            };

            this.Delete["/delete/{Id}"] = x => 
            {
                Guid id = Guid.Parse(x.Id);
                if (CursoManager.Delete(id))
                {
                    return Response.AsJson("Redirecionando para a página a lista de cursos.\t", HttpStatusCode.NoContent)
                        .WithHeader("Location", string.Format("/cursos"));
                }

                return Response.AsJson("Ocorreu um erro ao excluir o curso.") 
                .WithHeader("X-Status-Reason", "Ocorreu um erro ao excluir o curso.");
            };
        }
    }
}
