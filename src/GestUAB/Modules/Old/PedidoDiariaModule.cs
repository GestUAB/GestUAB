using System;
using Nancy;
using GestUAB.Models;
using Raven.Client.Linq;
using System.Linq;
using Nancy.ModelBinding;
using Nancy.Validation;
using FluentValidation;
using Nancy.Responses;

namespace GestUAB.Modules
{
    public class PedidoDiariaModule : BaseModule
    {
        public PedidoDiariaModule () : base("/pedidosdiaria")
        {
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<PedidoDiaria> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };

            Get ["/{Id}"] = x => { 
                Guid id = Guid.Parse(x.Id);
                var m = DocumentSession.Query<PedidoDiaria>("PedidoDiariaById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.Id == id).FirstOrDefault();
                if (m == null)
                    return new NotFoundResponse();
                return View["show", m];
            };

            Get ["/new"] = x => {
                return View ["new"];
            };

            Post ["/new"] = x => {
                var m = this.Bind<PedidoDiaria> ();
                var result = new PedidoDiariaValidator ().Validate (m);
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (m);
                return Response.AsRedirect(string.Format("/pedidosdiaria/{0}", m.Id));
            };

            Get ["/edit/{Id}"] = x => {
                Guid id = Guid.Parse(x.Id);
                var m = DocumentSession.Query<PedidoDiaria> ("PedidoDiariaById")
                    .Where (n => n.Id == id).FirstOrDefault ();
                if (m == null)
                    return new NotFoundResponse ();
                return View ["edit", m];
            };

            Post ["/edit/{Id}"] = x => {
                var m = this.Bind<PedidoDiaria>();
                var result = new PedidoDiariaValidator ().Validate (m, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                Guid id = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<PedidoDiaria> ("PedidoDiariaById")
                    .Where (n => n.Id == id).FirstOrDefault ();
                if (saved == null) 
                    return new NotFoundResponse ();
                saved.Fill (m);
                return Response.AsRedirect(string.Format("/pedidosdiaria/{0}", m.Id));
            };

            Get ["/delete/{Id}"] = x => { 
                Guid id = Guid.Parse(x.Id);
                var m = DocumentSession.Query<PedidoDiaria> ("PedidoDiariaById")
                    .Where (n => n.Id == id).FirstOrDefault ();
                if (m == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (m);
                return Response.AsRedirect("/pedidosdiaria");
            };
        }
    }
}

