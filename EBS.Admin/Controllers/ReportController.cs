using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Admin.Services;
namespace EBS.Admin.Controllers
{
    [Permission]
    /// <summary>
    /// 报表
    /// </summary>
    public class ReportController : Controller
    {
        IReportQuery _reportQuery;
        IContextService _context;

        public ReportController(IContextService contextService, IReportQuery reportQuery)
        {
            _reportQuery = reportQuery;
            this._context = contextService;
        }
           
        /// <summary>
        /// 收银流水
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchaseSaleInventory()
        {
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            var today = DateTime.Now;
            var firstDay = new DateTime(today.Year, today.Month, 1);
            ViewBag.StartDate = firstDay.ToString("yyyy-MM-dd");
            ViewBag.EndDate = firstDay.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            return View();
        }

        public JsonResult QueryPurchaseSaleInventorySummary(Pager page, PurchaseSaleInventorySearch condition)
        {
            var rows = _reportQuery.QueryPurchaseSaleInventorySummary(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        
    }
}