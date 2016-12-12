using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBS.Admin.Controllers
{
    public class StocktakingController : Controller
    {
        //
        // GET: /Stocktaking/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Audit()
        {
            return View();
        }
	}
}