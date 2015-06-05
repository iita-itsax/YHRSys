using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace YHRSys
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e) { 
            Exception lastError;
            String ErrMessage;

            lastError = Server.GetLastError();
            if (lastError != null)
                ErrMessage = lastError.Message;
            else
                ErrMessage = "";

            Response.Write("Last Error: " + ErrMessage);
        }
    }
}
