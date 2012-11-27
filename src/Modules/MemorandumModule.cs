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
    public class MemorandumModule : BaseModule
    {
        public MemorandumModule () : base("/memorandums")
        {
            #region Method that returns the index View Memorandum, with the registered Memorandum
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Memorandum> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };
            #endregion

            #region Method that returns a View Show, displaying the memorandum in the form according to the ID.
            Get ["/{Id}"] = x => { 
                Guid memorandumnumber = Guid.Parse(x.Id);
                var memorandum = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where (n => n.Id == memorandumnumber).FirstOrDefault ();
                if (memorandum == null)
                    return new NotFoundResponse ();
                return View ["show", memorandum];
            };
            #endregion

            #region Method that returns the New View, creating a default Memorandum
            Get ["/new"] = x => {
                return View ["new", Memorandum.DefaultMemorandum()];
            };
            #endregion

            #region Method that creates and validates a new memorandum in accordance with the specifications of the class MemorandumValidator
            Post ["/new"] = x => {
                var memorandum = this.Bind<Memorandum> ();
                var result = new MemorandumValidator ().Validate (memorandum);
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (memorandum);
                return Response.AsRedirect(string.Format("/memorandums/{0}", memorandum.Id));
            };
            #endregion

            #region Displays data in the form of the Memorandum according to ID
            Get ["/edit/{Id}"] = x => {
                Guid memorandumsnumber = Guid.Parse(x.Id);
                var memorandum = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Where (n => n.Id == memorandumsnumber).FirstOrDefault ();
                if (memorandum == null)
                    return new NotFoundResponse ();
                return View ["edit", memorandum];
            };
            #endregion

            #region Method editing the Memorandum according to ID
            Post ["/edit/{Id}"] = x => {
                var memorandum = this.Bind<Memorandum>();
                var result = new MemorandumValidator ().Validate (memorandum, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                Guid memorandumnumber = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Where (n => n.Id == memorandumnumber).FirstOrDefault ();
                if (saved == null) 
                    return new NotFoundResponse ();
                saved.Fill (memorandum);
                return Response.AsRedirect(string.Format("/memorandums/{0}", memorandum.Id));
            };
            #endregion

            #region Method to delete a record according to ID
            Get ["/delete/{Id}"] = x => { 
                Guid memorandumnumber = Guid.Parse(x.Id);
                var memorandum = DocumentSession.Query<Memorandum> ("MemorandumById")
                    .Where (n => n.Id == memorandumnumber).FirstOrDefault ();
                if (memorandum == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (memorandum);
                return Response.AsRedirect("/memorandums");
            };
            #endregion
        }
    }
}

