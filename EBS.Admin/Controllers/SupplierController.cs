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
    public class SupplierController : Controller
    {
         IQuery _query;
        ISupplierQuery _supplierQuery;
        ISupplierFacade _supplierFacade;
        IAreaQuery _areaQuery;       
        public SupplierController(IQuery query, ISupplierQuery supplierQuery, ISupplierFacade supplierFacade,IAreaQuery areaQuery)
        {
            this._query = query;
            this._supplierQuery = supplierQuery;
            this._supplierFacade = supplierFacade;
            this._areaQuery = areaQuery;         
        }
        public ActionResult Index()
        {           
            return View();
        }

        public JsonResult LoadData(Pager page, string name)
        {
            var rows = _supplierQuery.GetPageList(page, name);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
           // ViewBag.Tree = LoadChildArea();
            return View();
        }

        public string LoadChildArea()
        {
            var treeNodes = _areaQuery.GetTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            return tree;
        }

        [HttpPost]
        public JsonResult Create(SupplierModel model)
        {
            _supplierFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Supplier>(id);
            ViewBag.areaName = _query.Find<Area>(model.AreaId).FullName;
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(SupplierModel model)
        {
            _supplierFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(string ids)
        {
            _supplierFacade.Delete(ids);
            return Json(new { success = true });
        }
	}
}