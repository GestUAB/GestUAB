// 
// RavenModuleBuilder.cs
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
using Nancy.ModelBinding;
using Nancy.Routing;
using Nancy.Validation;
using Nancy.ViewEngines;
using Raven.Client;
using Nancy.Responses.Negotiation;
using Nancy.Extensions;

namespace GestUAB
{
    public class RavenModuleBuilder : INancyModuleBuilder
    {
        private readonly IViewFactory viewFactory;
        private readonly IResponseFormatterFactory responseFormatterFactory;
        private readonly IModelBinderLocator modelBinderLocator;
        private readonly IModelValidatorLocator validatorLocator;
        private readonly IRavenSessionProvider ravenSessionProvider;

        public RavenModuleBuilder (IViewFactory viewFactory, 
                              IResponseFormatterFactory responseFormatterFactory, 
                              IModelBinderLocator modelBinderLocator, 
                              IModelValidatorLocator validatorLocator, 
                              IRavenSessionProvider ravenSessionProvider)
        {
            this.viewFactory = viewFactory;
            this.responseFormatterFactory = responseFormatterFactory;
            this.modelBinderLocator = modelBinderLocator;
            this.validatorLocator = validatorLocator;
            this.ravenSessionProvider = ravenSessionProvider;
        }

        public NancyModule BuildModule (NancyModule module, NancyContext context)
        {
            CreateNegotiationContext(module, context);
            module.Context = context;
            module.Response = this.responseFormatterFactory.Create (context);
            module.ViewFactory = this.viewFactory;
            module.ModelBinderLocator = this.modelBinderLocator;
            module.ValidatorLocator = this.validatorLocator;

            context.Items.Add (Conventions.RavenSession, ravenSessionProvider.GetSession ());
            module.After.AddItemToStartOfPipeline (ctx =>
            {
                var session = ctx.Items [Conventions.RavenSession] as IDocumentSession;
                session.SaveChanges ();
                session.Dispose ();
            }
            );
            return module;
        }

        private static void CreateNegotiationContext (NancyModule module, NancyContext context)
        {
            // TODO - not sure if this should be here or not, but it'll do for now :)
            context.NegotiationContext = new NegotiationContext
                                             {
                                                 ModuleName = module.GetModuleName(),
                                                 ModulePath = module.ModulePath,
                                             };
        }
    }
}

