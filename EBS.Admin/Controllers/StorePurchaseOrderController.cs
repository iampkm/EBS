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
namespace EBS.Admin.Controllers
{
    [Permission]
    public class StorePurchaseOrderController : Controller
    {        
        IQuery _query;
        IStorePurchaseOrderQuery _storePurchaseOrderQuery;
        IStorePurchaseOrderFacade _storePurchaseOrderFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        public StorePurchaseOrderController(IContextService contextService, IQuery query, IStorePurchaseOrderQuery StorePurchaseOrderQuery, IStorePurchaseOrderFacade StorePurchaseOrderFacade, IAreaQuery areaQuery)
        {
            this._query = query;
            this._storePurchaseOrderQuery = StorePurchaseOrderQuery;
            this._storePurchaseOrderFacade = StorePurchaseOrderFacade;
            this._areaQuery = areaQuery;
            this._context = contextService;
        }

        public ActionResult Index()
        {
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;
            ViewBag.Stores = _query.FindAll<Store>();
            ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            return View();
        }

        public JsonResult LoadData(Pager page, SearchStorePurchaseOrder condition)
        {
            var rows = _storePurchaseOrderQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;
            ViewBag.Stores = _query.FindAll<Store>();
            ViewBag.Status = "创建";
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            return View();
        }
        [HttpPost]
        public JsonResult Create(CreateStorePurchaseOrder model)
        {               
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedByName = _context.CurrentAccount.NickName;
            _storePurchaseOrderFacade.Create(model);
            return Json(new { success = true });
        }

        public JsonResult GetPurchaseOrderItem(string productCodeOrBarCode, int supplierId, int storeId)
        {
            var item = _storePurchaseOrderQuery.GetPurchaseOrderItem(productCodeOrBarCode, supplierId, storeId);
            return Json(new { success = true,data = item });
        }


	}
}