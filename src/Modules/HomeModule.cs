using GestUAB.Models;
using System.Linq;

namespace GestUAB.Modules
{
    public class HomeModule : Nancy.NancyModule
    {

        public HomeModule ()
        {
            Get ["/"] = _ => { 
                return View ["Home/index"];
            };
        }
    }
    
}
