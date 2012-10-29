// 
// Store.cs
//  
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
// 
// Copyright (c) 2012 
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
using System.Linq;
using GestUAB.Models;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using System;
using System.Reflection;

namespace GestUAB
{
//    public class Database
//    {
//        static Database instance;
//        EmbeddableDocumentStore store;
//        IDocumentSession session;
//        static object syncLock = new object ();
//
//        public static Database Instance {
//            get {
//                if (instance == null) 
//                    lock (syncLock) {
//                        instance = new Database ();
//                    }
//                return instance;
//            }
//        }
//
//        public void Initialize (string path)
//        {
//            store = new EmbeddableDocumentStore  {  DataDirectory = path  };
//            store.Initialize ();
//        }
//
//        public EmbeddableDocumentStore Store {
//            get {
//                return store;
//            }
//        }
//
//        public IDocumentSession Session {
//            get {
//                return session;
//            }
//            set {
//                session = value;
//            }
//        }
//    }
//
//    public  static partial class DatabaseExtensions
//    {
//
//
//        public static T Create<T> (this Database db, T model) where T:IModel
//        {
//            using (var session = Database.Instance.Store.OpenSession()) {
//                session.Store (model);
//                session.SaveChanges ();
//            }
//            return model;
//        }
//        
//        public static IQueryable<T> Read<T> (this Database db) where T:IModel
//        {
//            using (var session = Database.Instance.Store.OpenSession()) {
//                return session.Query<T> ().AsQueryable ();
//            }
//        }
//
//        public static IQueryable<T> Read<T> (this Database db, string index) where T:IModel
//        {
//            using (var session = Database.Instance.Store.OpenSession()) {
//                return session.Query<T> (index).AsQueryable ();
//            }
//        }
//
//
//        public static void Update<T> (this Database db, T model) where T:IModel
//        {
//            using (var session = Database.Instance.Store.OpenSession()) {
//                session.SaveChanges ();
//            }
//        }
//
//        public static void OpenSession (this Database db) {
//            Database.Instance.Session = Database.Instance.Store.OpenSession();
//        }
//
//        public static void CloseSession (this Database db) {
//            Database.Instance.Session.Dispose();
//        }
//
//        public static void Delete<T> (this Database db, T model) where T:IModel
//        {
//            using (var session = Database.Instance.Store.OpenSession()) {
//                session.Delete (model);
//                session.SaveChanges ();
//            }
//        }
//    }

    public static partial class PopulateDatabaseExtensions
    {
		public static void PopulateAll (this IDocumentStore ds)
		{
            MethodInfo[] methodInfos = typeof(PopulateDatabaseExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.Name.Contains("Populate") && !methodInfo.Name.Contains("PopulateAll"))
                {
                    Console.Write("Executing " + methodInfo.Name);
                    object[] parametersArray = new object[] { ds };

                    methodInfo.Invoke(ds, parametersArray);
                }
            }
		}
	}

}
