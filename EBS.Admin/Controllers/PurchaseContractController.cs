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
    public class PurchaseContractController : Controller
    {
        IQuery _query;
        IPurchaseContractQuery _purchaseContractQuery;
        IPurchaseContractFacade _purchaseContractFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        public PurchaseContractController(IContextService contextService, IQuery query, IPurchaseContractQuery purchaseContractQuery, IPurchaseContractFacade purchaseContractFacade, IAreaQuery areaQuery)
        {
            this._query = query;
            this._purchaseContractQuery = purchaseContractQuery;
            this._purchaseContractFacade = purchaseContractFacade;
            this._areaQuery = areaQuery;
            this._context = contextService;
        }
        public ActionResult Index()
        {
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;
            ViewBag.Stores = _query.FindAll<Store>();
            ViewBag.Status = _purchaseContractQuery.GetPurchaseContractStatus();
            return View();
        }

        public ActionResult AuditIndex()
        {
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;
            ViewBag.Stores = _query.FindAll<Store>();
            ViewBag.Status = _purchaseContractQuery.GetPurchaseContractStatus();
            ViewBag.waitingAudit = (int)PurchaseContractStatus.WaitingAudit;
            return View();
        }

        public JsonResult LoadData(Pager page, SearchSupplierContract condition)
        {
            var rows = _purchaseContractQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;

            ViewBag.Stores = _query.FindAll<Store>();
            return View();
        }

        public string LoadChildArea()
        {
            var treeNodes = _areaQuery.GetTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            return tree;
        }

        [HttpPost]
        public JsonResult Create(CreatePurchaseContract model)
        {

            //if (string.IsNullOrEmpty(model.Items)) throw new Exception("商品明细为空");
            //var productPriceList = JsonConvert.DeserializeObject<List<ProductPriceModel>>(model.Items);
            //Dictionary<int, decimal> productPriceDic = new Dictionary<int, decimal>();
            //productPriceList.ForEach(n => productPriceDic.Add(n.Id, n.Price));
            //model.ProductPriceDic = productPriceDic;
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedByName = _context.CurrentAccount.NickName;
            _purchaseContractFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<PurchaseContract>(id);
            var items = _purchaseContractQuery.GetPurchaseContractItems(id);
            ViewBag.PurchaseContractItems = JsonConvert.SerializeObject(items.ToArray());
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;
            ViewBag.Stores = _query.FindAll<Store>();
            //创建和待审可编辑
            var editable = model.Status == PurchaseContractStatus.Create || model.Status == PurchaseContractStatus.WaitingAudit;
            ViewBag.Editable = editable ? "true" : "false";
            //查询处理流程：
            var logs= _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.PurchaseContract);
            ViewBag.Logs = logs;
          
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(EditPurchaseContract model)
        {
            if (string.IsNullOrEmpty(model.Items)) throw new Exception("商品明细为空");
            var productPriceList = JsonConvert.DeserializeObject<List<ProductPriceModel>>(model.Items);
            Dictionary<int, decimal> productPriceDic = new Dictionary<int, decimal>();
            productPriceList.ForEach(n => productPriceDic.Add(n.Id, n.Price));
            model.ProductPriceDic = productPriceDic;
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _purchaseContractFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(int id, string reason)
        {
            _purchaseContractFacade.Delete(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, reason);
            return Json(new { success = true });
        }

        public JsonResult Submit(int id)
        {
            _purchaseContractFacade.Submit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
        public JsonResult Audit(int id)
        {
            _purchaseContractFacade.Audit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

        public JsonResult ImportProduct(string productCodePriceInput)
        {
            var rows = _purchaseContractQuery.GetPurchaseContractItems(productCodePriceInput);
            return Json(new { success = true, data = rows });
        }

    }
}