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
using EBS.Infrastructure.Extension;
namespace EBS.Admin.Controllers
{
    [Permission]
    /// <summary>
    /// 门店调拨单
    /// </summary>
    public class TransferOrderController : Controller
    {
        IContextService _context;
        ITransferOrderQuery _transaferQuery;
        ITransferOrderFacade _transaferFacade;
        public TransferOrderController(IContextService context,ITransferOrderQuery transaferOrderQuery,ITransferOrderFacade transaferFacade)
        {
            _context = context;
            _transaferQuery = transaferOrderQuery;
            _transaferFacade = transaferFacade;
        }

        public ActionResult Index()
        {
            SetUserAuthention();
            return View();
        }

        public ActionResult Create()
        {            
            ViewBag.CreatedByName = _context.CurrentAccount.NickName;
            return View();
        }

        public ActionResult Detail(int id)
        {
            var model = _transaferQuery.GetById(id);
            return View(model);
        }

        public ActionResult AuditIndex()
        {
            SetUserAuthention();
            return View();
        }

        public ActionResult Finish()
        {
            SetUserAuthention();
            return View();
        }

        private void SetUserAuthention()
        {
            ViewBag.View = _context.CurrentAccount.ShowSelectStore() ? "true" : "false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            ViewBag.StoreName = _context.CurrentAccount.StoreName;
        }

        public JsonResult LoadData(Pager page, SearchTransferOrder conditon)
        {
            var rows = _transaferQuery.GetPageList(page, conditon);

            return Json(new { success = true, data = rows, total = page.Total });
        }

        [HttpPost]
        public JsonResult Create(TransferOrderModel model)
        {
            model.EditBy = _context.CurrentAccount.AccountId;
            model.EditByName = _context.CurrentAccount.NickName;
            _transaferFacade.Create(model);
            return Json(new { success = true, code=model.Code,statusName = model.StatusName,id=model.Id});
        }

        public ActionResult Edit(int id)
        {
            var model = _transaferQuery.GetById(id);
            ViewBag.TransferOrderItems = JsonConvert.SerializeObject(model.Items.ToArray());        
            ViewBag.Status = model.Status.Description();
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(TransferOrderModel model)
        {
            model.EditBy = _context.CurrentAccount.AccountId;
            model.EditByName = _context.CurrentAccount.NickName;
            _transaferFacade.Edit(model);
            return Json(new { success = true });
        }

         [HttpPost]
        public JsonResult Audit(int id)
        {
            _transaferFacade.Audit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }
         [HttpPost]
        public JsonResult Cancel(int id)
        {
            _transaferFacade.Cancel(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
            return Json(new { success = true });
        }

         public JsonResult Submit(int id)
         {
             _transaferFacade.Submit(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
             return Json(new { success = true });
         }

         public JsonResult Reject(int id)
         {
             _transaferFacade.Reject(id, _context.CurrentAccount.AccountId, _context.CurrentAccount.NickName);
             return Json(new { success = true });
         }

         public JsonResult QueryProduct(string productCodeOrBarCode, int storeId)
         {
             var model= _transaferQuery.QueryProduct(productCodeOrBarCode,storeId);
             return Json(new { success = true ,data = model});
         }

         public JsonResult QueryProductBatch(string productCodeOrBarCode, int storeId)
         {
             var rows = _transaferQuery.QueryProductBatch(productCodeOrBarCode, storeId);
             return Json(new { success = true, data = rows });
         }

         public ActionResult Print(int id)
         {
            var model = _transaferQuery.GetById(id);
            
            return PartialView("TransaferOrderTemplate", model);
        }

         public JsonResult GetDetail(int id)
         {
             var model = _transaferQuery.GetById(id);
             return Json(new { success = true, data = model });
         }
	}
}