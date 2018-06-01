using MobileApi.DataAccess;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MobileApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DocumentDBRepository.Initialize();
        }
    }
}
