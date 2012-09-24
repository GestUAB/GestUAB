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
    public class MemorandumModule : BaseModule
    {
        public MemorandumModule () : base("/memorandums")
        {

            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Memorandum> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };

            Get ["/{Id}"] = x => { 
                Guid memorandumnumber = Guid.Parse(x.Id);
                var memorandum = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where (n => n.Id == memorandumnumber).FirstOrDefault ();
                if (memorandum == null)
                    return new NotFoundResponse ();
                return View ["show", memorandum];
            };

            Get ["/new"] = x => {
                return View ["new", new Memorandum ()];
            };

            Post ["/new"] = x => {
                var memorandum = this.Bind<Memorandum> ();
                var result = this.Validate (memorandum);
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (memorandum);
                //return View ["show", course];
                return Response.AsRedirect(string.Format("/memorandums/{0}", memorandum.Id));
            };

            Get ["/edit/{Id}"] = x => {
                Guid memorandumsnumber = Guid.Parse(x.Id);
                var memorandum = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Where (n => n.Id == memorandumsnumber).FirstOrDefault ();
                if (memorandum == null)
                    return new NotFoundResponse ();
                return View ["edit", memorandum];
            };

            Post ["/edit/{Id}"] = x => {
                var memorandum = this.Bind<Memorandum> ();
                var result = new MemorandumValidator ().Validate (memorandum, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                Guid memorandumnumber = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Where (n => n.Id == memorandumnumber).FirstOrDefault ();
                if (saved == null) 
                    return new NotFoundResponse ();
                saved.Fill (memorandum);
                return Response.AsRedirect(string.Format("/memorandums/{0}", memorandum.Id));
            };
//
//            Delete ["/delete/{Id}"] = x => { 
//                Guid coursenumber = Guid.Parse(x.Id);
//                var course = DocumentSession.Query<Course> ("CoursesById")
//                        .Where (n => n.Id == coursenumber)
//                        .FirstOrDefault ();
//                if (course == null) 
//                    return new NotFoundResponse ();
//                DocumentSession.Delete (course);
//                var resp = new JsonResponse<Course> (
//                        course,
//                        new DefaultJsonSerializer ()
//                );
//                resp.StatusCode = HttpStatusCode.OK;
//                return resp;
//
//            };
//
            Get ["/delete/{Id}"] = x => { 
                Guid memorandumnumber = Guid.Parse(x.Id);
                var memorandum = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Where (n => n.Id == memorandumnumber).FirstOrDefault ();
                if (memorandum == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (memorandum);
                return Response.AsRedirect("/memorandums");
            };
        }
    }
}

