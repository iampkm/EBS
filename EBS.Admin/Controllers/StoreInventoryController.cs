using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Application;
using EBS.Application.DTO;
using Dapper.DBContext;
using EBS.Query;
using EBS.Query.DTO;
using Newtonsoft.Json;
using EBS.Admin.Services;
using EBS.Infrastructure.Extension;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
namespace EBS.Admin.Controllers
{
    public class StoreInventoryController : Controller
    {
        IQuery _query;
        IStoreInventoryQuery _storeInventoryQuery;
       // IStoreInventoryFacade _StoreInventoryFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        public StoreInventoryController(IContextService contextService, IQuery query, IStoreInventoryQuery storeInventoryQuery, IAreaQuery areaQuery)
        {
            this._query = query;
            this._storeInventoryQuery = storeInventoryQuery;
            this._areaQuery = areaQuery;
            this._context = contextService;
        }

        public ActionResult Index()
        {
           
            return View();
        }

        public JsonResult LoadData(Pager page, SearchStoreInventory condition)
        {
            var rows = _storeInventoryQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult History()
        {
            return View();
        }
        public JsonResult LoadDataHistory(Pager page, SearchStoreInventoryHistory condition)
        {
            var rows = _storeInventoryQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }
        public ActionResult Batch()
        {
            return View();
        }
        public JsonResult LoadDataBatch(Pager page, SearchStoreInventoryBatch condition)
        {
            var rows = _storeInventoryQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }
    }
}