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
                DocumentSession.Query<UserModel>().ToList()];
            };
        }
    }
    
}
