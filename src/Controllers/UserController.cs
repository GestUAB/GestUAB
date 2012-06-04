using Nancy;
using Gestuab.Models;

//using Raven.Client;
//using Raven.Client.Linq;
//using Raven.Client.Embedded;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Nancy.Responses;

namespace Gestuab.Controllers
{
    public class UserController : NancyModule
    {

        public UserController ()
        {

            var users = new List<UserModel> ();
            users.Add (new UserModel{UserName = "thild", FirstName = "Tony", LastName= "Hild"});
            users.Add (new UserModel{UserName = "jbarbosa", FirstName = "João", LastName= "Barbosa"});
            users.Add (new UserModel{UserName = "rciqueira", FirstName = "Ricardo", LastName= "Ciqueira"});

            Get ["/"] = _ => { 
                return View ["User/user",
                new UserModel { FirstName = "John", LastName = "Doe" }];
            };
    
            Get ["/users"] = _ => { 
                return View ["User/users", users];
            };
    
            Get ["/user/{username}"] = x => { 
                var user = (from n in users where n.UserName == x.username select n ).FirstOrDefault();
                return View ["User/user", user];
            };
    
            Post ["/"] = x => {
                var user = new UserModel { FirstName = Request.Form.FirstName, LastName = Request.Form.LastName };
                return View ["User/user", user];
            };

            Get ["/user/delete"] = x => { 
                return View ["User/delete"];
            };

            Delete ["/user/delete"] = x => { 
                string message = "";
                if (users.RemoveAll(n => n.UserName == Request.Form.UserName) > 0) {
                    message = "Usuário " + Request.Form.UserName + " excluído!";
                }
                else {
                    message = "Usuário " + Request.Form.UserName + " não encontrado!";
                }
                return new JsonResponse<object>(new {Message = message}, 
                        new DefaultJsonSerializer());
            };
    

        }
    }
}