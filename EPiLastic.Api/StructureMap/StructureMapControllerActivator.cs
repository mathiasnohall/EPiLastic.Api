using StructureMap;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Svea.Api.StructureMap
{
    public class StructureMapControllerActivator : IHttpControllerActivator
    {
        private readonly IContainer _container;

        public StructureMapControllerActivator(IContainer container)
        {
            _container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            using (var nestedContainer = _container.GetNestedContainer())
            {
                var controller = nestedContainer.GetInstance(controllerType) as IHttpController;

                return controller;
            }
        }
    }
}