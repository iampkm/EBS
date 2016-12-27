using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Application;
using EBS.Application.DTO;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Query;
using EBS.Query.DTO;
using Newtonsoft.Json;
using EBS.Admin.Services;
namespace EBS.Admin.Controllers
{
    /// <summary>
    /// 门店调拨单
    /// </summary>
    public class TransferOrderController : Controller
    {
        IContextService _context;
        ITransferOrderQuery _transaferQuery;
        public TransferOrderController(IContextService context,ITransferOrderQuery transaferOrderQuery)
        {
            _context = context;
            _transaferQuery = transaferOrderQuery;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AuditIndex()
        {
            return View();
        }

        public JsonResult LoadData(Pager page, SearchTransferOrder conditon)
        {
            var rows = _transaferQuery.GetPageList(page, conditon);

            return Json(new { success = true, data = rows, total = page.Total });
        }
	}
}