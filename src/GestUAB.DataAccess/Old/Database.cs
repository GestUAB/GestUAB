//
// Database.cs
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
using System;
using Raven.Client;
using System.IO;
using Raven.Client.Embedded;
using System.Reflection;
using System.Linq;

namespace GestUAB.DataAccess
{
    public class Database
    {
        public Database()
        {
        }

        /// <summary>
        /// The _document store.
        /// </summary>
        private static IDocumentStore _documentStore;

        /// <summary>
        /// Gets the document store.
        /// </summary>
        /// <value>
        /// The document store.
        /// </value>
        public static IDocumentStore DocumentStore
        {
            get { return _documentStore ?? (_documentStore = CreateDocumentStore()); }
        }

        /// <summary>
        /// Creates the document store.
        /// </summary>
        /// <returns>
        /// The document store.
        /// </returns>
        private static IDocumentStore CreateDocumentStore()
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            path = Path.Combine(path, Conventions.RavenDataDirectory);
            var populate = !Directory.Exists(path);

            var documentStore = new EmbeddableDocumentStore() 
            {
                EnlistInDistributedTransactions = false,
                DataDirectory = path,//"~\\App_Data\\Database",
                UseEmbeddedHttpServer = false //, 
                //Configuration = { Port = 8887 }
            }.Initialize();

            if (populate)
            {
                documentStore.PopulateAll();
            }

            return documentStore;
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>
        /// The session.
        /// </returns>
        public IDocumentSession GetSession()
        {
            return DocumentStore.OpenSession();
        }
    }

    public static partial class PopulateDatabaseExtensions
    {
        public static void PopulateAll (this IDocumentStore ds)
        {
            var methodInfos = typeof(IDocumentStore).GetExtensionMethods(typeof(PopulateDatabaseExtensions).Assembly).ToList();
            
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

