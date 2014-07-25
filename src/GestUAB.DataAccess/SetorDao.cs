//
// SetorDao.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2013 Tony Alexander Hild
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


namespace GestUAB.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dapper;
    using Dapper.Contrib.Extensions;

	public class SetorDao : ICrud<Guid>
	{

		public SetorDao ()
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
                return c.Query<T> ("select * from Setor").AsQueryable();
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
                return c.Execute ("delete from Setor where Id = @id", new {Id = id}) > 0;
            }
		}
        public bool Delete<T> (T model)  where T : class
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Delete (model);
            }
		}

		#endregion
	}
}

