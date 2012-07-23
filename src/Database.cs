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
using Raven.Client;
using Raven.Client.Embedded;
using System.Linq;
using GestUAB.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System;
using System.Diagnostics;

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
        public static void PopulateUsers (this IDocumentStore ds)
        {

            using (var session = ds.OpenSession()) {
                // Operations against session
//                var firstNames = new string[] {"Kristina", "Paige", "Sherri", "Gretchen", "Karen", "Patrick", "Elsie", "Hazel",
//                                       "Malcolm", "Dolores", "Francis", "Sandy", "Marion", "Beth", "Julia", "Jerome",
//                                       "Neal", "Jean", "Kristine", "Crystal", "Alex", "Eric", "Wesley", "Franklin",
//                                       "Claire", "Marian", "Marcia", "Dwight", "Wayne", "Stephanie", "Neal",
//                                       "Gretchen", "Tim", "Jerome", "Shelley", "Priscilla", "Elsie", "Beth", "Erica",
//                                       "Douglas", "Donald", "Katherine", "Paul", "Patricia", "Lois", "Louis", "Christina",
//                                       "Darlene", "Harvey", "William", "Frederick", "Shirley", "Jason", "Judith", "Gretchen",
//                                       "Don", "Glenda", "Scott", "Pat", "Michelle", "Jessica", "Evan", "Melinda", "Calvin",
//                                       "Eugene", "Vickie", "Luis", "Allan", "Melanie", "Marianne", "Natalie", "Caroline",
//                                       "Arlene", "Kyle", "Calvin", "Gary", "Samantha", "Sara", "Stacy", "Gladys", "Mike",
//                                       "Lynne", "Faye", "Diana", "Leon", "Ethel", "Steve", "Alison", "Sherri", "Patsy",
//                                       "Kelly", "Stacy", "Curtis", "Dana", "Jennifer", "Brett", "Brandon", "Keith", "Joann"};
//
//                var lastNames = new string[]{"Chung", "Chen", "Melton", "Hill", "Puckett", "Song", "Hamilton", "Bender", "Wagner",
//                                           "McLaughlin", "McNamara", "Raynor", "Moon", "Woodard", "Desai", "Wallace",
//                                           "Lawrence", "Griffin", "Dougherty", "Powers", "May", "Steele", "Teague", "Vick",
//                                           "Gallagher", "Solomon", "Walsh", "Monroe", "Connolly", "Hawkins", "Middleton",
//                                           "Goldstein", "Watts", "Johnston", "Weeks", "Wilkerson", "Barton", "Walton", "Hall",
//                                           "Ross", "Chung", "Bender", "Woods", "Mangum", "Joseph", "Rosenthal", "Bowden",
//                                           "Barton", "Underwood", "Jones", "Baker", "Merritt", "Cross", "Cooper", "Holmes",
//                                           "Sharpe", "Morgan", "Hoyle", "Allen", "Rich", "Rich", "Grant", "Proctor", "Diaz",
//                                           "Graham", "Watkins", "Hinton", "Marsh", "Hewitt", "Branch", "Walton", "O'Brien",
//                                           "Case", "Watts", "Christensen", "Parks", "Hardin", "Lucas", "Eason", "Davidson",
//                                           "Whitehead", "Rose", "Sparks", "Moore", "Pearson", "Rodgers", "Graves",
//                                           "Scarborough", "Sutton", "Sinclair", "Bowman", "Olsen", "Love", "McLean",
//                                           "Christian", "Lamb", "James", "Chandler", "Stout"};

                var firstNames = new string[] {"Kristina", "Paige", "Sherri", "Gretchen", "Karen", "Patrick", "Elsie", "Hazel", "Malcolm", "Dolores"};

                var lastNames = new string[]{"Chung", "Chen", "Melton", "Hill", "Puckett", "Song", "Hamilton", "Bender", "Wagner",
                                             "McLaughlin"};

                var combs =
                (from first in firstNames
                from last in lastNames
                select new Tuple<string, string>(first, last)).ToList();

                combs.ForEach(x => {
                    session.Store (
                        new User{Username = x.Item1.ToLower() + "." + x.Item2.ToLower(), 
                                 FirstName = x.Item1, 
                                 LastName = x.Item2, 
                                 Email = x.Item1.ToLower() + "." +  x.Item1.ToLower() + "@gestuab.com" 
                                });

                });

//                Random rand = new Random();

//                for (int i = combs.Count - 1; i > 0; i--)
//                {
//                    int n = rand.Next(i + 1);
//                    var mem = combs[i];
//                    combs[i] = combs[n];
//                    combs[n] = mem;
//                    session.Store (
////                        new User{Username = "username", 
////                                 FirstName = "firstname", 
////                                 LastName = "lastname", 
////                                 Email = "email" 
////                                });
//                        new User{Username = mem.Item1[0].ToString() + mem.Item2, 
//                                 FirstName = mem.Item1, 
//                                 LastName = mem.Item2, 
//                                 Email = mem.Item1.ToLower() + "." +  mem.Item1.ToLower() + "@gestuab.com" 
//                                });
//                }


//                session.Store (new User{Username = "thold", FirstName = "Ton", LastName= "Hold", Email = "ton.hold@gestuab.com" });
//                session.Store (new User{Username = "jbarbosa", FirstName = "João", LastName= "Barbosa", Email = "joao.barbosa@gestuab.com"});
//                session.Store (new User{Username = "rciqueira", FirstName = "Ricardo", LastName= "Ciqueira", Email = "ricardo.ciqueira@gestuab.com"});
                // Flush those changes
                session.SaveChanges ();
            }

            ds.DatabaseCommands.PutIndex ("UsersByUsername", new IndexDefinitionBuilder<User>
            {
                Map = users => from user in users
                               select new { user.Username },
                Indexes =
                    {
                        { x => x.Username, FieldIndexing.Analyzed}
                    }
            }
            );
            

        }


    }

}




