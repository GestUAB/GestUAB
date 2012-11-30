using Raven.Client;
using Raven.Client.Embedded;
using System.Linq;
using GestUAB.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System;

namespace GestUAB
{
    public static partial class PopulateDatabaseExtensions
    {
        public static void PopulateDesistence(this IDocumentStore ds)
        {
            using (var session = ds.OpenSession())
            {
                session.Store(new Desistence() { NameTeacher = "Professor 1",
                    Destiny = "Destino 1", ReasonGiveup = "Desistencia 1",
                    TravelDate = DateTime.Now.ToString()
                });

                session.Store(new Desistence()
                {
                    NameTeacher = "Professor 2",
                    Destiny = "Destino 2",
                    ReasonGiveup = "Desistencia 2",
                    TravelDate = DateTime.Now.ToString()
                });

                session.Store(new Desistence()
                {
                    NameTeacher = "Professor 3",
                    Destiny = "Destino 3",
                    ReasonGiveup = "Desistencia 3",
                    TravelDate = DateTime.Now.ToString()
                });

                session.SaveChanges();

            }

            ds.DatabaseCommands.PutIndex("DesistenceById", new IndexDefinitionBuilder<Desistence> 
            {
                Map = desistences => from desistence in desistences
                                 select new { desistence.Id },
                Indexes =
                    {
                        { x => x.Id, FieldIndexing.Analyzed}
                    }
            });

        }
    }
}