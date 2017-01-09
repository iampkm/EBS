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
            CurrentStore();
            ViewBag.Status = _purchaseContractQuery.GetPurchaseContractStatus();
            return View();
        }

        public ActionResult AuditIndex()
        {
            ViewBag.Status = _purchaseContractQuery.GetPurchaseContractStatus();
            ViewBag.waitingAudit = (int)PurchaseContractStatus.WaitingAudit;
            return View();
        }

        private void CurrentStore()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
        }

        public JsonResult LoadData(Pager page, SearchSupplierContract condition)
        {
            var rows = _purchaseContractQuery.GetPageList(page, condition);

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
        public JsonResult Create(CreatePurchaseContract model)
        {
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
            var supplier = _query.Find<Supplier>(model.SupplierId);
            ViewBag.SupplierName = supplier.Name;
            var stores = _query.Find<Store>(model.StoreIds.Split(',').ToIntArray()).Select(n=>n.Name).ToArray();
            ViewBag.StoreName =string.Join(",",stores) ;
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
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            model.UpdatedByName = _context.CurrentAccount.NickName;
            _purchaseContractFacade.Edit(model);
            return Json(new { success = true });
        }

        public ActionResult Detail(int id)
        {
            var model = _query.Find<PurchaseContract>(id);
            var items = _purchaseContractQuery.GetPurchaseContractItems(id);
            ViewBag.PurchaseContractItems = items;
            var supplier = _query.Find<Supplier>(model.SupplierId);
            ViewBag.SupplierName = supplier.Name;
            var stores = _query.Find<Store>(model.StoreIds.Split(',').ToIntArray()).Select(n => n.Name).ToArray();
            ViewBag.StoreName = string.Join(",", stores);
            //创建和待审可编辑
            var editable =model.Status == PurchaseContractStatus.WaitingAudit;
            ViewBag.CanAudit = editable ? "true" : "false";

            ViewBag.StatusName = model.Status.Description();
            var account = _query.Find<Account>(model.CreatedBy);
            ViewBag.CreatedByName = account.NickName;
            //查询处理流程：
            var logs = _query.FindAll<ProcessHistory>(n => n.FormId == id && n.FormType == FormType.PurchaseContract);
            ViewBag.Logs = logs;
            return View(model);
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

        /// <summary>
        ///  根据供应商比价结果创建合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateContract(int id)
        {
            var model = _purchaseContractQuery.QueryContractInfo(id);
            ViewBag.PurchaseContractItems = JsonConvert.SerializeObject(model.Items.ToArray());
            return View(model);
        }

    }
}