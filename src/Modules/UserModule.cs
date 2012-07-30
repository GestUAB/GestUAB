using System.Linq;
using GestUAB.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using Raven.Client.UniqueConstraints;
using Nancy.Responses;

namespace GestUAB.Modules
{
    public class UserModule : BaseModule
    {

        public UserModule ()
        {
            Get ["/users"] = _ => { 
                return View ["User/users", DocumentSession.Query<User> ().ToList ()];
            };
    
            Get ["/user/{Username}"] = x => { 
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                    .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null)
                    return new NotFoundResponse ();
                return View ["User/user", user];
            };

            Get ["/user/update/{Username}"] = x => {
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                    .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null) 
                    return new NotFoundResponse ();
                return View ["User/update", user];
            };

            Put ["/user/update/{Username}"] = x => {
                var user = this.Bind<User> ();
                var username = (string)x.Username;
                var saved = DocumentSession.Query<User> ("UsersByUsername")
                    .Where (n => n.Username == username)
                    .FirstOrDefault ();
                if (saved == null) 
                    return new NotFoundResponse ();
                saved.Fill (user);
                var resp = new JsonResponse<User> (
                    saved,
                    new DefaultJsonSerializer ()
                );
                resp.Headers.Add ("Location", "/user/" + saved.Username);
                resp.StatusCode = HttpStatusCode.OK;
                return resp;
            };

            Get ["/user/create"] = x => { 
                return View ["User/create"];
            };

            Post ["/user/create"] = x => {
                var user = this.Bind<User> ();
                var result = this.Validate(user);
                if (!result.IsValid) {
                    return View["Shared/_errors", result];
                }

                DocumentSession.Store (user);
                var resp = new JsonResponse<User> (
                    user,
                    new DefaultJsonSerializer ()
                );
                resp.Headers.Add ("Location", "/user/" + user.Username);
                resp.StatusCode = HttpStatusCode.Created;
                return resp;
            };

            Delete ["/user/delete/{Username}"] = x => { 
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
        }
    }
}
