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
        public StoreController(IQuery query, IStoreQuery storeQuery, IStoreFacade storeFacade)
        {
            this._query = query;
            this._storeQuery = storeQuery;
            this._storeFacade = storeFacade;
         
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadData(Pager page, string name)
        {
            var rows = _storeQuery.GetPageList(page, name);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {           
            return View();
        }
        [HttpPost]
        public JsonResult Create(StoreModel model)
        {
            _storeFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Store>(id);           
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(StoreModel model)
        {
            _storeFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(string ids)
        {
            _storeFacade.Delete(ids);
            return Json(new { success = true });
        }
	}
}