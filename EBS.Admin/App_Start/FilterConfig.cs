using System.Web;
using System.Web.Mvc;
using EBS.Admin.Services;
namespace EBS.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
