using System.Web.Mvc;
using System.Web.Routing;
using Tupperware.WebExample.Controllers;

namespace Tupperware.WebExample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new Container();
            container.Register<IFruitStand, FruitStand>();

            container.Register<SimpleController>();
            ControllerBuilder.Current.SetControllerFactory(new TupperwareControllerFactory(container));
        }
    }
}
