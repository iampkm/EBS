﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Application;
using EBS.Application.DTO;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Query;
using EBS.Query.DTO;
using Newtonsoft.Json;
using EBS.Admin.Services;
using EBS.Infrastructure.Log;
using EBS.Infrastructure.Extension;
namespace EBS.Admin.Controllers
{
    [Permission]
    public class VipProductController : Controller
    {
        IQuery _query;
        IVipProductQuery _vipProductQuery;
        IVipProductFacade _vipProductFacade;
        IContextService _context;
        ILogger _log;

        public VipProductController(IContextService context, IQuery query, IVipProductQuery vipProductQuery, ILogger log,IVipProductFacade vipProductFacade)
        {
            this._query = query;
            _vipProductQuery = vipProductQuery;
            _vipProductFacade = vipProductFacade;
            this._context = context;
            _log = log;
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadData(Pager page, SearchVipProduct condition)
        {
            var rows = _vipProductQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(string vipProducts)
        {
            _vipProductFacade.Create(vipProducts);
            return Json(new { success = true});
        }

        public JsonResult QueryProduct(string productCodeOrBarCode)
        {
            var model = _vipProductQuery.QueryProduct(productCodeOrBarCode);
            return Json(new { success = true, data = model });
        }

        public JsonResult ImportProduct(string inputProducts)
        {
            var rows=  _vipProductQuery.QueryProductByBarCode(inputProducts);
            return Json(new { success = true,data = rows });
        }
    }
}