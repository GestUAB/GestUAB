using System.Linq;
using GestUAB.Models;
using Nancy.Routing;

namespace GestUAB.Modules
{
    public class HomeModule : BaseModule
    {

        public HomeModule ()
        {
            Get ["/"] = x => { 
                return View ["index"];
            };
            Get ["/about"] = _ => { 
                return View ["about"];
            };
        }
    }
    
}
