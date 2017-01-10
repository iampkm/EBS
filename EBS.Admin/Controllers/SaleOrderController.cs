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
    public class SaleOrderController : Controller
    {
        ISaleOrderQuery _saleOrderQuery;
        IContextService _context;

        public SaleOrderController(IContextService contextService, ISaleOrderQuery saleQuery)
        {
            _saleOrderQuery = saleQuery;
            this._context = contextService;
        }
        //
        // GET: /SaleOrder/
        public ActionResult Index()
        {
            ViewBag.today = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
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
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            return View();
        }

        public JsonResult QuerySaleOrderItems(Pager page, SearchSaleOrder condition)
        {
            var rows = _saleOrderQuery.QuerySaleOrderItems(page, condition);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        /// <summary>
        /// 营业额汇总核对
        /// </summary>
        /// <returns></returns>
        public ActionResult SaleSummary()
        {
            ViewBag.today = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
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
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
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
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            return View();
        }

        public JsonResult QuerySaleSync(Pager page, DateTime saleDate)
        {
            var rows = _saleOrderQuery.QuerySaleSync(page, saleDate);

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
    }
}