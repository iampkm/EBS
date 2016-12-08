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
    public class StocktakingPlanController : Controller
    {
        IQuery _query;
        IContextService _context;
        IStocktakingQuery _stocktakingQuery;

        public StocktakingPlanController(IContextService contextService, IQuery query,IStocktakingQuery stocktakingQuery)
        {
            this._query = query;
            this._context = contextService;
            this._stocktakingQuery = stocktakingQuery;

        }

        public ActionResult Index()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.Status = _stocktakingQuery.GetStocktakingPlanStatus();
            return View();
        }

        public JsonResult LoadData(Pager page, SearchStocktakingPlan condition)
        {
            var rows = _stocktakingQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            var dic = typeof(StocktakingPlanMethod).GetValueToDescription();
            ViewBag.Method = dic;
            return View();
        }
        [HttpPost]
        public ActionResult Create(StocktakingPlanModel model)
        {
            model.EditedBy = _context.CurrentAccount.AccountId;
            model.Editor = _context.CurrentAccount.NickName;
            return Json(new { success = true });
        }

        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(StocktakingPlanModel model)
        {
            model.EditedBy = _context.CurrentAccount.AccountId;
            model.Editor = _context.CurrentAccount.NickName;
            return Json(new { success = true });
        }
	}
}