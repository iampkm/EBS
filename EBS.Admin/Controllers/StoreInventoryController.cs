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
using EBS.Infrastructure.File;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
namespace EBS.Admin.Controllers
{
    [Permission]
    public class StoreInventoryController : Controller
    {
        IQuery _query;
        IStoreInventoryQuery _storeInventoryQuery;
       // IStoreInventoryFacade _StoreInventoryFacade;
        IAreaQuery _areaQuery;
        IContextService _context;
        ICategoryQuery _categoryQuery;
        IExcel _excelService;
        public StoreInventoryController(IContextService contextService, IQuery query, IStoreInventoryQuery storeInventoryQuery, IAreaQuery areaQuery,ICategoryQuery categoryQuery ,IExcel iexcel)
        {
            this._query = query;
            this._storeInventoryQuery = storeInventoryQuery;
            this._areaQuery = areaQuery;
            this._context = contextService;
            this._categoryQuery = categoryQuery;
            this._excelService = iexcel;
        }

        public ActionResult Index()
        {
            SetUserAuthention();
            LoadCategory();
            return View();
        }       

        public ActionResult LoadData(Pager page, SearchStoreInventory condition)
        {
            if (string.IsNullOrEmpty(condition.StoreId)||condition.StoreId=="0") { condition.StoreId = _context.CurrentAccount.CanViewStores; }
            var rows = _storeInventoryQuery.GetPageList(page, condition);
            if (page.toExcel)
            {
                var data = _excelService.WriteToExcelStream(rows.ToList(), ExcelVersion.Above2007, false, true).ToArray();
                var fileName = string.Format("库存_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                return File(data, "application/ms-excel", fileName);
            }
            return Json(new { success = true, data = rows, total = page.Total,sum = page.SumColumns });
        }

        public ActionResult History()
        {
            SetUserAuthention();
            DateTime now = DateTime.Now;
            DateTime monthBegin = new DateTime(now.Year, now.Month, 1);
            DateTime monthEnd = monthBegin.AddMonths(1).AddDays(-1);
            ViewBag.BeginDate = monthBegin.ToString("yyyy-MM-dd");
            ViewBag.EndDate = monthEnd.ToString("yyyy-MM-dd");
            return View();
        }
        public JsonResult LoadDataHistory(Pager page, SearchStoreInventoryHistory condition)
        {
            if (string.IsNullOrEmpty(condition.StoreId) || condition.StoreId == "0") { condition.StoreId = _context.CurrentAccount.CanViewStores; }
            var rows = _storeInventoryQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }
        public ActionResult Batch()
        {
            SetUserAuthention();
            LoadCategory();
            return View();
        }
        public JsonResult LoadDataBatch(Pager page, SearchStoreInventoryBatch condition)
        {
            if (string.IsNullOrEmpty(condition.StoreId) || condition.StoreId == "0") { condition.StoreId = _context.CurrentAccount.CanViewStores; }
            var rows = _storeInventoryQuery.GetPageList(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        /// <summary>
        /// 单品查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Product()
        {
            SetUserAuthention();
            return View();
        }

        public JsonResult QueryProduct(SearchStoreInventory condition)
        {
            var rows = _storeInventoryQuery.QueryProduct(condition).ToList();           
            return Json(new { success = true, data = rows, total = rows.Count });
        }

        private void LoadCategory()
        {
            var treeNodes = _categoryQuery.GetCategoryTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            ViewBag.Tree = tree;
        }

        private void SetUserAuthention()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
        }
    }
}