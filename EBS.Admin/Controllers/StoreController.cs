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
    public class StoreController : Controller
    {
         IQuery _query;
        IStoreQuery _storeQuery;
        IStoreFacade _storeFacade;
        IContextService _context;
        public StoreController(IContextService context, IQuery query, IStoreQuery storeQuery, IStoreFacade storeFacade)
        {
            this._query = query;
            this._storeQuery = storeQuery;
            this._storeFacade = storeFacade;
            this._context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadData(Pager page, string name, string code)
        {
            // 显示当前用户有权限查看的门店信息
            var rows = _storeQuery.GetPageList(page, name,code, _context.CurrentAccount.CanViewStores);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {           
            return View();
        }
        [HttpPost]
        public JsonResult Create(StoreModel model)
        {
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedOn = DateTime.Now;
            _storeFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Store>(id);
            ViewBag.AreaName = _query.Find<Area>(n => n.Id == model.AreaId).FullName;
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(StoreModel model)
        {
            model.CreatedBy = _context.CurrentAccount.AccountId;
            model.CreatedOn = DateTime.Now;
            _storeFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(string ids)
        {
            _storeFacade.Delete(ids);
            return Json(new { success = true });
        }
        public ActionResult EditLicense()
        {
            ViewBag.View =_context.CurrentAccount.ShowSelectStore()?"true":"false";
            ViewBag.StoreId = _context.CurrentAccount.StoreId;
            return View();
        }

        [HttpPost]
        public JsonResult EditLicense(int storeId,string license)
        {
            _storeFacade.EditLicense(storeId, license);
            return Json(new { success = true });
        }

        public string LoadStore()
        {
            // 按区域和门店，加载二级树形结构
            var row = this._storeQuery.LoadStore(_context.CurrentAccount.CanViewStores).ToArray();
            return JsonConvert.SerializeObject(row);
        }
    }
}