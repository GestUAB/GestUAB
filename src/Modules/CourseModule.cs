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
    /// <summary>
    /// Course Module.
    /// </summary>
    public class CourseModule : BaseModule
    {
        /// <summary>
        /// Builder CourseModule Class.
        /// </summary>
        public CourseModule () : base("/courses")
        {
            #region Method that returns the index View Course, with the registered courses.
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Course> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };
            #endregion

            #region Method that returns a View Show, displaying the course in the form according to the ID.
            Get ["/{Id}"] = x => { 
                Guid coursenumber = Guid.Parse(x.Id);
                var course = DocumentSession.Query<Course> ("CoursesById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where (n => n.Id == coursenumber).FirstOrDefault ();
                if (course == null)
                    return new NotFoundResponse ();
                return View ["show", course];
            };
            #endregion

            #region Method that returns the New View, creating a default course
            Get ["/new"] = x => {
                return View ["new", Course.DefaultCourse()];
            };
            #endregion

            #region Method that creates and validates a new course in accordance with the specifications of the class CourseValidator
            Post ["/new"] = x => {
                var course = this.Bind<Course> ();
                var result = new CourseValidator().Validate (course);
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (course);
                return Response.AsRedirect(string.Format("/courses/{0}", course.Id));
            };
            #endregion

            #region Displays data in the form of the Course according to ID
            Get ["/edit/{Id}"] = x => {
                Guid coursenumber = Guid.Parse(x.Id);
                var course = DocumentSession.Query<Course> ("CoursesById")
                    .Where (n => n.Id == coursenumber).FirstOrDefault ();
                if (course == null)
                    return new NotFoundResponse ();
                return View ["edit", course];
            };
            #endregion

            #region Method editing the course according to ID
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
            #endregion

            #region Method to delete a record according to ID
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
            #endregion

            Get["/select"] = _ =>
            {
                return View["select", DocumentSession.Query<Course>()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList()];
            };
        }
    }
}
