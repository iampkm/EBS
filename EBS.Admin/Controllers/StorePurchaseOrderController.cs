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
    [Permission]
    public class StorePurchaseOrderController : Controller
    {
        IQuery _query;
        IStorePurchaseOrderQuery _storePurchaseOrderQuery;
        IStorePurchaseOrderFacade _storePurchaseOrderFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        IAdjustContractPriceQuery _adjustContractPriceQuery;
        public StorePurchaseOrderController(IContextService contextService, IQuery query, IStorePurchaseOrderQuery StorePurchaseOrderQuery, 
            IStorePurchaseOrderFacade StorePurchaseOrderFacade, IAreaQuery areaQuery,IAdjustContractPriceQuery adjustContractPriceQuery)
        {
            this._query = query;
            this._storePurchaseOrderQuery = StorePurchaseOrderQuery;
            this._storePurchaseOrderFacade = StorePurchaseOrderFacade;
            this._areaQuery = areaQuery;
            this._context = contextService;
            this._adjustContractPriceQuery = adjustContractPriceQuery;
        }

        public ActionResult Index()
        {
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
            ViewBag.IsGift = "false";
            ViewBag.Status = "初始";
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            return View();
        }

        public ActionResult CreateGift()
        {
            ViewBag.IsGift = "true";
            ViewBag.Status = "初始";
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            return View("Create");
        }

        [HttpPost]
        public JsonResult Create(CreateStorePurchaseOrder model)
        {
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedByName = _context.CurrentAccount.NickName;
            _storePurchaseOrderFacade.Create(model);
            return Json(new { success = true });
        }

        public ActionResult Edit(int id)
        {
            var model = _storePurchaseOrderQuery.GetById(id);
            var page = "Edit";
            switch (model.Status)
            {
                case PurchaseOrderStatus.WaitReceivedGoods:
                case PurchaseOrderStatus.WaitingStockIn:
                    page = "WaitStockIn";
                    model.ReceivedByName = _context.CurrentAccount.NickName;
                    break;
                case PurchaseOrderStatus.HadStockIn:
                case PurchaseOrderStatus.Cancel:
                    page = "HadStockedIn";
                    break;
                default:
                    page = "Edit";
                    break;
            }
            //设置默认实收 = 应收
            model.Items.ForEach((item) =>
            {
                if (model.Status == PurchaseOrderStatus.WaitingStockIn)
                {
                    item.ActualQuantity = item.ActualQuantity == 0 ? item.Quantity : item.ActualQuantity;                   
                }
                if (item.SpecificationQuantitys[0] > 1)
                {
                    item.PackageQuantity = item.Quantity / item.SpecificationQuantitys[0];
                    item.ActualPackageQuantity = item.ActualQuantity / item.SpecificationQuantitys[0];
                }
               
            });
            ViewBag.StorePurchaseOrderItems = JsonConvert.SerializeObject(model.Items.ToArray());
            //查询处理流程：
            var logs = _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.StorePurchaseOrder);
            ViewBag.Logs = logs;
            return View(page, model);
        }
        /// <summary>
        /// 待入库
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitStockIn()
        {
            return View();
        }
        /// <summary>
        /// 收货
        /// </summary>
        /// <returns></returns>
        public JsonResult ReceiveGoods(ReceivedGoodsStorePurchaseOrder model)
        {           
            model.ReceivedBy = _context.CurrentAccount.AccountId;
            model.ReceivedByName = _context.CurrentAccount.NickName;
            model.ReceivedOn = DateTime.Now;
            _storePurchaseOrderFacade.ReceivedGoods(model);
            return Json(new { success = true });
        }

        public ActionResult HadStockedIn()
        {
            return View();
        }
        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult SaveInventory(int id)
        {
            _storePurchaseOrderFacade.SaveInventory(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }



        [HttpPost]
        public JsonResult Edit(EditStorePurchaseOrder model)
        {
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedByName = _context.CurrentAccount.NickName;
            _storePurchaseOrderFacade.Edit(model);           
            return Json(new { success = true });
        }

        public JsonResult GetPurchaseOrderItem(string productCodeOrBarCode, int supplierId, int storeId)
        {
            var result = _storePurchaseOrderQuery.GetPurchaseOrderItem(productCodeOrBarCode, supplierId, storeId);
            //  查询是否有自主调价
           // var item= _adjustContractPriceQuery.GetAdjustContractPriceItem(productCodeOrBarCode, supplierId, storeId);
             
            return Json(new { success = true, data = result });
        }

        public JsonResult ImportProduct(string inputProducts, int supplierId, int storeId)
        {
            var result = _storePurchaseOrderQuery.GetPurchaseOrderItemList(inputProducts, supplierId, storeId);
            return Json(new { success = true, data = result });
        }
        public JsonResult Delete(int id, string reason)
        {
            _storePurchaseOrderFacade.Delete(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, reason);
            return Json(new { success = true });
        }

        public JsonResult Submit(int id)
        {
            _storePurchaseOrderFacade.Submit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

        public ActionResult Print(int id)
        {
            var model = _storePurchaseOrderQuery.GetById(id);
            return PartialView("StorePurchaseOrderTemplate",model);
        }

    }
}