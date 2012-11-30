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
    public class DesistenceModule : BaseModule
    {
        public DesistenceModule() : base("/desistences")
        {
            #region Method that returns the index View Desistence, with the registered desistence.
            Get["/"] = _ => {
                return View["index", DocumentSession.Query<Desistence>()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList()];
            };
            #endregion

            #region  Method that returns a View Show, displaying the desistence in the form according to the ID.
            Get["/{Id}"] = x => {
                Guid desistenceId = Guid.Parse(x.Id);
                var desistence = DocumentSession.Query<Desistence>("DesistenceById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.Id == desistenceId).FirstOrDefault();
                if (desistence == null)
                    return new NotFoundResponse();
                return View["show", desistence];
            };
            #endregion

            #region Method that returns the New View, creating a default desistence
            Get["/new"] = x => {
                return View["new", Desistence.DefaultDesistence()];
            };
            #endregion

            #region Method that creates and validates a new desistence in accordance with the specifications of the class DesistenceValidator
            Post["/new"] = x => {
                var desistence = this.Bind<Desistence>();
                var result = new DesistenceValidator().Validate(desistence);
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                DocumentSession.Store(desistence);
                return Response.AsRedirect(string.Format("/desistences/{0}", desistence.Id));
            };
            #endregion

            #region Displays data in the form of the Desistence according to ID
            Get["/edit/{Id}"] = x => {
                Guid desistenceId = Guid.Parse(x.Id);
                var desistence = DocumentSession.Query<Desistence>("DesistenceById")
                    .Where(n => n.Id == desistenceId).FirstOrDefault();
                if (desistence == null)
                    return new NotFoundResponse();
                return View["edit", desistence];
            };
            #endregion

            #region Method editing the desistence according to ID
            Post["/edit/{Id}"] = x => {
                var desistence = this.Bind<Desistence>();
                var result = new DesistenceValidator().Validate(desistence, ruleSet: "Update");
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                Guid desistenceId = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Desistence>("DesistenceById")
                    .Where(n => n.Id == desistenceId).FirstOrDefault();
                if (saved == null)
                    return new NotFoundResponse();
                saved.Fill(desistence);
                return Response.AsRedirect(string.Format("/desistences/{0}", desistence.Id));
            };
            #endregion

            #region Method to delete a record according to ID
            Get["/delete/{Id}"] = x => 
            {
                Guid desistenceId = Guid.Parse(x.Id);
                var desistence = DocumentSession.Query<Desistence>("DesistenceById")
                    .Where(n => n.Id == desistenceId).FirstOrDefault();
                if (desistence == null)
                    return new NotFoundResponse();
                DocumentSession.Delete(desistence);
                return Response.AsRedirect("/desistences");
            };
            #endregion
        }
    }
}