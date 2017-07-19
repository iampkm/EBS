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
    /// <summary>
    /// 其他出入库
    /// </summary>
    public class OutInOrderController : Controller
    {

        IContextService _context;
        IOutInOrderFacade _outInOrderFacade;
        IOutInOrderQuery _outInOrderQuery;
        public OutInOrderController(IContextService contextService, IOutInOrderFacade outInOrderFacade, IOutInOrderQuery outInOrderQuery)
        {
            this._context = contextService;
            this._outInOrderFacade = outInOrderFacade;
            this._outInOrderQuery = outInOrderQuery;
        }
       

        /// <summary>
        /// 其他入库单
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SetUserAuthention();
            ViewBag.Dics = _outInOrderQuery.GetOutInOrderTypes(1);
            ViewBag.OrderStatus = (int)OutInOrderStatus.Create;
            return View();
        }

        public ActionResult Create()
        {
            SetUserAuthention();
            ViewBag.Dics = _outInOrderQuery.GetOutInOrderTypes(1);
            return View(); 
        }


        /// <summary>
        /// 其他出库单
        /// </summary>
        /// <returns></returns>
        public ActionResult OutIndex()
        {
            SetUserAuthention();
            return View();
        }

        public ActionResult Refund()
        {
            SetUserAuthention();
            return View();
        }

        public ActionResult Detail(int id)
        {
            var model = _outInOrderQuery.GetById(id);
            return View(model);
        }

        public ActionResult AuditIndex()
        {
            SetUserAuthention();
            ViewBag.Dics = _outInOrderQuery.GetOutInOrderTypes(1);
            ViewBag.OrderStatus = (int)OutInOrderStatus.WaitAudit;
            return View();
        }
        public ActionResult FinanceIndex()
        {
            SetUserAuthention();
            ViewBag.Dics = _outInOrderQuery.GetOutInOrderTypes(1);
            ViewBag.OrderStatus = (int)OutInOrderStatus.Audited;
            return View();
        }

        public ActionResult Finish()
        {
            SetUserAuthention();
            SetThisMonth();
            return View();
        }

        public JsonResult LoadData(Pager page, SearchOutInOrder condition)
        {
            if (string.IsNullOrEmpty(condition.StoreId) || condition.StoreId == "0") { condition.StoreId = _context.CurrentAccount.CanViewStores; }
            var rows = _outInOrderQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }

        public JsonResult LoadFinishData(Pager page, SearchOutInOrder condition)
        {
            if (string.IsNullOrEmpty(condition.StoreId) || condition.StoreId == "0") { condition.StoreId = _context.CurrentAccount.CanViewStores; }
            var rows = _outInOrderQuery.GetFinishList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }

        [HttpPost]
        public JsonResult Create(OutInOrderModel model)
        {
            model.EditBy = _context.CurrentAccount.AccountId;
            model.EditByName = _context.CurrentAccount.NickName;
            _outInOrderFacade.Create(model);
            return Json(new { success = true, code = model.Code,  id = model.Id });
        }

        public ActionResult Edit(int id)
        {
            var model = _outInOrderQuery.GetById(id);
            ViewBag.OutInOrderItems = JsonConvert.SerializeObject(model.Items.ToArray());
            ViewBag.Status = model.Status.Description();
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(OutInOrderModel model)
        {
            model.EditBy = _context.CurrentAccount.AccountId;
            model.EditByName = _context.CurrentAccount.NickName;
            _outInOrderFacade.Edit(model);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Audit(int id)
        {
            _outInOrderFacade.Audit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult Cancel(int id,string reason)
        {
            _outInOrderFacade.Cancel(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, reason);
            return Json(new { success = true });
        }

        public JsonResult Submit(int id)
        {
            _outInOrderFacade.Submit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

        public JsonResult Reject(int id)
        {
            _outInOrderFacade.Reject(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

        public JsonResult QueryProduct(string productCodeOrBarCode, int storeId,int supplierId)
        {
            var model = _outInOrderQuery.QueryProduct(productCodeOrBarCode, storeId, supplierId);
            return Json(new { success = true, data = model });
        }

        public JsonResult QueryProductList(string inputProducts, int storeId, int supplierId)
        {
            var model = _outInOrderQuery.QueryProductList(inputProducts, storeId, supplierId);
            return Json(new { success = true, data = model });
        }


        public JsonResult GetDetail(int id)
        {
            var model = _outInOrderQuery.GetById(id);
            return Json(new { success = true, data = model });
        }


        public ActionResult Summary()
        {
            SetUserAuthention();
            SetThisMonth();
            return View();
        }

        public JsonResult LoadSummaryData(Pager page, SearchOutInOrder conditon)
        {
            var rows = _outInOrderQuery.GetSummaryList(page, conditon);

            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }

        private void SetThisMonth()
        {
            DateTime now = DateTime.Now;
            DateTime monthBegin = new DateTime(now.Year, now.Month, 1);
            DateTime monthEnd = monthBegin.AddMonths(1).AddDays(-1);
            ViewBag.BeginDate = monthBegin.ToString("yyyy-MM-dd");
            ViewBag.EndDate = monthEnd.ToString("yyyy-MM-dd");
        }

        private void SetUserAuthention()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
        }
	}
}