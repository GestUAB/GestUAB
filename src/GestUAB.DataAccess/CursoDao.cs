//
// CursoDao.cs
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
using GestUAB.Models;


namespace GestUAB.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using Dapper;
    using Dapper.Contrib.Extensions;

	public class CursoDao// : ICrud<Guid>
	{

		public CursoDao ()
		{
		}

		#region ICrud implementation
//		public T Create <T>(T model) where T : class
//		{
//            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
//                c.Insert<T>(model);
//            }
//            return model;
//		}

        public void Create (Curso model) 
        {
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                c.Open();
                var m = model.ToDynamic();
                m.SetorId = model.Setor.Id;
                (m as IDictionary<string, object>).Remove("Setor");
                c.Insert((object)m, "Curso");
            }
        }

        public IQueryable<Curso> ReadAll ()
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Query<Curso> ("select * from Curso").AsQueryable();
            }
		}

        public Curso Read (Guid id)
		{
			using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                var sql = @"select * from Curso c left join Setor s on s.Id = c.SetorId where c.Id = @Id";
                var data = c.Query<Curso, Setor, Curso>(sql, 
                                                        (curso, setor) => { 
                                                            curso.Setor = setor; 
                                                            return curso;
                                                        }, 
                                                        new {Id = id});
                return data.First();
			}
		}

        public bool Update (Curso entity)
        {
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
               return c.Update<Curso>(entity);
            }
        }

        public bool Update (Curso oldT, Curso newT)
        {
            throw new NotImplementedException ();
        }

        public bool Update (Guid id, Curso newT)
		{
			throw new NotImplementedException ();
		}

		public bool Delete (Guid id)
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Execute ("delete from Curso where Id = @id", new {Id = id}) > 0;
            }
		}
        public bool Delete (Curso model)
		{
            using (var c = new Mono.Data.Sqlite.SqliteConnection(Database.ConnectionString)) {
                return c.Delete (model);
            }
		}

		#endregion
	}
}

