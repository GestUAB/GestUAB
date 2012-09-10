using System.Linq;
using GestUAB.Models;

namespace GestUAB.Modules
{
    public class HomeModule : BaseModule
    {

        public HomeModule ()
        {
            Get ["/"] = _ => { 
                return View ["Home/index",
                DocumentSession.Query<User>().ToList()];
            };
            Get ["/about"] = _ => { 
                return View ["Home/about",
                DocumentSession.Query<User>().ToList()];
            };
        }
    }
    
}
