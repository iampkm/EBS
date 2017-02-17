using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Admin.Services;
using EBS.Infrastructure;
using Dapper.DBContext;
using EBS.Domain.Entity;
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
        IQuery _query;
        public ReportController(IContextService contextService, IReportQuery reportQuery,IQuery query)
        {
            _reportQuery = reportQuery;
            this._context = contextService;
            _query = query;
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
           // var today = new DateTime(2017, 1, 1);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">门店id</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult PurchaseSaleInventoryDetail(int id,string startDate,string endDate)
        {
            var model = _query.Find<Store>(id);
            ViewBag.StoreId = id;
            ViewBag.StoreName = model.Name;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            return View();
        }

        public JsonResult QueryPurchaseSaleInventoryDetail(Pager page, PurchaseSaleInventoryDetailSearch condition)
        {
            var rows = _reportQuery.QueryPurchaseSaleInventoryDetail(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }
    }
}