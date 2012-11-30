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
            
            #region Method that returns a View Show, displaying the User in the form according to the ID.
            Get ["/{Username}"] = x => { 
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null)
                return new NotFoundResponse ();
                return View ["show", user];
            };
            #endregion
            
            #region Method that returns the New View, creating a default User
            Get ["/new"] = x => { 
                return View ["new", User.DefaultUser()];
            };
#endregion
            
            #region Method that creates and validates a new User in accordance with the specifications of the class UserValidator
            Post ["/new"] = x => {
                var user = this.Bind<User> ();
                var result = new UserValidator ().Validate (user);
                if (!result.IsValid) {
                    return View ["Shared/_errors", result];
                }
                DocumentSession.Store (user);
                return Response.AsRedirect(string.Format("/users/{0}", user.Username));
            };
#endregion
            
            #region Displays data in the form of the User according to Username
            Get ["/edit/{Username}"] = x => {
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null) 
                return new NotFoundResponse ();
                return View ["edit", user];
            };
#endregion
            
            #region Method editing the Memorandum according to Username
            Post ["/edit/{Username}"] = x => {
                var user = this.Bind<User> ();
                var result = new UserValidator ().Validate (user, ruleSet: "Update");
                if (!result.IsValid) {
                    return View ["Shared/_errors", result];
                }
                var username = (string)x.Username;
                var saved = DocumentSession.Query<User> ("UsersByUsername")
                .Where (n => n.Username == username)
                .FirstOrDefault ();
                if (saved == null) 
                return new NotFoundResponse ();
                saved.Fill (user);
                return Response.AsRedirect(string.Format("/users/{0}", user.Username));
            };
#endregion
            
            #region Method to delete a record according to Username
            Delete ["/delete/{Username}"] = x => { 
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                .Where (n => n.Username == username)
                .FirstOrDefault ();
                if (user == null) 
                return new NotFoundResponse ();
                DocumentSession.Delete (user);
                var resp = new JsonResponse<User> (
                    user,
                    new DefaultJsonSerializer ()
                    );
                resp.StatusCode = HttpStatusCode.OK;
                return resp;
                
            };
            
            Get ["/delete/{Username}"] = x => { 
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null) 
                return new NotFoundResponse ();
                DocumentSession.Delete (user);
                return Response.AsRedirect("/users");
            };
#endregion

       
        }
    }
}

