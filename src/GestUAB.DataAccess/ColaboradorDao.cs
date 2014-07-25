using System;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace GestUAB.DataAccess
{
	public class ColaboradorDao : ICrud<Guid>
	{

		public ColaboradorDao ()
		{
		}

		#region ICrud implementation
		public T Create <T>(T model) where T : class
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                c.Insert<T>(model);
            }
            return model;
		}

        public IQueryable<T> ReadAll<T> ()  where T : class
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Query<T> ("select * from Colaborador").AsQueryable();
            }
		}

        public T Read<T> (Guid id)  where T : class
		{
			using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Get<T>(id);
			}
		}

        public bool Update<T> (T entity)  where T : class
        {
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
               return c.Update<T>(entity);
            }
        }

        public bool Update<T> (T oldT, T newT)  where T : class
        {
            throw new NotImplementedException ();
        }

        public bool Update<T> (Guid id, T newT)  where T : class
		{
			throw new NotImplementedException ();
		}

		public bool Delete (Guid id)
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Execute ("delete from Colaborador where Id = @id", new {Id = id}) > 0;
            }
		}
        public bool Delete<T> (T model)  where T : class
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Delete (model);
            }
		}

		public bool NomeExists(string nome) {
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return Dapper.SqlMapper.Query (c, "select * from Colaborador where Nome = @nome", new {Nome = nome}).Count() > 0;
            }
		}

		public bool CpfExists (string cpf)
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return Dapper.SqlMapper.Query (c, "select * from Colaborador where Cpf = @cpf", new {Cpf = cpf}).Count() > 0;
            }
        }
		#endregion
	}
}

