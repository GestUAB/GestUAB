//
// RavenSessionProvider.cs
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

namespace GestUAB
{
    using System;
    using System.IO;
    using Raven.Client;
    using Raven.Client.Embedded;

    /// <summary>
    /// I raven session provider.
    /// </summary>
    public interface IRavenSessionProvider
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>
        /// The session.
        /// </returns>
        IDocumentSession GetSession();
    }

    /// <summary>
    /// Raven session provider.
    /// </summary>
    public class RavenSessionProvider : IRavenSessionProvider
    {
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
                DataDirectory = path
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
}