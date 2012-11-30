using System.Linq;
using GestUAB.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using FluentValidation;

namespace GestUAB.Modules
{
    public class TravelModule : BaseModule
    {
        public TravelModule ()
        {
            #region Method that returns the index View Travel, with the scheduled travels
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Travel> ()
                             .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                             .ToList ()];
            };
            #endregion
            
            #region Method that returns a View Show, displaying the Travel of the teacher.
            Get ["/{TeacherName}"] = x => { 
                var teacher = (string)x.TeacherName;
                var travel = DocumentSession.Query<Travel> ("TravelByTeacher")
                .Where (n => n.TeacherName == teacher).FirstOrDefault ();
                if (travel == null)
                return new NotFoundResponse ();
                return View ["show", travel];
            };
            #endregion
            
            #region Method that returns the New View, creating a default Travel
            Get ["/new"] = x => { 
                return View ["new", Travel.DefaultTravel()];
            };
            #endregion
            
            #region Method that creates and validates a new Travel in accordance with the specifications of the class TravelValidator
            Post ["/new"] = x => {
                var travel = this.Bind<Travel> ();
                var result = new TravelValidator ().Validate (travel);
                if (!result.IsValid) {
                    return View ["Shared/_errors", result];
                }
                DocumentSession.Store (travel);
                return Response.AsRedirect(string.Format("/travels/{0}", travel.TeacherName));
            };
            #endregion
            
            #region Displays data in the form of the Travel according to TeacherName
            Get ["/edit/{TeacherName}"] = x => {
                var teacher = (string)x.TeacherName;
                var travel = DocumentSession.Query<Travel> ("TravelByTeacher")
                .Where (n => n.TeacherName == teacher).FirstOrDefault ();
                if (travel == null) 
                return new NotFoundResponse ();
                return View ["edit", travel];
            };
            #endregion
            
            #region Method editing the Memorandum according to TeacherName
            Post ["/edit/{TeacherName}"] = x => {
                var travel = this.Bind<Travel> ();
                var result = new TravelValidator ().Validate (travel, ruleSet: "Update");
                if (!result.IsValid) {
                    return View ["Shared/_errors", result];
                }
                var teacher = (string)x.TeacherName;
                var saved = DocumentSession.Query<Travel> ("TravelByTeacher")
                .Where (n => n.TeacherName == teacher)
                .FirstOrDefault ();
                if (saved == null) 
                return new NotFoundResponse ();
                saved.Fill (teacher);
                return Response.AsRedirect(string.Format("/travels/{0}", travel.TeacherName));
            };
            #endregion
            
            #region Method to delete a record according to TeacherName
            Delete ["/delete/{TeacherName}"] = x => { 
                var teacher = (string)x.TeacherName;
                var travel = DocumentSession.Query<Travel> ("TravelByTeacher")
                .Where (n => n.TeacherName == teacher)
                .FirstOrDefault ();
                if (travel == null) 
                return new NotFoundResponse ();
                DocumentSession.Delete (travel);
                var resp = new JsonResponse<Travel> (
                    travel,
                    new DefaultJsonSerializer ()
                    );
                resp.StatusCode = HttpStatusCode.OK;
                return resp;
                
            };
            
            Get ["/delete/{TravelName}"] = x => { 
                var teacher = (string)x.TeacherName;
                var travel = DocumentSession.Query<Travel> ("TravelByTeacher")
                .Where (n => n.TeacherName == teacher).FirstOrDefault ();
                if (travel == null) 
                return new NotFoundResponse ();
                DocumentSession.Delete (travel);
                return Response.AsRedirect("/travels");
            };
            #endregion

       
        }
    }
}

