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
        public static void PopulatePedidoDiaria (this IDocumentStore ds)
        {
//            using (var session = ds.OpenSession()) {
//
//                var departamentos = session.Query<Departamento> ("DepartamentosByNome").ToList();  
//
//                // Operations against session
//                session.Store (new PedidoDiaria{
//                    Observacao = "First Travel Test",
//                    Destino = "Neverland",
//                    Saida = DateTime.Now,
//                    Retorno = DateTime.Now.AddDays(10),
//                    Requerente = departamentos[0]
//                });
//                session.Store (new PedidoDiaria{
//                    Observacao = "Second Travel Test",
//                    Destino = "Neverland",
//                    Saida = DateTime.Now,
//                    Retorno = DateTime.Now.AddDays(10),
//                    Requerente = departamentos[1]
//                });
//                session.SaveChanges ();
//            }
        }
    }
}
