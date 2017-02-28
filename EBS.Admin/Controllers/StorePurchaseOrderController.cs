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
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
           // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowCreateStatus = (int)PurchaseOrderStatus.Create;
            
            return View();
        }

        public JsonResult LoadData(Pager page, SearchStorePurchaseOrder condition)
        {
             var rows = _storePurchaseOrderQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 待收货列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiveIndex()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowReceiveStatus = string.Format("{0},{1}", (int)PurchaseOrderStatus.Create, (int)PurchaseOrderStatus.WaitStockIn);
            return View();
        }

        /// <summary>
        /// 财务待审-采购单
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceIndex()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
           // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowStatus = (int)PurchaseOrderStatus.Finished;
            ViewBag.ShowType = (int)OrderType.Order;
            return View();
        }

        /// <summary>
        /// 财务待审-采购退单
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceRefundIndex()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowStatus = (int)PurchaseOrderStatus.Finished;
            ViewBag.ShowType = (int)OrderType.Refund;
            return View();
        }

        /// <summary>
        /// 采购单查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Query()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
           // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowStatus = string.Format("{0},{1}", (int)PurchaseOrderStatus.Finished, (int)PurchaseOrderStatus.FinanceAuditd);
            ViewBag.ShowType = (int)OrderType.Order;
            return View();
        }

        public ActionResult QueryRefund()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowStatus = string.Format("{0},{1}", (int)PurchaseOrderStatus.Finished, (int)PurchaseOrderStatus.FinanceAuditd);
            ViewBag.ShowType = (int)OrderType.Refund;
            return View();
        }

        /// <summary>
        /// 采购单退单
        /// </summary>
        /// <returns></returns>
        public ActionResult RefundIndex()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
           // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowCreateStatus = (int)PurchaseOrderStatus.Create;
            return View();
        }
        /// <summary>
        /// 采购单退单-退货
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitRefundIndex()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
           // ViewBag.Status = _storePurchaseOrderQuery.GetStorePurchaseOrderStatus();
            ViewBag.ShowRefundStatus = string.Format("{0},{1}", (int)PurchaseOrderStatus.Create, (int)PurchaseOrderStatus.WaitStockOut);
            return View();
        }


        public ActionResult Create()
        {
            ViewBag.IsGift = "false";
            ViewBag.Status = PurchaseOrderStatus.Create.Description();
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
            return View();
        }

        public ActionResult CreateGift()
        {
            ViewBag.IsGift = "true";
            ViewBag.Status = PurchaseOrderStatus.Create.Description();
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
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
       
        public ActionResult Details(int id,string audit="false")
        {
            var model = _storePurchaseOrderQuery.GetById(id);            
            var logs = _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.StorePurchaseOrder);
            ViewBag.Logs = logs;
            ViewBag.Audit = audit;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            var model = _storePurchaseOrderQuery.GetById(id);            
            ViewBag.StorePurchaseOrderItems = JsonConvert.SerializeObject(model.Items.ToArray());
            //查询处理流程：
            var logs = _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.StorePurchaseOrder);
            ViewBag.Logs = logs;
            return View(model);
        }
        /// <summary>
        /// 待入库
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitStockIn(int id)
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            var model = _storePurchaseOrderQuery.GetById(id);
            model.ReceivedByName = _context.CurrentAccount.NickName;
            //设置默认实收 = 应收
            model.Items.ForEach((item) =>
            {

                item.ActualQuantity = item.ActualQuantity == 0 ? item.Quantity : item.ActualQuantity;

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
            return View(model);
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

        public JsonResult GetPurchaseOrderItem(string productCodeOrBarCode, int storeId,int supplierId)
        {
            var result = _storePurchaseOrderQuery.GetPurchaseOrderItem(productCodeOrBarCode, storeId, supplierId);
          
            return Json(new { success = true, data = result });
        }

        public JsonResult GetRefundOrderItem(string productCodeOrBarCode, int storeId, long batchNo = 0)
        {
            var result = _storePurchaseOrderQuery.GetRefundOrderItem(productCodeOrBarCode, storeId, batchNo);
            return Json(new { success = true, data = result });
        }

        public JsonResult ImportRefundProduct(string inputProducts, int storeId)
        {
            var result = _storePurchaseOrderQuery.GetRefundOrderItemList(inputProducts, storeId);
            return Json(new { success = true, data = result });
        }

        public JsonResult ImportProduct(string inputProducts, int storeId, int supplierId)
        {
            var result = _storePurchaseOrderQuery.GetPurchaseOrderItemList(inputProducts,  storeId, supplierId);
            return Json(new { success = true, data = result });
        }
        public JsonResult Delete(int id, string reason)
        {
            _storePurchaseOrderFacade.Delete(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, reason);
            return Json(new { success = true });
        }
        /// <summary>
        /// 财务审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult FinanceAuditd(int id)
        {
            _storePurchaseOrderFacade.FinanceAuditd(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

        public ActionResult Print(int id)
        {
            var model = _storePurchaseOrderQuery.GetById(id);
            return PartialView("StorePurchaseOrderTemplate",model);
        }

        public ActionResult Refund()
        {
            ViewBag.IsGift = "false";
            ViewBag.Status = PurchaseOrderStatus.Create.Description();
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
            return View();
        }

        public JsonResult LoadProductBatchs(string productCodeOrBarCode,int storeId)
        {
            var rows = _storePurchaseOrderQuery.GetProductBatchs(productCodeOrBarCode, storeId).ToList();
            return Json(new { success = true, data = rows, total = rows.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefundEdit(int id)
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            var model = _storePurchaseOrderQuery.GetById(id);
            ViewBag.StorePurchaseOrderItems = JsonConvert.SerializeObject(model.Items.ToArray());

            return View(model);
        }

        public ActionResult RefundDetails(int id,string audit = "false")
        {
            var model = _storePurchaseOrderQuery.GetById(id);
            var logs = _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == BillIdentity.StorePurchaseRefundOrder.ToString());
            ViewBag.Logs = logs;
            ViewBag.Audit = audit;
            return View(model);
        }

        /// <summary>
        /// 待退货
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitStockOut(int id)
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            var model = _storePurchaseOrderQuery.GetById(id);
            model.ReceivedByName = _context.CurrentAccount.NickName;
            //设置默认实收 = 应收
            model.Items.ForEach((item) =>
            {

                item.ActualQuantity = item.ActualQuantity == 0 ? item.Quantity : item.ActualQuantity;

                if (item.SpecificationQuantitys[0] > 1)
                {
                    item.PackageQuantity = item.Quantity / item.SpecificationQuantitys[0];
                    item.ActualPackageQuantity = item.ActualQuantity / item.SpecificationQuantitys[0];
                }

            });
            ViewBag.StorePurchaseOrderItems = JsonConvert.SerializeObject(model.Items.ToArray());

            return View(model);
        }

        //出库
        public JsonResult StockOutInventory(int id)
        {
            _storePurchaseOrderFacade.GetOutOfInventory(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

    }
}