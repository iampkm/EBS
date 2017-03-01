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
using EBS.Domain.Service;
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
            ViewBag.StoreId = _context.CurrentAccount.StoreId == 0 ? "" : _context.CurrentAccount.StoreId.ToString();
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.IsAdmin = _context.CurrentAccount.AccountId == 1 ? "true" : "false";
            ViewBag.Years = _reportQuery.GetYears();
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
        public ActionResult PurchaseSaleInventoryDetail(int id,string yearMonth)
        {
            var model = _query.Find<Store>(id);
            ViewBag.StoreId = id;
            ViewBag.StoreName = model.Name;          
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.IsAdmin = _context.CurrentAccount.AccountId == 1 ? "true" : "false";
            ViewBag.Year = yearMonth.Substring(0, 4);
            ViewBag.Month = yearMonth.Substring(yearMonth.Length-2, 2);
            ViewBag.Years = _reportQuery.GetYears();
            return View();
        }

        public JsonResult QueryPurchaseSaleInventoryDetail(Pager page, PurchaseSaleInventoryDetailSearch condition)
        {
            var rows = _reportQuery.QueryPurchaseSaleInventoryDetail(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public JsonResult Generate(int year, int month)
        {            
            var selectDate = new DateTime(year, month, 1);
            PurchaseSaleInventoryTask task = new PurchaseSaleInventoryTask(selectDate);
            task.Execute();
            return Json(new { success = true });
        }

        public JsonResult GenerateDetail(int year, int month)
        {
            var selectDate = new DateTime(year, month, 1);
            PurchaseSaleInventoryDetailTask task = new PurchaseSaleInventoryDetailTask(selectDate);
            task.Execute();
            return Json(new { success = true });
        }

       
    }
}