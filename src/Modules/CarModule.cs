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
    /// Car Module
    /// </summary>
    /// 
    public class CarModule : BaseModule
    {

        /// <summary>
        /// Builder CarModule Class
        /// </summary>
        /// 
        public CarModule () : base("/cars")
        {
            #region Method that returns the index View Car, with the registered Cars
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Car> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };
            #endregion

            #region This method shows the data about a specified car
            Get ["/{Id}"] = x => { 
                Guid carnumber = Guid.Parse(x.Id);
                var car = DocumentSession.Query<Car> ("CarsById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where (n => n.Id == carnumber).FirstOrDefault ();
                if (car == null)
                    return new NotFoundResponse ();
                return View ["show", car];
            };
            #endregion

            #region This method returns a New View to create a Default Car
            Get ["/new"] = x => {
                return View ["new", Car.DefaultCar() ];
            };
            #endregion

            #region This method creates a new Car
            Post ["/new"] = x => {
                var car = this.Bind<Car> ();
                var result = new CarValidator().Validate(car, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (car);               
                return Response.AsRedirect(string.Format("/cars/{0}", car.Id));
            };
            #endregion

            #region Method that shows a view to edit a car
            Get ["/edit/{Id}"] = x => {
                Guid carnumber = Guid.Parse(x.Id);
                var car = DocumentSession.Query<Car> ("CarsById")
                    .Where (n => n.Id == carnumber).FirstOrDefault ();
                if (car == null)
                    return new NotFoundResponse ();
                return View ["edit", car];
            };
            #endregion

            #region Method that edits a Car by its ID
            Post ["/edit/{Id}"] = x => {
                var car = this.Bind<Car> ();
                var result = new CarValidator().Validate (car, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                Guid carnumber = Guid.Parse(x.Id);
                var saved = DocumentSession.Query<Car> ("CarsById")
                    .Where (n => n.Id == carnumber).FirstOrDefault ();
                if (saved == null) 
                    return new NotFoundResponse ();
                saved.Fill (car);
                return Response.AsRedirect(string.Format("/cars/{0}", car.Id));               
            };
            #endregion

            #region This method deletes a car by its ID
            Delete ["/delete/{Id}"] = x => { 
                Guid carnumber = Guid.Parse(x.Id);
                var car = DocumentSession.Query<Car> ("CarsById")
                        .Where (n => n.Id == carnumber)
                        .FirstOrDefault ();
                if (car == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (car);
                var resp = new JsonResponse<Car> (
                        car,
                        new DefaultJsonSerializer ()
                );
                resp.StatusCode = HttpStatusCode.OK;
                return resp;

            };
            #endregion

            #region This method deletes a car by its ID
            Get ["/delete/{Id}"] = x => { 
                Guid carnumber = Guid.Parse(x.Id);
                var car = DocumentSession.Query<Car> ("CarsById")
                    .Where (n => n.Id == carnumber).FirstOrDefault ();
                if (car == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (car);
                return Response.AsRedirect("/cars");
            };
            #endregion
        }
    }
}
