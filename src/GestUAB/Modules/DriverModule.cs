//
// Driver.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2012 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
namespace GestUAB.Modules
{
    using System;
    using Nancy;
    using GestUAB.Models;
    using Raven.Client.Linq;
    using System.Linq;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using FluentValidation;
    using Nancy.Responses;

    /// <summary>
    /// Driver Module
    /// </summary>
    public class DriverModule : BaseModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverModule" /> class.
        /// </summary>
        public DriverModule () : base("/drivers")
        {
            #region Method that returns the index View Course, with the registered Drivers
            Get ["/"] = _ => 
            {
                return View ["index", DocumentSession.Query<Driver> ()
                        .Customize (q => q.WaitForNonStaleResultsAsOfLastWrite ())
                        .ToList ()];
            };
            #endregion

            #region Method that returns a View Show, displaying the driver in the form according to the ID.
            Get ["/{Id}"] = x => 
            {
                Guid driverId = Guid.Parse (x.Id);
                var driver = DocumentSession.Query<Driver> ("DriverById")
                    .Customize (q => q.WaitForNonStaleResultsAsOfLastWrite ())
                    .Where (n => n.Id == driverId).FirstOrDefault ();
                if (driver == null) 
                {
                    return new NotFoundResponse ();
                }
                return View ["show", driver];
            };
            #endregion

            #region  Method that returns the New View, creating a default driver
            Get ["/new"] = x => 
            {
                return View ["new", Driver.DefaultDriver ()];
            };
            #endregion

            #region Method that creates and validates a new driver in accordance with the specifications of the class DriverValidator
            Post ["/new"] = x => 
            {
                var driver = this.Bind<Driver> ();
                var result = new DriverValidator ().Validate (driver);
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (driver);
                return Response.AsRedirect (string.Format ("/drivers/{0}", driver.Id));
            };
            #endregion

            #region Displays data in the form of the Driver according to ID
            Get ["/edit/{Id}"] = x => 
            {
                Guid driverId = Guid.Parse (x.Id);
                var driver = DocumentSession.Query<Driver> ("DriverById")
                    .Where (n => n.Id == driverId).FirstOrDefault ();
                if (driver == null)
                    return new NotFoundResponse ();
                return View ["edit", driver];
            };
            #endregion

            #region Method editing the Driver according to ID
            Post ["/edit/{Id}"] = x => {
                var driver = this.Bind<Driver> ();
                var result = new DriverValidator ().Validate (driver, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                Guid driverId = Guid.Parse (x.Id);
                var saved = DocumentSession.Query<Driver> ("DriverById")
                    .Where (n => n.Id == driverId).FirstOrDefault ();
                if (saved == null)
                    return new NotFoundResponse ();
                saved.Fill (driver);
                return Response.AsRedirect (string.Format ("/drivers/{0}", driver.Id));
            };
            #endregion

            #region Method to delete a record according to ID
            
            Get ["/delete/{Id}"] = x => {
                Guid driverId = Guid.Parse (x.Id);
                var driver = DocumentSession.Query<Driver> ("DriverById")
                    .Where (n => n.Id == driverId).FirstOrDefault ();
                if (driver == null)
                    return new NotFoundResponse ();
                DocumentSession.Delete (driver);
                return Response.AsRedirect ("/drivers");
            };
            #endregion

        }
    }
}