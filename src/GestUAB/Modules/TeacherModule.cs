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
    public class TeacherModule : BaseModule
    {
        public TeacherModule () : base("/teachers")
        {
            Get["/"] = _ => { 
                return View ["index", DocumentSession.Query<Teacher> ()
                    .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                    .ToList ()];
            };
        }
    }
}

