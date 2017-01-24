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
    public class StocktakingPlanController : Controller
    {
        IQuery _query;
        IContextService _context;
        IStocktakingPlanQuery _stocktakingPlanQuery;
        IStocktakingPlanFacade _stocktakingPlanFacade;

        public StocktakingPlanController(IContextService contextService, IQuery query, IStocktakingPlanQuery stocktakingPlanQuery, IStocktakingPlanFacade stocktakingPlanFacade)
        {
            this._query = query;
            this._context = contextService;
            this._stocktakingPlanQuery = stocktakingPlanQuery;
            _stocktakingPlanFacade = stocktakingPlanFacade;

        }

        public ActionResult Index()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.Status = _stocktakingPlanQuery.GetStocktakingPlanStatus();
            return View();
        }

        public JsonResult LoadData(Pager page, SearchStocktakingPlan condition)
        {
            var rows = _stocktakingPlanQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }



        public ActionResult Help()
        {
            return View();
        }
        /// <summary>
        /// 汇总表
        /// </summary>
        /// <returns></returns>
        public ActionResult Summary()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";          
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;                  
            return View();
        }

        public JsonResult LoadSummaryData(Pager page, SearchStocktakingPlanSummary condition)
        {
            var rows = _stocktakingPlanQuery.GetSummaryData(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult Finish()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";         
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
            return View();
        }

        public ActionResult Detail(int id)
        {
            ViewBag.PlanId = id;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="from">差异数 从</param>
        /// <param name="to">差异数 到</param>
        /// <param name="showDifference">显示差异</param>
        /// <returns></returns>
        public ActionResult LoadDetail(Pager page, int planId, int? from, int? to, bool? showDifference,string productCodeOrBarCode)
        {
            var model = _query.Find<StocktakingPlan>(planId);
            var rows =new List<StocktakingPlanItemDto>();
            var difference = false;
            if (showDifference.HasValue)
            {
                difference = showDifference.Value;
            }
            // 没合并过盘点，不让看明细
            if (model.Status == StocktakingPlanStatus.FirstInventory)
            {
                return Json(new { success = true, data = rows, total = rows.Count });
            }
            else
            {
                rows = _stocktakingPlanQuery.GetDetails(page, planId, from, to, difference, productCodeOrBarCode).ToList();
                return Json(new { success = true, data = rows, total = rows.Count });
            }           
        }

        public ActionResult Create()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
            var dic = typeof(StocktakingPlanMethod).GetValueToDescription();
            ViewBag.Method = dic;
            return View();
        }
        [HttpPost]
        public ActionResult Create(StocktakingPlanModel model)
        {
            model.EditedBy = _context.CurrentAccount.AccountId;
            model.Editor = _context.CurrentAccount.NickName;
            _stocktakingPlanFacade.Create(model);
            return Json(new { success = true });
        }

        public ActionResult Edit(int id)
        {
            var model = _query.Find<StocktakingPlan>(id);
            var store = _query.Find<Store>(model.StoreId);
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = store.Id;
            ViewBag.StoreName = store.Name;
            var dic = typeof(StocktakingPlanMethod).GetValueToDescription();
            ViewBag.Method = dic;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(StocktakingPlanModel model)
        {
            model.EditedBy = _context.CurrentAccount.AccountId;
            model.Editor = _context.CurrentAccount.NickName;
            _stocktakingPlanFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult StartPlan(int id)
        {
            _stocktakingPlanFacade.StartPlan(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
        public JsonResult MergeDetial(int id)
        {
            _stocktakingPlanFacade.MergeDetial(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
        public JsonResult EndPlan(int id,string loginPassword)
        {
            _stocktakingPlanFacade.EndPlan(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName, loginPassword);
            return Json(new { success = true });
        }


	}
}