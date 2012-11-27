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
		public static void PopulateCars (this IDocumentStore ds)
        {
            using (var session = ds.OpenSession()) {
                
                session.Store(new Car{Name = "Gol", Year = "2002", Chassi = "shdaud7as6d7a9", Plate = "AAA 0000"});
                session.Store(new Car{Name = "Celta", Year = "2012", Chassi = "shdaud7as6d7a9", Plate = "ABA 2300"});
                session.Store(new Car{Name = "Palio", Year = "2006", Chassi = "shdaud7as6d7a9", Plate = "ABC 2340"});
           
                session.SaveChanges ();
            }

            ds.DatabaseCommands.PutIndex("CarsById", new IndexDefinitionBuilder<Car>
            {
                Map = cars => from car in cars
                               select new { car.Id },
                Indexes =
                    {
                        { x => x.Id, FieldIndexing.Analyzed}
                    }
            });
		}
	}
}
