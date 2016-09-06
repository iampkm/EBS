using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Infrastructure;
using EBS.Infrastructure.Log;
using EBS.Admin.Services;
namespace EBS.Admin.Controllers
{
    [Permission]
    public class HomeController : Controller
    {
        ILogger _log;
        IContextService _contextService;
        public HomeController(ILogger log,IContextService contextService)
        {
            this._log = log;
            this._contextService = contextService;
        }
        public ActionResult Index()
        {
            AppContext.Current.Resolve<ILogger>().Info("app context had Start  this is /home/index");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            this._log.Info("about");
           // loadError();
            return View();
        }

        public ActionResult DashBoard()
        {
            this._log.Info("account:"+this._contextService.CurrentAccount.NickName);
            return View();
        }

        private void loadError()
        {
            throw new Exception("code is error");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}