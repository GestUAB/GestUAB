using System;
using System.Configuration;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using System.Data;

namespace GestUAB.DataAccess
{
	public class Database
	{
		static string  _connectionString = ConfigurationManager.ConnectionStrings ["gestuab"].ConnectionString;
		static OrmLiteConnectionFactory _dbFactory = new OrmLiteConnectionFactory (_connectionString, SqliteDialect.Provider);


		public Database ()
		{
		}

		public static IDbConnection OpenConnection() {
			return _dbFactory.OpenDbConnection ();
		}

		public static string ConnectionString {
			get {
				return _connectionString;
			}
		}
	}
}

