using System;
using Raven.Client;
using Raven.Client.Embedded;
using System.Linq;
using GestUAB.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace GestUAB
{
    public static partial class PopulateDatabaseExtensions
    {
        public static void PopulateScholarship (this IDocumentStore ds)
        {
            using (var session = ds.OpenSession()) {
                var function = session.Query<Function> ("FunctionByName")
                    .Where (x => x.Name == "Tutor a distancia").FirstOrDefault();
              //  var course = session.Query<Course> ("CourseByName")
                    //.Where (x => x.Name == "Geografia").FirstOrDefault();
                // Operations against session
               session.Store(new Scholarship() {CPF = "075.113.070-40", Name = "Zezao", Function = function.Name, Lots =  "50", State = "ES", Value = "R$ 1.000,00"});
                // Flush those changes
                session.SaveChanges ();
            }
                        
            ds.DatabaseCommands.PutIndex("ScholarshipById", new IndexDefinitionBuilder<Scholarship>
                                {
                Map = scholarships => from scholarship in scholarships
                select new { scholarship.Id},
                Indexes =
                {
                    { x => x.Id, FieldIndexing.Analyzed}
                }      
            });

            ds.DatabaseCommands.PutIndex("ScholarshipByName", new IndexDefinitionBuilder<Scholarship>
                                         {
                Map = scholarships => from scholarship in scholarships
                select new { scholarship.Name},
                Indexes =
                {
                    { x => x.Id, FieldIndexing.Analyzed}
                }
            });

            ds.DatabaseCommands.PutIndex("ScholarshipByFunction", new IndexDefinitionBuilder<Scholarship>
                                         {
                Map = scholarships => from scholarship in scholarships
                select new { scholarship.Function},
                Indexes =
                {
                    { x => x.Id, FieldIndexing.Analyzed}
                }
            });

            #region populate function
            using (var session = ds.OpenSession()) {
                // Operations against session
                session.Store (new Function {Name = "Tutor a distancia"});
                session.Store (new Function {Name = "Tutor presencial"});
                session.Store (new Function {Name = "Professor Pesquisador"});
                // Flush those changes
                session.SaveChanges ();
            }
            
            ds.DatabaseCommands.PutIndex("FunctionpById", new IndexDefinitionBuilder<Function>
                                         {
                Map = functions => from function in functions
                select new { function.Id },
                Indexes =
                {
                    { x => x.Id, FieldIndexing.Analyzed}
                }
            });
            
            ds.DatabaseCommands.PutIndex("FunctionByName", new IndexDefinitionBuilder<Function>
                                         {
                Map = functions => from function in functions
                select new { function.Name },
                Indexes =
                {
                    { x => x.Name, FieldIndexing.Analyzed}
                }
            });
            #endregion

            /**
            #region populate course
            using (var session = ds.OpenSession()) {
                // Operations against session
                session.Store (new Course{Name = "Ciência da computação"});
                session.Store (new Course{Name = "Matemática"});
                session.Store (new Course{Name = "Geografia"});
                // Flush those changes
                session.SaveChanges ();
            }
            
            ds.DatabaseCommands.PutIndex("CoursesById", new IndexDefinitionBuilder<Course>
                                         {
                Map = courses => from course in courses
                select new { course.Id },
                Indexes =
                {
                    { x => x.Id, FieldIndexing.Analyzed}
                }
            });
            
            ds.DatabaseCommands.PutIndex("CoursesByName", new IndexDefinitionBuilder<Course>
                                         {
                Map = courses => from course in courses
                select new { course.Name },
                Indexes =
                {
                    { x => x.Name, FieldIndexing.Analyzed}
                }
            });
            #endregion
            */
        }
    }
}

