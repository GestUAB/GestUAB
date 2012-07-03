using Nancy;
using GestUAB.Models;
using Raven.Client.Linq;
using System.Linq;
using Nancy.Responses;
using Nancy.ModelBinding;

namespace GestUAB.Modules
{
    public class UserModule : BaseModule
    {

        public UserModule ()
        {
            Get ["/users"] = _ => { 
                //http://stackoverflow.com/questions/5399967/parse-string-into-a-linq-query
                //http://www.codeproject.com/Articles/43678/Dynamically-evaluated-SQL-LINQ-queries
                return View ["User/users",
                DocumentSession.Query<User> ().ToList ()];
            };
    
            Get ["/user/{Username}"] = x => { 
                var username = (string)x.Username;
//                var user = Database.Instance.ReadAll<UserModel> ().Where(u => u.Username == username).FirstOrDefault();
                var user = DocumentSession.Query<User> ("UsersByUsername")
                    .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null)
                    return new NotFoundResponse ();
//                Context.EnableOutputCache(int.MaxValue);
                return View ["User/user", user];
            };

            Get ["/user/update/{Username}"] = x => {
                var username = (string)x.Username;
//                var user = Database.Instance.ReadAll<UserModel> ().Where(u => u.Username == username).FirstOrDefault();
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
//                Context.DisableOutputCache();
                resp.Headers.Add ("Location", "/user/" + saved.Username);
                resp.StatusCode = HttpStatusCode.OK;
                return resp;
            };

            Get ["/user/create"] = x => { 
                return View ["User/create"];
            };

            Post ["/user/create"] = x => {
                var user = this.Bind<User> ();
                DocumentSession.Store (user);
                var resp = new JsonResponse<User> (
                    user,
                    new DefaultJsonSerializer ()
                );
                resp.Headers.Add ("Location", "/user/" + user.Username);
                resp.StatusCode = HttpStatusCode.Created;
                return resp;
//                return Response.AsRedirect("/users");
//                return View ["User/users", Database.Instance.Read<UserModel> ().ToArray ()];
            };

            Delete ["/user/delete/{Username}"] = x => { 
//                string message = "";
                var username = (string)x.Username;
                var user = DocumentSession.Query<User> ("UsersByUsername")
                        .Where (n => n.Username == username)
                        .FirstOrDefault ();
                if (user == null) 
                    return new NotFoundResponse ();
//                if (user == default(UserModel)) {
//                    message = "Usuário " + x.Username + " excluído!";
//                    Database.Instance.Delete (user);
//                } else {
//                    message = "Usuário " + x.Username + " não encontrado!";
//                }
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