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
<<<<<<< HEAD
            };
            Get ["/test"] = _ => { 
                return "test";
=======
>>>>>>> 541c1c286094df6ca08a6cd48d9a3955e4db6068
            };
        }
    }
    
}
