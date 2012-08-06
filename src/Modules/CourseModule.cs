using Nancy;
using GestUAB.Models;
using Raven.Client.Linq;
using System.Linq;
using Nancy.Responses;
using Nancy.ModelBinding;
using Nancy.Validation;

namespace GestUAB.Modules
{
	public class CourseModule : BaseModule
	{
		public CourseModule ()
		{
			Get ["/courses"] = _ => { 
                return View ["index", DocumentSession.Query<Course>().ToList()];
            };
			
			Get ["course/{Number}"] = x => { 
                var coursenumber = (int)x.Number;
                var course = DocumentSession.Query<Course> ("CoursesByNumber")
                    .Where (n => n.Number == coursenumber).FirstOrDefault ();
                if (course == null)
                    return new NotFoundResponse();
                return View ["show", course];
            };
			
			Get ["courses/new"] = x => {
                return View ["new", new Course()];
			};

            Post ["courses/create"] = x => {
                var course = this.Bind<Course>();
                var result = this.Validate(course);
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                DocumentSession.Store(course);
                return View ["show", course];
            };
			
            Get ["courses/edit/{Number}"] = x => {
                var coursenumber = (int)x.Number;
                var course = DocumentSession.Query<Course>("CoursesByNumber")
                    .Where (n => n.Number == coursenumber).FirstOrDefault();
                if (course == null)
                    return new NotFoundResponse();
                return View ["edit", course];
            };

            Post ["courses/update/{Number}"] = x => {
                var course = this.Bind<Course>();
                var result = this.Validate(course);
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                var coursenumber = (int)x.Number;
                var saved = DocumentSession.Query<Course>("CoursesByNumber")
                    .Where (n => n.Number == coursenumber).FirstOrDefault();
                if (saved == null) 
                    return new NotFoundResponse();
                saved.Fill(course);
                return View ["show", course];
            };

            Delete ["courses/delete/{Number}"] = x => { 
                var coursenumber = (int)x.Number;
                var course = DocumentSession.Query<Course>("CoursesByNumber")
                    .Where (n => n.Number == coursenumber).FirstOrDefault();
                if (course == null) 
                    return new NotFoundResponse();
                DocumentSession.Delete(course);
                return View ["index", course];
            };
		}
	}
}
