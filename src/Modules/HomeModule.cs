using System.Linq;
using GestUAB.Models;
using Nancy.Routing;

namespace GestUAB.Modules
{
    /// <summary>
    /// HomeModule class.
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
                return View ["about"];
            };
            #endregion
        }
    }
    
}
