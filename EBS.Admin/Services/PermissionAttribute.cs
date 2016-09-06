using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBS.Admin.Services
{
     [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //   // base.OnActionExecuting(filterContext);           
        //    if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    { 
        //        // not login user
        //        filterContext.Result = new RedirectResult("/Account/Login" );
        //        return;
        //    }
        //    string controllerName = filterContext.RouteData.Values["controller"].ToString();
        //    string actionName = filterContext.RouteData.Values["action"].ToString();
        //    string requestURL = "/" + controllerName + "/" + actionName;
        //}

        //public void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;
        //    if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
        //    {
        //        filterContext.Result = new JsonResult
        //        {
        //            Data = new { success = false, error = filterContext.Exception.Message },
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };
        //    }
        //    else {
        //        filterContext.Result = new RedirectResult("/Error.html");
        //    }
        //}



        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;           
             
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //current request is authenticated
            //var routeDate = filterContext.RouteData;
            //string controllerName = routeDate.Values["controller"].ToString();
            //string actionName = routeDate.Values["action"].ToString();
            //string requestURL = string.Format("/{0}/{1}", controllerName, actionName);
            // if admin return true

            // if did not setting requestUrl   return true
           // filterContext.Result = new HttpUnauthorizedResult();
            // if setting request url, but current user not has this url ,return false
            //if(string.Equals(requestURL,"",StringComparison.OrdinalIgnoreCase))
            //{
            //    filterContext.Result = new RedirectResult("/Home/Index");
            //    return;
            //}
           
            base.OnAuthorization(filterContext);
                 
        }
    }
}