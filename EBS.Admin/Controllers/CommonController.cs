﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBS.Admin.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
	}
}