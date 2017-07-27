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
using EBS.Application;
using Newtonsoft.Json;
using EBS.Infrastructure.File;
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
        IPurchaseSaleInventoryFacade _purchaseSaleInventoryFacade;
        ICategoryQuery _categoryQuery;
        IExcel _excelService;
        public ReportController(IContextService contextService, IReportQuery reportQuery,IQuery query, IPurchaseSaleInventoryFacade psiFacade, ICategoryQuery categoryQuery, IExcel excelService)
        {
            _reportQuery = reportQuery;
            this._context = contextService;
            _query = query;
            _purchaseSaleInventoryFacade = psiFacade;
            this._categoryQuery = categoryQuery;
            this._excelService = excelService;
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

        public ActionResult QueryPurchaseSaleInventorySummary(Pager page, PurchaseSaleInventorySearch condition)
        {
            var rows = _reportQuery.QueryPurchaseSaleInventorySummary(page, condition);
            if (page.toExcel)
            {
                var data = _excelService.WriteToExcelStream(rows.ToList(), ExcelVersion.Above2007, false, true).ToArray();
                var fileName = string.Format("进销存_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                return File(data, "application/ms-excel", fileName);
            }
            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">门店id</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult PurchaseSaleInventoryDetail()
        {
            SetUserAuthention();
            LoadCategory();
            ViewBag.IsAdmin = _context.CurrentAccount.AccountId == 1 ? "true" : "false";
            ViewBag.Year = DateTime.Now.Year;
            ViewBag.Month = DateTime.Now.Month.ToString().PadLeft(2, '0'); 
            ViewBag.Years = _reportQuery.GetYears();
            return View();
        }

        public ActionResult QueryPurchaseSaleInventoryDetail(Pager page, PurchaseSaleInventoryDetailSearch condition)
        {
            if (string.IsNullOrEmpty(condition.StoreId) || condition.StoreId == "0") { condition.StoreId = _context.CurrentAccount.CanViewStores; }
            var rows = _reportQuery.QueryPurchaseSaleInventoryDetail(page, condition);
            if (page.toExcel)
            {
                var data = _excelService.WriteToExcelStream(rows.ToList(), ExcelVersion.Above2007, false, true).ToArray();
                var fileName = string.Format("进销存明细_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                return File(data, "application/ms-excel", fileName);
            }
            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }

        public JsonResult Generate(int year, int month)
        {            
            var selectDate = new DateTime(year, month, 1);
            _purchaseSaleInventoryFacade.Generate(selectDate);
            return Json(new { success = true });
        }

        public JsonResult GenerateDetail(int year, int month)
        {
            var selectDate = new DateTime(year, month, 1);
            _purchaseSaleInventoryFacade.GenerateDetail(selectDate);
            return Json(new { success = true });
        }

        private void SetUserAuthention()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
        }

        private void LoadCategory()
        {
            var treeNodes = _categoryQuery.GetCategoryTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            ViewBag.Tree = tree;
        }
    }
}