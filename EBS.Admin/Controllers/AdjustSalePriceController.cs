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
    public class AdjustSalePriceController : Controller
    {
        IQuery _query;
        IAdjustSalePriceQuery _adjustSalePriceQuery;
        IAdjustSalePriceFacade _adjustSalePriceFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        public AdjustSalePriceController(IContextService contextService, IQuery query, IAdjustSalePriceQuery adjustSalePriceQuery, IAdjustSalePriceFacade adjustSalePriceFacade, IAreaQuery areaQuery)
        {
            this._query = query;
            this._adjustSalePriceQuery = adjustSalePriceQuery;
            this._adjustSalePriceFacade = adjustSalePriceFacade;
            this._areaQuery = areaQuery;
            this._context = contextService;
        }
        public ActionResult Index()
        {
            ViewBag.Status = _adjustSalePriceQuery.GetAdjustSalePriceStatus();
            return View();
        }

        public ActionResult AuditIndex()
        {
            // ViewBag.Status = _AdjustSalePriceQuery.GetAdjustSalePriceStatus();
            ViewBag.waitingAudit = (int)AdjustSalePriceStatus.Valid;
            return View();
        }

        public JsonResult LoadData(Pager page, SearchAdjustSalePrice condition)
        {
            var rows = _adjustSalePriceQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
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
        public JsonResult Create(AdjustSalePriceModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _adjustSalePriceFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<AdjustSalePrice>(id);
            var items = _adjustSalePriceQuery.GetItems(id);

            ViewBag.AdjustSalePriceItems = JsonConvert.SerializeObject(items.ToArray());
            ViewBag.CreatedByName = _query.Find<Account>(model.CreatedBy).NickName;
            ViewBag.Status = model.Status.Description();
            //创建和待审可编辑
            var editable = model.Status == AdjustSalePriceStatus.InValid ;
            ViewBag.Editable = editable.ToString().ToLower();
            //查询处理流程：
            //var logs= _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.AdjustSalePrice);
            //ViewBag.Logs = logs;
            if (model.Status == AdjustSalePriceStatus.Valid || model.Status == AdjustSalePriceStatus.Cancel)
            {
                return View("Detail", model);
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(AdjustSalePriceModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _adjustSalePriceFacade.Edit(model);
            return Json(new { success = true });
        }

        public ViewResult Detail(int id)
        {
            var model = _query.Find<AdjustSalePrice>(id);
            var items = _adjustSalePriceQuery.GetItems(id);

            ViewBag.AdjustSalePriceItems = JsonConvert.SerializeObject(items.ToArray());
            ViewBag.CreatedByName = _query.Find<Account>(model.CreatedBy).NickName;
            ViewBag.Status = model.Status.Description();
            return View(model);
        }

        public JsonResult Delete(int id, string reason)
        {
            _adjustSalePriceFacade.Delete(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, reason);
            return Json(new { success = true });
        }

        public JsonResult Submit(int id)
        {
            _adjustSalePriceFacade.Submit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
        public JsonResult Audit(int id)
        {
            _adjustSalePriceFacade.Audit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }


        public JsonResult GetItem(string productCodeOrBarCode)
        {
            var result = _adjustSalePriceQuery.GetAdjustSalePriceItem(productCodeOrBarCode);
            return Json(new { success = true, data = result });
        }

        public JsonResult ImportProduct(string inputProducts)
        {
            var result = _adjustSalePriceQuery.GetAdjustSalePriceList(inputProducts);
            return Json(new { success = true, data = result });
        }
    }
}