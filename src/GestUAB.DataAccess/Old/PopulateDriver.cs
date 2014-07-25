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
        public static void PopulateDriver(this IDocumentStore ds)
        {
            using (var session = ds.OpenSession())
            {
                #region CadastroMotorista
                // Operations against session
                session.Store(new Driver
                {
                    Name = "Motorista 1",
                    BirthDate = DateTime.Parse("01/01/2001"),
                    Cpf = "222.222.222-22",
                    Rg = "2222",
                    Phone = "(22) 2222 - 2222",
                    CellPhone = "(22) 2222 - 2222",
                    Address = "Endereço 1",
                    AddressNumber = "1",
                    Complement = "C1",
                    Cep = "2222",
                    Neighborhood = "Bairro 1",
                    Workplace = "Local 1",
                    Rotation = "Turno 1",
                    Obs = "Obs 1"
                });

                session.Store(new Driver
                {
                    Name = "Motorista 2",
                    BirthDate = DateTime.Parse("01/01/2001"),
                    Cpf = "222.222.222-22",
                    Rg = "2222",
                    Phone = "(22) 2222 - 2222",
                    CellPhone = "(22) 2222 - 2222",
                    Address = "Endereço 2",
                    AddressNumber = "2",
                    Complement = "C2",
                    Cep = "2222",
                    Neighborhood = "Bairro 2",
                    Workplace = "Local 2",
                    Rotation = "Turno 2",
                    Obs = "Obs 2"
                });
                session.Store(new Driver
                {
                    Name = "Motorista 3",
                    BirthDate = DateTime.Parse("01/01/2001"),
                    Cpf = "222.222.222-22",
                    Rg = "2222",
                    Phone = "(22) 2222 - 2222",
                    CellPhone = "(22) 2222 - 2222",
                    Address = "Endereço 3",
                    AddressNumber = "3",
                    Complement = "C3",
                    Cep = "2222",
                    Neighborhood = "Bairro 3",
                    Workplace = "Local 3",
                    Rotation = "Turno 3",
                    Obs = "Obs 3"
                });
                #endregion
                // Flush those changes
                session.SaveChanges();
            }

            ds.DatabaseCommands.PutIndex("DriverById", new IndexDefinitionBuilder<Driver>
            {
                Map = drivers => from drive in drivers
                                 select new { drive.Id },
                Indexes =
                    {
                        { x => x.Id, FieldIndexing.Analyzed}
                    }
            });

        }
    }
}