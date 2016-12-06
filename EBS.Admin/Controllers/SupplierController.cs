﻿using System;
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
        ICategoryQuery _categoryQuery;     
        public SupplierController(IQuery query, ISupplierQuery supplierQuery, ISupplierFacade supplierFacade,IAreaQuery areaQuery,ICategoryQuery _categoryQuery)
        {
            this._query = query;
            this._supplierQuery = supplierQuery;
            this._supplierFacade = supplierFacade;
            this._areaQuery = areaQuery;
            this._categoryQuery = _categoryQuery;
        }
        public ActionResult Index()
        {           
            return View();
        }

        public JsonResult LoadData(Pager page, string name, string code)
        {
            var rows = _supplierQuery.GetPageList(page, name,code);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }        

        public ActionResult Create()
        {
            ViewBag.SupplierTypes = _supplierQuery.GetSupplierType();
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
            ViewBag.SupplierTypes = _supplierQuery.GetSupplierType();
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

        public JsonResult CheckCode(string code)
        {
            var result = _query.Exists<Supplier>(n => n.Code == code);
            return Json(new { success = true,data = !result });
        }

        public JsonResult GetSupplierByCode(string code)
        {
           var rows=  _query.FindAll<Supplier>(n => n.Code.Like(code+"%"));
            return Json(rows) ;
           
           // return Json(new { success = true, data = rows });
        }

        #region 供应商商品维护
        /// <summary>
        /// 供应商商品
        /// </summary>
        /// <returns></returns>
        public ActionResult Product()
        {
            LoadCategory();
            return View();
        }

        public ActionResult ImportProduct()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ImportProduct(int supplierId,string products)
        {
            _supplierFacade.ImportProduct(supplierId, products);
            return Json(new { success = true });
        }

        private void LoadCategory()
        {
            var treeNodes = _categoryQuery.GetCategoryTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            ViewBag.Tree = tree;
        }

        #endregion
    }
}