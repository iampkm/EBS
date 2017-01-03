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
        public ActionResult SaleOrderItems()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            return View();
        }
   
        public JsonResult QuerySaleOrderItems(Pager page, SearchSaleOrder condition)
        {
            var rows = _reportQuery.QuerySaleOrderItems(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        /// <summary>
        /// 营业额汇总核对
        /// </summary>
        /// <returns></returns>
        public ActionResult SaleSummary()
        {
            return View();
        }

        public JsonResult QuerySaleSummary(Pager page, SearchSaleOrder condition)
        {
            var rows = _reportQuery.QuerySaleSummary(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        /// <summary>
        /// 收银防损核对
        /// </summary>
        /// <returns></returns>
        public ActionResult SaleCheck()
        {
            return View();
        }

        public JsonResult QuerySaleCheck(Pager page, SearchSaleOrder condition)
        {
            var rows = _reportQuery.QuerySaleCheck(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }


        public ActionResult SaleSync()
        {
            ViewBag.saleDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            return View();
        }

        public JsonResult QuerySaleSync(Pager page, DateTime saleDate)
        {
            var rows = _reportQuery.QuerySaleSync(page, saleDate);

            return Json(new { success = true, data = rows, total = page.Total });
        }
    }
}