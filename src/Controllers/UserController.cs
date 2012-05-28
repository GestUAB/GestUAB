using Nancy;
using Gestuab.Models;
//using Raven.Client;
//using Raven.Client.Linq;
//using Raven.Client.Embedded;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Gestuab.Controllers
{
    public class UserController : NancyModule
    {

        public UserController ()
        {


            Get ["/"] = _ => { 
                return View ["user", new UserModel { FirstName = "John", LastName = "Doe" }];
            };
    
            Post ["/"] = x => {
                var user = new UserModel { FirstName = Request.Form.FirstName, LastName = Request.Form.LastName };
                return View ["user", user];
            };
        }
    }
}