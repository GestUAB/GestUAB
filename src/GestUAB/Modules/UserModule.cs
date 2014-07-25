using Nancy;
using Nancy.ModelBinding;
using GestUAB.DataAccess;
using GestUAB.Models;

namespace GestUAB.Modules
{
    public class UserModule : NancyModule
    {

        public UserModule () : base("/users")
        {
            #region Method that returns the index View User, with the registered Users
            Get ["/"] = _ => { 
                var da = new UserDataAccess();
                return View ["index", da.ReadAll()];
            };
            #endregion

            #region Method that returns a View Show, displaying the User in the form according to the ID.
            Get ["/{Username}"] = x => { 
                var username = (string)x.Username;
                var da = new UserDataAccess();
                var user = da.Read(username);
                if (user == null)
                    return new NotFoundResponse ();
                return View ["show", user];
            };
            #endregion

            #region Method that returns the New View, creating a default User
            Get ["/new"] = x => { 
                return View ["new", new User()];
            };
            #endregion

            #region Method that creates and validates a new User in accordance with the specifications of the class UserValidator
            Post ["/new"] = x => {
                var user = this.Bind<User> ();
                var result = new UserValidator ().Validate (user);
                if (!result.IsValid) {
                    return View ["Shared/_errors", result];
                }
                var da = new UserDataAccess();
                da.Create (user);
                return Response.AsRedirect(string.Format("/users/{0}", user.UserName));
            };
            #endregion
//
//            #region Displays data in the form of the User according to Username
//            Get ["/edit/{Username}"] = x => {
//                var username = (string)x.Username;
//                var user = DocumentSession.Query<User> ("UsersByUsername")
//                    .Where (n => n.UserName == username).FirstOrDefault ();
//                if (user == null) 
//                    return new NotFoundResponse ();
//                return View ["edit", user];
//            };
//            #endregion
//
//            #region Method editing the Memorandum according to Username
//            Post ["/edit/{Username}"] = x => {
//                var user = this.Bind<User> ();
//                var result = new UserValidator ().Validate (user, ruleSet: "Update");
//                if (!result.IsValid) {
//                    return View ["Shared/_errors", result];
//                }
//                var username = (string)x.Username;
//                var saved = DocumentSession.Query<User> ("UsersByUsername")
//                    .Where (n => n.UserName == username)
//                    .FirstOrDefault ();
//                if (saved == null) 
//                    return new NotFoundResponse ();
//                saved.Fill (user);
//                return Response.AsRedirect(string.Format("/users/{0}", user.UserName));
//            };
//            #endregion
//
//            #region Method to delete a record according to Username
//            Delete ["/delete/{Username}"] = x => { 
//                var username = (string)x.Username;
//                var user = DocumentSession.Query<User> ("UsersByUsername")
//                        .Where (n => n.UserName == username)
//                        .FirstOrDefault ();
//                if (user == null) 
//                    return new NotFoundResponse ();
//                DocumentSession.Delete (user);
//                var resp = new JsonResponse<User> (
//                        user,
//                        new DefaultJsonSerializer ()
//                );
//                resp.StatusCode = HttpStatusCode.OK;
//                return resp;
//
//            };
//
//            Get ["/delete/{Username}"] = x => { 
//                var username = (string)x.Username;
//                var user = DocumentSession.Query<User> ("UsersByUsername")
//                    .Where (n => n.UserName == username).FirstOrDefault ();
//                if (user == null) 
//                    return new NotFoundResponse ();
//                DocumentSession.Delete (user);
//                return Response.AsRedirect("/users");
//            };
//            #endregion
        }
    }
}
