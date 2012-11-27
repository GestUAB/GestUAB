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
    public class CarModule : BaseModule
    {
        public CarModule () : base("/cars")
        {
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Car> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };
            
            Get ["/{Id}"] = x => { 
                Guid carnumber = Guid.Parse(x.Id);
                var car = DocumentSession.Query<Car> ("CarsById")
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .Where (n => n.Id == carnumber).FirstOrDefault ();
                if (car == null)
                    return new NotFoundResponse ();
                return View ["show", car];
            };
            
            Get ["/new"] = x => {
                return View ["new", new Car ()];
            };

            Post ["/new"] = x => {
                var car = this.Bind<Car> ();
                var result = new CarValidator().Validate(car, ruleSet: "Update");
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (car);               
                return Response.AsRedirect(string.Format("/cars/{0}", car.Id));
            };
            
            Get ["/edit/{Id}"] = x => {
                Guid carnumber = Guid.Parse(x.Id);
                var car = DocumentSession.Query<Car> ("CarsById")
                    .Where (n => n.Id == carnumber).FirstOrDefault ();
                if (car == null)
                    return new NotFoundResponse ();
                return View ["edit", car];
            };

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
//                return View ["show", course];
            };

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

            Get ["/delete/{Id}"] = x => { 
                Guid carnumber = Guid.Parse(x.Id);
                var car = DocumentSession.Query<Car> ("CarsById")
                    .Where (n => n.Id == carnumber).FirstOrDefault ();
                if (car == null) 
                    return new NotFoundResponse ();
                DocumentSession.Delete (car);
                return Response.AsRedirect("/cars");
//                return View ["index", DocumentSession.Query<Course> ().ToList ()];
            };
        }
    }
}
