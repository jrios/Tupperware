using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tupperware.WebExample
{
    public class TupperwareControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public TupperwareControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return _container.Resolve(controllerType).As<IController>();
        }
    }
}