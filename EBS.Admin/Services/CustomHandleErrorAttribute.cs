using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Infrastructure;
using EBS.Infrastructure.Log;
namespace EBS.Admin.Services
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //记录系统日志
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            var log = AppContext.Current.Resolve<ILogger>();
            log.Error(filterContext.Exception);          
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, error = filterContext.Exception.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.OnException(filterContext);
            }          
        }
    }
}