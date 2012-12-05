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
    public class ApiModule : BaseModule
    {
        public ApiModule()
            : base("/api")
        {
            Get["/teachers"] = _ => Response.AsJson(DocumentSession.Query<Teacher> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList (), HttpStatusCode.OK); 
        }
    }
}

