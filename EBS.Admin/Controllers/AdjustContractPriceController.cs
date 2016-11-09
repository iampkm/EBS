using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Application;
using EBS.Application.DTO;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using EBS.Query;
using EBS.Query.DTO;
using Newtonsoft.Json;
using EBS.Admin.Services;
using EBS.Infrastructure.Extension;
namespace EBS.Admin.Controllers
{
    public class AdjustContractPriceController : Controller
    {
         IQuery _query;
        IAdjustContractPriceQuery _adjustContractPriceQuery;
        IAdjustContractPriceFacade _adjustContractPriceFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        public AdjustContractPriceController(IContextService contextService, IQuery query, IAdjustContractPriceQuery adjustContractPriceQuery, IAdjustContractPriceFacade adjustContractPriceFacade, IAreaQuery areaQuery)
        {
            this._query = query;
            this._adjustContractPriceQuery = adjustContractPriceQuery;
            this._adjustContractPriceFacade = adjustContractPriceFacade;
            this._areaQuery = areaQuery;
            this._context = contextService;
        }
        public ActionResult Index()
        {
            ViewBag.Status = _adjustContractPriceQuery.GetAdjustContractPriceStatus();
            return View();
        }

        public ActionResult AuditIndex()
        {
            ViewBag.Status = _adjustContractPriceQuery.GetAdjustContractPriceStatus();
            ViewBag.waitingAudit = (int)AdjustContractPriceStatus.WaitingAudit;
            return View();
        }

        public JsonResult LoadData(Pager page, SearchSupplierContract condition)
        {
            var rows = _adjustContractPriceQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {           
            return View();
        }

        public string LoadChildArea()
        {
            var treeNodes = _areaQuery.GetTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            return tree;
        }

        [HttpPost]
        public JsonResult Create(AdjustContractPriceModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _adjustContractPriceFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<AdjustContractPrice>(id);
            var items = _adjustContractPriceQuery.GetAdjustContractPriceItems(id);
            ViewBag.AdjustContractPriceItems = JsonConvert.SerializeObject(items.ToArray());
            var supplier = _query.Find<Supplier>(model.SupplierId);
            ViewBag.SupplierName = supplier.Name;
            var store = _query.Find<Store>(model.StoreId);
            ViewBag.StoreName =store.Name ;
            //创建和待审可编辑
            var editable = model.Status == AdjustContractPriceStatus.Create || model.Status == AdjustContractPriceStatus.WaitingAudit;
            ViewBag.Editable = editable ? "true" : "false";
            //查询处理流程：
            var logs= _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.AdjustContractPrice);
            ViewBag.Logs = logs;
          
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(AdjustContractPriceModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _adjustContractPriceFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(int id, string reason)
        {
            _adjustContractPriceFacade.Delete(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, reason);
            return Json(new { success = true });
        }

        public JsonResult Submit(int id)
        {
            _adjustContractPriceFacade.Submit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
        public JsonResult Audit(int id)
        {
            _adjustContractPriceFacade.Audit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

        public JsonResult ImportProduct(string productCodePriceInput)
        {
            var rows = _adjustContractPriceQuery.GetAdjustContractPriceItems(productCodePriceInput);
            return Json(new { success = true, data = rows });
        }
	}
}