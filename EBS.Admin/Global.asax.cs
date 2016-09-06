using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EBS.Infrastructure.Log;
using EBS.Infrastructure;
using EBS.Admin.Controllers;
namespace EBS.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppConfig.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();           
            var httpStatusCode = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;
            Response.Clear();
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            switch (httpStatusCode)
            {
                case 404:
                    routeData.Values.Add("action", "PageNotFound");
                    break;
                default:
                    routeData.Values["action"] = "Error";
                    var log = AppContext.Current.Resolve<ILogger>();
                    log.Error(exception);
                    break;
            }

            IController errorController = AppContext.Current.Resolve<CommonController>();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
