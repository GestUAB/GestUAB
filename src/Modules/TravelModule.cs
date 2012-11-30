using System.Linq;
using GestUAB.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using FluentValidation;

namespace GestUAB.Modules
{
    public class TravelModule : BaseModule
    {
        public TravelModule ()
        {/*
            Get ["/travels"] = _ => { 
                var a = DocumentSession.Query<Travel> ().ToList ();
                return View ["Travel/travels", a];
            };
            
            Get ["/travel/create"] = x => { 
                return View ["Travel/create"];
            };

            Post ["/travel/create"] = x => {
                var travel = this.Bind<Travel> ();
                var result = new TravelValidator().Validate(travel);
                if (!result.IsValid) {
                    return View["Shared/_errors", result];
                }
                DocumentSession.Store (travel);
                var resp = new JsonResponse<Travel> (
                    travel,
                    new DefaultJsonSerializer ()
                );
                resp.Headers.Add ("Location", "/travel/" + travel.ID);
                resp.StatusCode = HttpStatusCode.Created;
                return resp;
            };*/

       
        }
    }
}

