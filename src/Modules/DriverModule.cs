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
    public class DriverModule : BaseModule
    {
        public DriverModule() : base("/drivers")
        {
            Get["/"] = _ => {
                return View["index", DocumentSession.Query<Driver>()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList()];
            };

            Get["/{Id}"] = x => {
                Guid driverId = Guid.Parse(x.Id);
                var driver = DocumentSession.Query<Driver>("DriverById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.Id == driverId).FirstOrDefault();
                if (driver == null)
                    return new NotFoundResponse();
                return View["show", driver];
            };

            Get["/new"] = x => {
                return View["new", Driver.DefaultDriver()];
            };

            Post["/new"] = x => {
                var driver = this.Bind<Driver>();
                var result = new DriverValidator().Validate(driver);
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                DocumentSession.Store(driver);
                return Response.AsRedirect(string.Format("/drivers/{0}",driver.Id));
            };

            Get["/edit/{Id}"] = x => {
                Guid driverId = Guid.Parse(x.Id);
                var driver = DocumentSession.Query<Driver>("DriverById")
                    .Where(n => n.Id == driverId).FirstOrDefault();
                if(driver == null)
                    return new NotFoundResponse();
                return View["edit", driver];
            };

            Post["/edit/{Id}"] = x => {
                var driver = this.Bind<Driver>();
                var result = new DriverValidator().Validate(driver, ruleSet: "Update");
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                Guid driverId = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Driver>("DriverById")
                    .Where(n => n.Id == driverId).FirstOrDefault();
                if (saved == null)
                    return new NotFoundResponse();
                saved.Fill(driver);
                return Response.AsRedirect(string.Format("/drivers/{0}", driver.Id));
            };

            Get["/delete/{Id}"] = x => {
                Guid driverId = Guid.Parse(x.Id);
                var driver = DocumentSession.Query<Driver>("DriverById")
                    .Where(n => n.Id == driverId).FirstOrDefault();
                if (driver == null)
                    return new NotFoundResponse();
                DocumentSession.Delete(driver);
                return Response.AsRedirect("/drivers");
            };

        }
    }
}