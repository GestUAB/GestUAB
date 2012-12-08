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
    public class RenunciationModule : BaseModule
    {
        public RenunciationModule() : base("/renunciations")
        {
            #region 
            Get["/"] = _ =>
            {
                return View["index", DocumentSession.Query<Renunciation>()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList()];
            };
            #endregion

            #region 
            Get["/{Id}"] = x =>
            {
                Guid renunciationId = Guid.Parse(x.Id);
                var renunciation = DocumentSession.Query<Renunciation >("RenunciationById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.Id == renunciationId).FirstOrDefault();
                if (renunciation == null)
                    return new NotFoundResponse();
                return View["show", renunciation];
            };
            #endregion

            #region 
            Get["/new"] = x =>
            {
                return View["new", Renunciation.DefaultRenunciation()];
            };
            #endregion

            #region 
            Post["/new"] = x =>
            {
                var renunciation = this.Bind<Renunciation>();
                var result = new RenunciationValidator().Validate(renunciation);
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                DocumentSession.Store(renunciation);
                return Response.AsRedirect(string.Format("/renunciations/{0}", renunciation.Id));
            };
            #endregion

            #region
            Get["/edit/{Id}"] = x =>
            {
                Guid renunciationId = Guid.Parse(x.Id);
                var renunciation = DocumentSession.Query<Renunciation>("RenunciationById")
                    .Where(n => n.Id == renunciationId).FirstOrDefault();
                if (renunciation == null)
                    return new NotFoundResponse();
                return View["edit", renunciation];
            };
            #endregion

            #region 
            Post["/edit/{Id}"] = x =>
            {
                var renunciation = this.Bind<Renunciation>();
                var result = new RenunciationValidator().Validate(renunciation, ruleSet: "Update");
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                Guid renunciationId = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Renunciation>("RenunciationById")
                    .Where(n => n.Id == renunciationId).FirstOrDefault();
                if (saved == null)
                    return new NotFoundResponse();
                saved.Fill(renunciation);
                return Response.AsRedirect(string.Format("/renunciations/{0}", renunciation.Id));
            };
            #endregion

            #region 
            Get["/delete/{Id}"] = x =>
            {
                Guid renunciationId = Guid.Parse(x.Id);
                var renunciation = DocumentSession.Query<Renunciation>("RenunciationById")
                    .Where(n => n.Id == renunciationId).FirstOrDefault();
                if (renunciation == null)
                    return new NotFoundResponse();
                DocumentSession.Delete(renunciation);
                return Response.AsRedirect("/renunciations");
            };
            #endregion
        }
    }
}
