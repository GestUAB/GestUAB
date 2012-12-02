using System;
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
        public static void PopulateTeachers (this IDocumentStore ds)
        {
            using (var session = ds.OpenSession())
            {

                //                var firstNames = new string[] {
                //                     "Joao",
                //                     "Mario",
                //                     "Jose"
                //                 };
                //                
                //                var courses = new string[]{
                //                    "Administração",
                //                    "Ciência da Computação",
                //                    "Artes"
                //                };
                //                
                //                 int i = 0;
                //                 foreach(string name in firstNames){
                //                    Course course = new Course();
                //                    course.Name = courses[i];
                //                    course.Active = true;
                //                    session.Store(new Teacher{Name = name,Course = course});
                //                    i++;
                //                }

                Course course = new Course();
                course.Name = "Administração";
                course.Active = true;
                session.Store(new Teacher { Name = "Joao", Course = course });
                session.SaveChanges();
            }

            ds.DatabaseCommands.PutIndex("TeachersByName", new IndexDefinitionBuilder<Teacher>
            {
                Map = teachers => from teacher in teachers
                                  select new { teacher.Name },
                Indexes =
                     {
                         { x => x.Name, FieldIndexing.Analyzed}
                     }
            });

            ds.DatabaseCommands.PutIndex("TeachersById", new IndexDefinitionBuilder<Teacher>
            {
                Map = teachers => from teacher in teachers
                                  select new { teacher.Id },
                Indexes =
                     {
                         { x => x.Id, FieldIndexing.Analyzed}
                     }
            });     
                          
        }
    }
}

