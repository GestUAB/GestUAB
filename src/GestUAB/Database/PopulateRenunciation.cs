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
        public static void PopulateRenunciation(this IDocumentStore ds)
        {
            ds.DatabaseCommands.PutIndex("RenunciationById", new IndexDefinitionBuilder<Renunciation>
            {
                Map = renunciations => from renunciation in renunciations
                                       select new { renunciation.Id },
                Indexes =
                    {
                        { x => x.Id, FieldIndexing.Analyzed}
                    }
            });
        }
    }
}
