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
            using (var session = ds.OpenSession()) {
                                
                var firstNames = new string[] {
                     "Joao",
                     "Mario",
                     "Jose"
                 };
                    
                 foreach(string name in firstNames){
                    session.Store(new Teacher{Name = name});
                 }
                
                session.SaveChanges();
                
                ds.DatabaseCommands.PutIndex ("TeachersByName", new IndexDefinitionBuilder<Teacher>
                {
                     Map = teachers => from teacher in teachers
                                    select new { teacher.Name },
                     Indexes =
                     {
                         { x => x.Name, FieldIndexing.Analyzed}
                     }
                });
               
            }
        }
    }
}

