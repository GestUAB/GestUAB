using Raven.Client;
using Raven.Client.Embedded;
using System.Linq;
using GestUAB.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace GestUAB
{
	public static partial class PopulateDatabaseExtensions
	{
		public static void PopulateCourses (this IDocumentStore ds)
        {
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
		}
	}
}
