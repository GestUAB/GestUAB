using System.Linq;
using GestUAB.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using FluentValidation;

namespace GestUAB.Modules
{
    public class UserModule : BaseModule
    {

        public UserModule () : base("/users")
        {
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<User> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };
    
            Get ["/{Username}"] = x => { 
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null)
                    return new NotFoundResponse ();
                return View ["show", user];
            };

            Get ["/new"] = x => { 
                return View ["new", User.DefaultUser()];
            };

            Post ["/new"] = x => {
                var user = this.Bind<User> ();
                var result = new UserValidator ().Validate (user);
                if (!result.IsValid) {
                    return View ["Shared/_errors", result];
                }
                DocumentSession.Store (user);
                return Response.AsRedirect(string.Format("/users/{0}", user.Username));
            };

            Get ["/edit/{Username}"] = x => {
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                    .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null) 
                    return new NotFoundResponse ();
                return View ["edit", user];
            };
            
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
        }
    }
}
