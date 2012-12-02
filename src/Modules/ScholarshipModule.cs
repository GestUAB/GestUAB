using System;
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

            Get ["/new"] = x => {
                return View ["new", Scholarship.DefaultScholarship()];
            };

            Get ["/craw"] = x => { 
                string url = "../../GestUAB/test_data/101752.html"; // x.Url
                string htmlString = DataCrawler.GetTestText(url);
                foreach (Scholarship scholarship in DataCrawler.CrawData(htmlString))
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

