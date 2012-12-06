using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.ErrorHandling;
using System.IO;
using Nancy.ViewEngines;


namespace GestUAB
{
    public class Generic404ErrorHandler : IErrorHandler
    {
        private readonly IViewFactory _factory;
        private readonly IViewLocationCache _cache;
        
        public Generic404ErrorHandler(IViewFactory factory, IViewLocationCache cache)
        {
            _factory = factory;
            _cache = cache;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = _factory.RenderView(_cache, context, "views/shared/404.html");

            // RenderView sets the context.Response.StatusCode to HttpStatusCode.OK
            // so make sure to override it correctly
            response.StatusCode = HttpStatusCode.NotFound;
            context.Response = response;
        }
    }

    public class Api404ErrorHandler : IErrorHandler
    {
        private readonly IViewFactory _factory;
        private readonly IViewLocationCache _cache;

        public Api404ErrorHandler(IViewFactory factory, IViewLocationCache cache)
        {
            _factory = factory;
            _cache = cache;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = _factory.RenderView(_cache, context, "views/shared/404.html");

            // RenderView sets the context.Response.StatusCode to HttpStatusCode.OK
            // so make sure to override it correctly
            response.StatusCode = HttpStatusCode.NotFound;
            context.Response = response;
        }
    }
    
}
