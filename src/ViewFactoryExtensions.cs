using System;
using System.Linq;
using Nancy.ViewEngines;



namespace GestUAB
{
    public static class ViewFactoryExtensions
    {
        public static Nancy.Response RenderView(this IViewFactory factory, IViewLocationCache cache, Nancy.NancyContext context, string viewName, object model = null)
        {
            var foundMatchingView = cache.Any(x => viewName.Equals(string.Concat(x.Location, "/", x.Name, ".", x.Extension), StringComparison.OrdinalIgnoreCase));

            if (foundMatchingView)
            {
                var viewContext = new ViewLocationContext { Context = context };
                context.Response = factory.RenderView(viewName, model, viewContext);
            }

            return context.Response;
        }

    }
}
