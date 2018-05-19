using MobileApi.DataAccess;
using MobileApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MobileApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //            DocumentDBRepository<HierarchyNode>.Initialize();
            DocumentDBRepository.Initialize();
        }
    }
}
