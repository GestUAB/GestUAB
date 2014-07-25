//
// DataFacade.cs
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
    using GestUAB.Models;
    using System;
    using System.Linq;

    public class DataFacade
    {

        public DataFacade()
        {
            
        }
        #region Colaborador
        public IQueryable<T> ReadAllColaboradores<T>()  where T : class
        {
            return new ColaboradorDao().ReadAll<T>();
        }

        public T ReadColaborador<T>(Guid id)  where T : class
        {
            return new ColaboradorDao().Read<T>(id);
        }

        public bool CpfExistsColaborador(string cpf)
        {
            return new ColaboradorDao().CpfExists(cpf);
        }

        public T CreateColaborador<T>(T obj) where T : class
        {
            return new ColaboradorDao().Create(obj);
        }

        public bool UpdateColaborador<T>(T obj) where T : class
        {
            return new ColaboradorDao().Update(obj);
        }

        public bool DeleteColaborador(Guid id)
        {
            return new ColaboradorDao().Delete(id);
        }
        #endregion

        #region Curso
        public IQueryable<Curso> ReadAllCursos()
        {
            return new CursoDao().ReadAll();
        }

        public Curso ReadCurso<T>(Guid id)
        {
            return new CursoDao().Read(id);
        }

//        public T CreateCurso<T>(T obj) where T : class
//        {
//            return new CursoDao().Create(obj);
//        }

        public void CreateCurso(Curso obj)
        {
            var dao = new CursoDao();
            dao.Create(obj);
        }

        public bool UpdateCurso(Curso obj)
        {
            return new ColaboradorDao().Update(obj);
        }

        public bool DeleteCurso(Guid id)
        {
            return new ColaboradorDao().Delete(id);
        }
        #endregion

        #region Setor
        public IQueryable<T> ReadAllSetores<T>()  where T : class
        {
            return new SetorDao().ReadAll<T>();
        }

        public T ReadSetor<T>(Guid id)  where T : class
        {
            return new SetorDao().Read<T>(id);
        }

        public T CreateSetor<T>(T obj) where T : class
        {
            return new SetorDao().Create(obj);
        }

        public bool UpdateSetor<T>(T obj) where T : class
        {
            return new SetorDao().Update(obj);
        }

        public bool DeleteSetor(Guid id)
        {
            return new SetorDao().Delete(id);
        }
        #endregion
    }
}

