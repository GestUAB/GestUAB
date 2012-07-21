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
        public static void PopulateUsers (this IDocumentStore ds)
        {
            using (var session = ds.OpenSession()) {
                // Operations against session
                session.Store (new User{Username = "thild", FirstName = "Tony", LastName= "Hild"});
                session.Store (new User{Username = "jbarbosa", FirstName = "João", LastName= "Barbosa"});
                session.Store (new User{Username = "rciqueira", FirstName = "Ricardo", LastName= "Ciqueira"});
                // Flush those changes
                session.SaveChanges ();
            }

            ds.DatabaseCommands.PutIndex("UsersByUsername", new IndexDefinitionBuilder<User>
            {
                Map = users => from user in users
                               select new { user.Username },
                Indexes =
                    {
                        { x => x.Username, FieldIndexing.Analyzed}
                    }
            });
        }
	}
}

