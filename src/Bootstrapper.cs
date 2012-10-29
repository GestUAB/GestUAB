// 
// Bootstrapper.cs
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
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;

namespace GestUAB
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
//        const int CACHE_SECONDS = 30;
//        readonly Dictionary<string, Tuple<DateTime, Response, int>> cachedResponses = new Dictionary<string, Tuple<DateTime, Response, int>> ();

        protected override NancyInternalConfiguration InternalConfiguration {
            get {   
                return NancyInternalConfiguration.WithOverrides (x => x.NancyModuleBuilder = typeof(RavenModuleBuilder)); 
            }
        }

        protected override void ApplicationStartup (TinyIoC.TinyIoCContainer container, IPipelines pipelines)
        {
#if (DEBUG)
            base.ApplicationStartup (container, pipelines);
            StaticConfiguration.DisableErrorTraces = false;
            StaticConfiguration.DisableCaches = true;
#endif
        }

        protected override Nancy.Diagnostics.DiagnosticsConfiguration DiagnosticsConfiguration {
            get {
                return new DiagnosticsConfiguration { Password = @"teste"};
            }
        }

//        protected override void ConfigureConventions (NancyConventions nancyConventions)
//        {
////            nancyConventions.StaticContentsConventions.Add (StaticContentConventionBuilder.AddDirectory (
////                "styles", @"content\css", "css"
////            ));
//            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("content", "Assets"));
//        }

//        protected override void ApplicationStartup (TinyIoC.TinyIoCContainer container, IPipelines pipelines)
//        {
//            base.ApplicationStartup (container, pipelines);
//            StaticConfiguration.DisableCaches  = false;
//            pipelines.BeforeRequest += CheckCache;
//
//            pipelines.AfterRequest += SetCache;
//        }
//
//        /// <summary>
//        /// Check to see if we have a cache entry - if we do, see if it has expired or not,
//        /// if it hasn't then return it, otherwise return null;
//        /// </summary>
//        /// <param name="context">Current context</param>
//        /// <returns>Request or null</returns>
//        public Response CheckCache (NancyContext context)
//        {
//            Tuple<DateTime, Response, int> cacheEntry;
//
//            if (this.cachedResponses.TryGetValue (
//                context.Request.Path,
//                out cacheEntry
//            )) {
//                if (cacheEntry.Item3 == int.MaxValue) {
//                    return cacheEntry.Item2;
//                }
//                else if (cacheEntry.Item1.AddSeconds (cacheEntry.Item3) > DateTime.Now) {
//                    return cacheEntry.Item2;
//                }
//            }
//
//            return null;
//        }
//
//        /// <summary>
//        /// Adds the current response to the cache if required
//        /// Only stores by Path and stores the response in a dictionary.
//        /// Do not use this as an actual cache :-)
//        /// </summary>
//        /// <param name="context">Current context</param>
//        public void SetCache (NancyContext context)
//        {
//            if (context.Response.StatusCode != HttpStatusCode.OK) {
//                return;
//            }
//
//            object cacheSecondsObject;
//            if (!context.Items.TryGetValue (
//                ContextExtensions.OUTPUT_CACHE_TIME_KEY,
//                out cacheSecondsObject
//            )) {
//                return;
//            }
//
//            int cacheSeconds;
//            if (!int.TryParse (
//                cacheSecondsObject.ToString (),
//                out cacheSeconds
//            )) {       
//                return;
//            }
//
//            var cachedResponse = new CachedResponse (context.Response);
//
//            this.cachedResponses [context.Request.Path] = new Tuple<DateTime, Response, int> (
//                DateTime.Now,
//                cachedResponse,
//                cacheSeconds
//            );
//
//            context.Response = cachedResponse;
//        }
    }
}

