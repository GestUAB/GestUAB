using GestUAB.Models;
using System.Linq;

namespace GestUAB.Modules
{
    public class HomeModule : BaseModule
    {

        public HomeModule ()
        {
            Get ["/"] = _ => { 
                return View ["Home/index",
                DocumentSession.Query<UserModel>().ToList()];
            };
            Get ["/test"] = _ => { 
                return "test";
            };
        }
    }
    
}
