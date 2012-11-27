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
    public class CourseModule : BaseModule
    {
        public CourseModule () : base("/courses")
        {
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Course> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };
            
            Get ["/{Id}"] = x => { 
                Guid coursenumber = Guid.Parse(x.Id);
                var course = DocumentSession.Query<Course> ("CoursesById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where (n => n.Id == coursenumber).FirstOrDefault ();
                if (course == null)
                    return new NotFoundResponse ();
                return View ["show", course];
            };
            
            Get ["/new"] = x => {
                return View ["new", Course.DefaultCourse()];
            };

            Post ["/new"] = x => {
                var course = this.Bind<Course> ();
                var result = new CourseValidator().Validate (course);
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (course);
                return Response.AsRedirect(string.Format("/courses/{0}", course.Id));
            };
            
            Get ["/edit/{Id}"] = x => {
                Guid coursenumber = Guid.Parse(x.Id);
                var course = DocumentSession.Query<Course> ("CoursesById")
                    .Where (n => n.Id == coursenumber).FirstOrDefault ();
                if (course == null)
                    return new NotFoundResponse ();
                return View ["edit", course];
            };

            Post ["/edit/{Id}"] = x => {
                var course = this.Bind<Course> ();
                var result = new CourseValidator ().Validate (course, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                Guid coursenumber = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Course> ("CoursesById")
                    .Where (n => n.Id == coursenumber).FirstOrDefault ();
                if (saved == null) 
                    return new NotFoundResponse ();
                saved.Fill (course);
                return Response.AsRedirect(string.Format("/courses/{0}", course.Id));
            };

            Delete ["/delete/{Id}"] = x => { 
                Guid coursenumber = Guid.Parse(x.Id);
                var course = DocumentSession.Query<Course> ("CoursesById")
                        .Where (n => n.Id == coursenumber)
                        .FirstOrDefault ();
                if (course == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (course);
                var resp = new JsonResponse<Course> (
                        course,
                        new DefaultJsonSerializer ()
                );
                resp.StatusCode = HttpStatusCode.OK;
                return resp;

            };

            Get ["/delete/{Id}"] = x => { 
                Guid coursenumber = Guid.Parse(x.Id);
                var course = DocumentSession.Query<Course> ("CoursesById")
                    .Where (n => n.Id == coursenumber).FirstOrDefault ();
                if (course == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (course);
                return Response.AsRedirect("/courses");
            };
        }
    }
}
