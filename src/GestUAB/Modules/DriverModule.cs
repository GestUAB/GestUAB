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
    /// <summary>
    /// Driver Module
    /// </summary>
    /// 
    public class DriverModule : BaseModule
    {
        /// <summary>
        /// Builder DriveModule Class
        /// </summary>
        /// 
        public DriverModule() : base("/drivers")
        {
            #region Method that returns the index View Course, with the registered Drivers
            Get["/"] = _ => {
                return View["index", DocumentSession.Query<Driver>()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList()];
            };
            #endregion

            #region Method that returns a View Show, displaying the driver in the form according to the ID.
            Get["/{Id}"] = x => {
                Guid driverId = Guid.Parse(x.Id);
                var driver = DocumentSession.Query<Driver>("DriverById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where(n => n.Id == driverId).FirstOrDefault();
                if (driver == null)
                    return new NotFoundResponse();
                return View["show", driver];
            };
            #endregion

            #region  Method that returns the New View, creating a default driver
            Get["/new"] = x => {
                return View["new", Driver.DefaultDriver()];
            };
            #endregion

            #region Method that creates and validates a new driver in accordance with the specifications of the class DriverValidator
            Post["/new"] = x => {
                var driver = this.Bind<Driver>();
                var result = new DriverValidator().Validate(driver);
                if (!result.IsValid)
                    return View["Shared/_errors", result];
                DocumentSession.Store(driver);
                return Response.AsRedirect(string.Format("/drivers/{0}",driver.Id));
            };
            #endregion

            #region Displays data in the form of the Driver according to ID
            Get["/edit/{Id}"] = x => {
                Guid driverId = Guid.Parse(x.Id);
                var driver = DocumentSession.Query<Driver>("DriverById")
                    .Where(n => n.Id == driverId).FirstOrDefault();
                if(driver == null)
                    return new NotFoundResponse();
                return View["edit", driver];
            };
            #endregion

            #region Method editing the Driver according to ID
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
            #endregion

            #region Method to delete a record according to ID
            
            Get["/delete/{Id}"] = x => {
                Guid driverId = Guid.Parse(x.Id);
                var driver = DocumentSession.Query<Driver>("DriverById")
                    .Where(n => n.Id == driverId).FirstOrDefault();
                if (driver == null)
                    return new NotFoundResponse();
                DocumentSession.Delete(driver);
                return Response.AsRedirect("/drivers");
            };
            #endregion

        }
    }
}