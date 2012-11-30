using System.Linq;
using GestUAB.Models;
using Nancy.Routing;

namespace GestUAB.Modules
{
    /// <summary>
    /// HomeModule Class
    /// </summary>
    public class HomeModule : BaseModule
    {
        /// <summary>
        /// HomeModule Builder
        /// </summary>
        public HomeModule () : base()
        {
            #region Index
            Get ["/"] = x => { 
                return View ["index"];
            };
            #endregion

            #region About
            Get ["/about"] = _ => { 
<<<<<<< HEAD
                return View ["Home/about",
                DocumentSession.Query<User>().ToList()];
<<<<<<< HEAD
            };
            Get ["/test"] = _ => { 
                return "test";
=======
>>>>>>> 541c1c286094df6ca08a6cd48d9a3955e4db6068
=======
                return View ["about"];
>>>>>>> 2ee53ef89a9e3a41f20ca0c5f8d00b21bb0f480f
            };
            #endregion
        }
    }
    
}
