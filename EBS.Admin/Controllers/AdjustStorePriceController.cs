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
    [Permission]
    public class AdjustStorePriceController : Controller
    {
        IQuery _query;
        IAdjustStorePriceQuery _adjustStorePriceQuery;
        IAdjustStorePriceFacade _adjustStorePriceFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        public AdjustStorePriceController(IContextService contextService, IQuery query, IAdjustStorePriceQuery adjustStorePriceQuery, IAdjustStorePriceFacade adjustStorePriceFacade, IAreaQuery areaQuery)
        {
            this._query = query;
            this._adjustStorePriceQuery = adjustStorePriceQuery;
            this._adjustStorePriceFacade = adjustStorePriceFacade;
            this._areaQuery = areaQuery;
            this._context = contextService;
        }
        public ActionResult Index()
        {
            SetUserAuthention();
           // ViewBag.Status = _adjustStorePriceQuery.GetAdjustStorePriceStatus();
            return View();
        }

        private void SetUserAuthention()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
        }
        public ActionResult AuditIndex()
        {
            SetUserAuthention();
            // ViewBag.Status = _AdjustStorePriceQuery.GetAdjustStorePriceStatus();
            ViewBag.waitingAudit = (int)AdjustStorePriceStatus.WaitAudit;
            return View();
        }

        public ActionResult Finish()
        {
            SetUserAuthention();
            // ViewBag.Status = _AdjustStorePriceQuery.GetAdjustStorePriceStatus();
            ViewBag.waitingAudit = (int)AdjustStorePriceStatus.WaitAudit;
            return View();
        }

        public JsonResult LoadData(Pager page, SearchAdjustStorePrice condition)
        {
            var rows = _adjustStorePriceQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public JsonResult QueryFinish(Pager page, SearchAdjustStorePrice condition)
        {
            var rows = _adjustStorePriceQuery.QueryFinish(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult Create()
        {
            SetUserAuthention();
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            return View();
        }

        public string LoadChildArea()
        {
            var treeNodes = _areaQuery.GetTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            return tree;
        }

        [HttpPost]
        public JsonResult Create(AdjustStorePriceModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _adjustStorePriceFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            SetUserAuthention();
           // var model = _query.Find<AdjustStorePrice>(id);
            var model = _adjustStorePriceQuery.GetById(id);
            if (model.Status == Domain.ValueObject.AdjustStorePriceStatus.Create)
            {
                model.UpdatedByName = "";
            }
            //var items = _adjustStorePriceQuery.GetItems(id);
            var items = model.Items;
            ViewBag.AdjustStorePriceItems = JsonConvert.SerializeObject(items.ToArray());
            //ViewBag.CreatedByName = _query.Find<Account>(model.CreatedBy).NickName;
            ViewBag.Status = model.Status.Description();
            //创建和待审可编辑
            //var editable = model.Status == AdjustStorePriceStatus.Create ;
            //ViewBag.Editable = editable.ToString().ToLower();
            //查询处理流程：
            //var logs= _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.AdjustStorePrice);
            //ViewBag.Logs = logs;
            //if (model.Status == AdjustStorePriceStatus.Create || model.Status == AdjustStorePriceStatus.Cancel)
            //{
            //    return View("Detail", model);
            //}

            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(AdjustStorePriceModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _adjustStorePriceFacade.Edit(model);
            return Json(new { success = true });
        }

        public ViewResult Detail(int id)
        {
            //var model = _query.Find<AdjustStorePrice>(id);
            //var items = _adjustStorePriceQuery.GetItems(id);
            var model = _adjustStorePriceQuery.GetById(id);

            ViewBag.AdjustStorePriceItems = JsonConvert.SerializeObject(model.Items.ToArray());
           // ViewBag.CreatedByName = _query.Find<Account>(model.u).NickName;
            ViewBag.Status = model.Status.Description();
            return View(model);
        }

        public JsonResult GetDetail(int id)
        {
            var model = _adjustStorePriceQuery.GetById(id);
            
            return Json(new { success = true,data = model });
        }

        public JsonResult Delete(int id, string reason)
        {
            _adjustStorePriceFacade.Delete(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, reason);
            return Json(new { success = true });
        }

        public JsonResult Submit(int id)
        {
            _adjustStorePriceFacade.Submit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
        public JsonResult Audit(int id)
        {
            _adjustStorePriceFacade.Audit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

        public JsonResult Reject(int id)
        {
            _adjustStorePriceFacade.Reject(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }


        public JsonResult GetItem(int storeId,string productCodeOrBarCode)
        {
            var result = _adjustStorePriceQuery.GetAdjustStorePriceItem(storeId,productCodeOrBarCode);
            return Json(new { success = true, data = result });
        }

        public JsonResult ImportProduct(int storeId, string inputProducts)
        {
            var result = _adjustStorePriceQuery.GetAdjustStorePriceList(storeId,inputProducts);
            return Json(new { success = true, data = result });
        }
	}
}