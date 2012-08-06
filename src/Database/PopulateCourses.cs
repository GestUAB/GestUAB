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
                session.Store (new Course{Number = 1, Name = "Ciência da computação"});
                session.Store (new Course{Number = 2, Name = "Matemática"});
                session.Store (new Course{Number = 3, Name = "Geografia"});
                // Flush those changes
                session.SaveChanges ();
            }

            ds.DatabaseCommands.PutIndex("CoursesByNumber", new IndexDefinitionBuilder<Course>
            {
                Map = courses => from course in courses
                               select new { course.Number },
                Indexes =
                    {
                        { x => x.Number, FieldIndexing.Analyzed}
                    }
            });
		}
	}
}
