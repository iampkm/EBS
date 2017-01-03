using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBS.Admin.Controllers
{
    public class SaleOrderController : Controller
    {
        //
        // GET: /SaleOrder/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }
	}
}