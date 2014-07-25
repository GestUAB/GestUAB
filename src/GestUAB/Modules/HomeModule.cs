using System.Linq;
using Nancy.Routing;
using Nancy;

namespace GestUAB.Modules
{
    /// <summary>
    /// HomeModule class.
    /// </summary>
    public class HomeModule : NancyModule
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
                return View ["about"];
            };
            #endregion

            Get ["/test"] = _ => { 
                return View ["test"];
            };
            Get ["/bla"] = _ => { 
                return View ["bla"];
            };
            Get ["/foo"] = _ => { 
                return View ["foo"];
            };
        }
    }
    
}
