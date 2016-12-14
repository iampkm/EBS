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
    public class StocktakingController : Controller
    {       
        IQuery _query;
        IContextService _context;
        IStocktakingQuery _stocktakingQuery;
        IStocktakingFacade _stocktakingFacade;

        public StocktakingController(IContextService contextService, IQuery query, IStocktakingQuery stocktakingQuery, IStocktakingFacade stocktakingFacade)
        {
            this._query = query;
            this._context = contextService;
            this._stocktakingQuery = stocktakingQuery;
            _stocktakingFacade = stocktakingFacade;

        }

        public ActionResult Index()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.Status = typeof(StocktakingType).GetValueToDescription();
            return View();
        }

        public JsonResult LoadData(Pager page, SearchStocktaking condition)
        {
            var rows = _stocktakingQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }
        /// <summary>
        /// 修正单审核
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditIndex()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.Status = typeof(StocktakingStatus).GetValueToDescription();
            return View();
        }
        [HttpPost]
        public ActionResult Audit(int id)
        {           
            _stocktakingFacade.Audit(id);
            return Json(new { success = true });
        }

         public JsonResult LoadAuditData(Pager page, SearchStocktaking condition)
        {
            var rows = _stocktakingQuery.GetAuditList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult Create()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = 0;
            ViewBag.StoreName = "";
            if (_context.CurrentAccount.StoreId > 0)
            {
                var store = _query.Find<Store>(_context.CurrentAccount.StoreId);
                ViewBag.StoreId = store.Id;
                ViewBag.StoreName = store.Name;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(StocktakingModel model)
        {
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedByName = _context.CurrentAccount.NickName;
            _stocktakingFacade.Create(model);
            return Json(new { success = true });
        }

        public JsonResult QueryShelfProduct(int storeId, string shelfCode)
        {
            var model = GetRunningPlan(storeId);

            var rows = _stocktakingQuery.QueryShelfProduct(storeId, shelfCode).ToList();
            return Json(new { success = true,data = rows,plan = model });
        }
        public JsonResult QueryShelfProduct(int planId,int storeId, string productCodeOrBarCode)
        {
            var stocktakingPlan = new StocktakingPlan();
            if (planId == 0) {
                stocktakingPlan = GetRunningPlan(storeId);
            }
            var model = _stocktakingQuery.QueryShelfProduct(planId, storeId, productCodeOrBarCode);
            return Json(new { success = true, data = model, plan = stocktakingPlan });
        }

        private StocktakingPlan GetRunningPlan(int storeId)
        {
            var model = this._query.Find<StocktakingPlan>(n => n.StoreId == storeId && n.Status == StocktakingPlanStatus.FirstInventory);
            if (model == null)
            {
                throw new Exception("没有开始盘点计划");
            }
            return model;
        }

        public ActionResult Correct()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = 0;
            ViewBag.StoreName = "";
            if (_context.CurrentAccount.StoreId > 0)
            {
                var store = _query.Find<Store>(_context.CurrentAccount.StoreId);
                ViewBag.StoreId = store.Id;
                ViewBag.StoreName = store.Name;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Correct(StocktakingModel model)
        {
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedByName = _context.CurrentAccount.NickName;
            _stocktakingFacade.Correct(model);
            return Json(new { success = true });
        }

        public JsonResult QueryStocktakingItem(int planId, string productCodeOrBarCode)
        {
            var model = _stocktakingQuery.QueryStocktaingItem(planId, productCodeOrBarCode);
            return Json(new { success = true,data = model });
        }

        public ActionResult Edit(int id)
        {
            var model = _query.Find<Stocktaking>(id);
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = 0;
            ViewBag.StoreName = "";
            if (_context.CurrentAccount.StoreId > 0)
            {
                var store = _query.Find<Store>(_context.CurrentAccount.StoreId);
                ViewBag.StoreId = store.Id;
                ViewBag.StoreName = store.Name;
            }
            return View(model);
        }

        public ActionResult Edit(StocktakingModel model)
        {

            return Json(new { success = true });
        }
	}
}