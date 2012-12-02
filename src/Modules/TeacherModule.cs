using System;
using Nancy;
using GestUAB.Models;
using Raven.Client.Linq;
using System.Linq;
using Nancy.ModelBinding;
using Nancy.Validation;
using FluentValidation;
using Nancy.Responses;

namespace GestUAB.Modules
{
    public class TeacherModule : BaseModule
    {
        public TeacherModule () : base("/teachers")
        {
            Get["/"] = _ =>
            {
                return View["index", DocumentSession.Query<Teacher>()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList()];
            };


            Get["/{Id}"] = x =>
            {
                Guid id = Guid.Parse(x.Id);
                var professor = DocumentSession.Query<Teacher>("TeachersById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.Id == id).FirstOrDefault();
                if (professor == null)
                    return new NotFoundResponse();
                return View["show", professor];
            };

            Get["/edit/{Id}"] = x =>
            {
                Guid id = Guid.Parse(x.Id);
                var professor = DocumentSession.Query<Teacher>("TeachersById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.Id == id).FirstOrDefault();
                if (professor == null)
                    return new NotFoundResponse();
                return View["edit", professor];
            };


            Post["/edit/{Id}"] = x =>
            {
                var teacher = this.Bind<Teacher>();
                var result = new TeacherValidator().Validate(teacher, ruleSet: "Update");
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                Guid teacher_id = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Teacher>("TeachersById")
                    .Where(n => n.Id == teacher_id).FirstOrDefault();
                if (saved == null)
                    return new NotFoundResponse();
                saved.Fill(teacher);
                return Response.AsRedirect(string.Format("/teacher/{0}", teacher.Id));
            };


            Get["/new"] = x =>
            {
                return View["new", Teacher.DefaultTeacher()];
            };

            Post["/new"] = x =>
            {
                var teacher = this.Bind<Teacher>();
                var result = new TeacherValidator().Validate(teacher);
                if (!result.IsValid)
                {
                    return View["Shared/_errors", result];
                }
                DocumentSession.Store(teacher);
                return Response.AsRedirect(string.Format("/teacher/{0}", teacher.Id));
            };
            
        }
    }
}

