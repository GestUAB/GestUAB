using System;
using System.Collections.Generic;
using Nancy;
using GestUAB.Models;
using Raven.Client.Linq;
using System.Linq;
using Nancy.ModelBinding;
using Nancy.Validation;
using FluentValidation;
using Nancy.Responses;
using GestUAB;

namespace GestUAB.Modules
{
    public class ScholarshipModule : BaseModule
    {
        public ScholarshipModule () : base("/scholarships")
        {
            Get ["/"] = _ => { 
                return View ["index", DocumentSession.Query<Scholarship> ()
                             .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                             .ToList ()];
            };
            
            Get ["/{Id}"] = x => { 
                Guid scholarshipId = Guid.Parse(x.Id);
                var scholarship = DocumentSession.Query<Scholarship> ("ScholarshipById")
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .Where (n => n.Id == scholarshipId).FirstOrDefault ();
                if (scholarship == null)
                return new NotFoundResponse ();

                return View ["show", scholarship];
            };

            Get ["/{Name}"] = x => { 
                string scholarshipName = x.Name;
                var scholarship = DocumentSession.Query<Scholarship> ("ScholarshipByName")
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .Where (n => n.Name == scholarshipName);
                if (scholarship == null)
                return new NotFoundResponse ();
                
                return View ["show", scholarship];
            };

            Get ["/{Function}"] = x => { 
                string scholarshipFunction = x.Function;
                var scholarship = DocumentSession.Query<Scholarship> ("ScholarshipByFunction")
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .Where (n => n.Name == scholarshipFunction);
                if (scholarship == null)
                return new NotFoundResponse ();
                
                return View ["show", scholarship];
            };

            Get ["/new"] = x => {
                return View ["new", Scholarship.DefaultScholarship()];
            };

            Get ["/craw"] = x => { 
                string url = "../../GestUAB/test_data/101752.html"; // x.Url
                string htmlString = DataCrawler.GetTestText(url);
                List<Scholarship> scholarships = DataCrawler.CrawData(htmlString);
                //ScholarshipsCounts c = new ScholarshipsCounts();
                //c.CountFunction(scholarships,"TUTOR A DISTÂNCIA");
                //c.Count = scholarships.Count; 
                foreach (Scholarship scholarship in scholarships)
                {
                    // Cadastrar no banco
                    DocumentSession.Store(scholarship);
                }
                // redirect index 
                return Response.AsRedirect("/scholarships");
               
                //if (scholarship == null)
                //return new NotFoundResponse ();
                
                //return View ["show", scholarship];
            };

            Post ["/new"] = x => {
                var scholarship = this.Bind<Scholarship> ();
                var result = new ScholarshipValidator ().Validate (scholarship);
                if (!result.IsValid)
                    return View ["Shared/_errors", result];
                DocumentSession.Store (scholarship);
                return Response.AsRedirect(string.Format("/scholarships/{0}", scholarship.Id));
            };
        }
    }
}

