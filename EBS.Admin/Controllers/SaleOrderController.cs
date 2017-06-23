using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Admin.Services;
using Newtonsoft.Json;
using EBS.Application;
namespace EBS.Admin.Controllers
{
    [Permission]
    public class SaleOrderController : Controller
    {
        ISaleOrderQuery _saleOrderQuery;
        IContextService _context;
        ICategoryQuery _categoryQuery;
        ISaleReportFacade _saleReportFacade;
        public SaleOrderController(IContextService contextService, ISaleOrderQuery saleQuery,ICategoryQuery categoryQuery,ISaleReportFacade saleReportFacade)
        {
            _saleOrderQuery = saleQuery;
            this._context = contextService;
             this._categoryQuery = categoryQuery;
             _saleReportFacade = saleReportFacade;
        }
        //
        // GET: /SaleOrder/
        public ActionResult Index()
        {
           // ViewBag.today = DateTime.Now.ToString("yyyy-MM-dd");
            SetUserAuthention();
            return View();
        }

        public ActionResult Details(int id)
        {
            var model = _saleOrderQuery.GetById(id);
            return View(model);
        }

        /// <summary>
        /// 收银流水
        /// </summary>
        /// <returns></returns>
        public ActionResult SaleOrderItems()
        {
            SetUserAuthention();
            return View();
        }

        public JsonResult QuerySaleOrderItems(Pager page, SearchSaleOrder condition)
        {
            var rows = _saleOrderQuery.QuerySaleOrderItems(page, condition);

            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }

        /// <summary>
        /// 营业额汇总核对
        /// </summary>
        /// <returns></returns>
        public ActionResult SaleSummary()
        {
            ViewBag.today = DateTime.Now.ToString("yyyy-MM-dd");
            SetUserAuthention();
            return View();
        }

        public JsonResult QuerySaleSummary(Pager page, SearchSaleOrder condition)
        {
            var rows = _saleOrderQuery.QuerySaleSummary(page, condition);

            return Json(new { success = true, data = rows, total = page.Total, sum = page.SumColumns });
        }

        /// <summary>
        /// 收银防损核对
        /// </summary>
        /// <returns></returns>
        public ActionResult SaleCheck()
        {
            ViewBag.today = DateTime.Now.ToString("yyyy-MM-dd");
            SetUserAuthention();
            return View();
        }

        public JsonResult QuerySaleCheck(Pager page, SearchSaleOrder condition)
        {
            var rows = _saleOrderQuery.QuerySaleCheck(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }


        public ActionResult SaleSync()
        {
            ViewBag.saleDate = DateTime.Now.ToString("yyyy-MM-dd");
            SetUserAuthention();
            return View();
        }

        public JsonResult QuerySaleSync(Pager page, DateTime saleDate,string storeId)
        {
            var rows = _saleOrderQuery.QuerySaleSync(page, saleDate,storeId);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult SaleList(int id, int status, int orderType)
        {
            ViewBag.workScheduleId = id;
            ViewBag.status = status;
            ViewBag.orderType = orderType;
            return View();
        }

        public JsonResult QuerySaleOrder(Pager page,int workScheduleId, int status, int orderType)
        {
            var rows = _saleOrderQuery.QuerySaleOrder(page,workScheduleId, status, orderType);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        private void SetUserAuthention()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
        }
        /// <summary>
        /// 单品销售汇总：门店员工用
        /// </summary>
        /// <returns></returns>
        public ActionResult SingleProductSale()
        {
            SetUserAuthention();
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        public JsonResult QuerySingleProductSale(Pager page, SearchSingleProductSale condition)
        {
            var rows = _saleOrderQuery.QuerySingleProductSale(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult SaleReport()
        {
            SetUserAuthention();
            ViewBag.Today = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            LoadCategory();
            ViewBag.IsAdmin = _context.CurrentAccount.AccountId == 1 ? "true" : "false";
            return View();
        }

        private void LoadCategory()
        {
            var treeNodes = _categoryQuery.GetCategoryTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            ViewBag.Tree = tree;
        }

        public JsonResult QuerySaleReport(Pager page, SearchSaleReport condition)
        {

            var rows = _saleOrderQuery.QuerySaleReport(page, condition);

            return Json(new { success = true, data = rows, total = page.Total,sum = page.SumColumns });
        }

        public JsonResult GenerateSaleReport(DateTime beginDate, DateTime endDate)
        {
            _saleReportFacade.Create(beginDate, endDate);
            return Json(new { success = true });
        }
    }
}