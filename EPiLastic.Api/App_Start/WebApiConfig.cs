using EpiLastic.Querying;
using EpiLastic.Services;
using EPiLastic.Api.Services;
using EPiLastic.Api.StructureMap;
using StructureMap;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace EPiLastic.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new Container();
            container.Configure(c => c.For<ISearchResponseMapper>().Use<SearchResponseMapper>());
            container.Configure(c => c.For<ISearchClient>().Use<SearchClient>());
            container.Configure(c => c.For<ILoggerWrapper>().Use<LoggerWrapper>());            

            config.Services.Replace(typeof(IHttpControllerActivator), new StructureMapControllerActivator(container));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "SearchAPI",
            routeTemplate: "search/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional });
        }
    }
}
