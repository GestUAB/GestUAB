using System.Linq;
using GestUAB.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client.Linq;

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
                DocumentSession.Query<UserModel> ().ToList ()];
            };
    
            Get ["/user/{Username}"] = x => { 
                var username = (string)x.Username;
//                var user = Database.Instance.ReadAll<UserModel> ().Where(u => u.Username == username).FirstOrDefault();
                var user = DocumentSession.Query<UserModel> ("UsersByUsername")
                    .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null)
                    return new NotFoundResponse ();
//                Context.EnableOutputCache(int.MaxValue);
                return View ["User/user", user];
            };

            Get ["/user/put"] = x => {
                return View ["User/put"];
            };

            Put ["/user/put"] = x => {
                var user = this.Bind<UserModel> ();
                DocumentSession.Store (user);
                var resp = new JsonResponse<UserModel> (
                    user,
                    new DefaultJsonSerializer ()
                );
                resp.Headers.Add ("Location", "/user/" + user.Username);
                resp.StatusCode = HttpStatusCode.Created;
                return resp;
//                return Response.AsRedirect("/users");
//                return View ["User/users", Database.Instance.Read<UserModel> ().ToArray ()];
            };

            Get ["/user/post/{Username}"] = x => { 
                var username = (string)x.Username;
//                var user = Database.Instance.ReadAll<UserModel> ().Where(u => u.Username == username).FirstOrDefault();
                var user = DocumentSession.Query<UserModel> ("UsersByUsername")
                    .Where (n => n.Username == username).FirstOrDefault ();
                if (user == null) 
                    return new NotFoundResponse ();
                return View ["User/post", user];
            };

            Post ["/user/post/{Username}"] = x => {
                var user = this.Bind<UserModel> ();
                var username = (string)x.Username;
                var saved = DocumentSession.Query<UserModel> ("UsersByUsername")
                    .Where (n => n.Username == username)
                    .FirstOrDefault ();
                if (saved == null) 
                    return new NotFoundResponse ();
                saved.Fill (user);
                var resp = new JsonResponse<UserModel> (
                    saved,
                    new DefaultJsonSerializer ()
                );
//                Context.DisableOutputCache();
                resp.Headers.Add ("Location", "/user/" + saved.Username);
                resp.StatusCode = HttpStatusCode.OK;
                return resp;
            };

            Delete ["/user/delete/{Username}"] = x => { 
//                string message = "";
                var username = (string)x.Username;
                var user = DocumentSession.Query<UserModel> ("UsersByUsername")
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
                var resp = new JsonResponse<UserModel> (
                        user,
                        new DefaultJsonSerializer ()
                );
                resp.StatusCode = HttpStatusCode.OK;
                return resp;

            };
    

        }
    }
}