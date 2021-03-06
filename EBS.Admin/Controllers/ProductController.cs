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
using EBS.Infrastructure.Log;
using EBS.Infrastructure.Extension;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.IO;

namespace EBS.Admin.Controllers
{
    [Permission]
    public class ProductController : Controller
    {        
        IQuery _query;
        IProductQuery _productQuery;
        IProductFacade _productFacade;
        ICategoryQuery _categoryQuery;
        IContextService _context;
        ILogger _log;

        public ProductController(IContextService context,IQuery query, IProductQuery productQuery, IProductFacade productFacade, ICategoryQuery categoryQuery,ILogger log)
        {
            this._query = query;
            this._productQuery = productQuery;
            this._productFacade = productFacade;
            this._categoryQuery = categoryQuery;
            this._context = context;
            _log = log;
        }
        public ActionResult Index()
        {
            LoadCategory();
            return View();
        }

        public JsonResult LoadData(Pager page, string name, string codeOrBarCode, string categoryId, int brandId)
        {
            var rows = _productQuery.GetPageList(page, name, codeOrBarCode,categoryId,brandId);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        private void LoadCategory()
        {
            var treeNodes = _categoryQuery.GetCategoryTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            ViewBag.Tree = tree;
        }
        private void LoadBrand()
        {
            var brands = _query.FindAll<Brand>();
            ViewBag.Brands = brands;
        }

        public ActionResult Create()
        {
            
            LoadCategory();
            //显示当前品类 规格名
            return View();
        }
        [HttpPost]
        public JsonResult Create(ProductModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            this._productFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {           
            LoadCategory();

            //显示当前品类 规格名
            var model = _query.Find<Product>(id);
            ViewBag.categoryName = _query.Find<Category>(model.CategoryId).Name;
            ViewBag.brandName = _query.Find<Brand>(model.BrandId).Name;
            return View(model);
        }
        [HttpPost]
        public JsonResult Edit(ProductModel model)
        {
            model.UpdatedBy = _context.CurrentAccount.AccountId;
            this._productFacade.Edit(model);
            return Json(new { success = true });
        }

        //public JsonResult PublishToggle(string ids, bool isPublish)
        //{
        //    this._productFacade.PublishToggle(ids, isPublish);
        //    return Json(new { success = true });
        //}

        public JsonResult Import(string productsInput)
        {
            var error= this._productFacade.Import(productsInput,_context.CurrentAccount.AccountId);
            return Json(new { success = true, data = error });
        }

        public ActionResult PriceTag()
        {
            return View();
        }

        public ActionResult PrintPriceTag(string ids)
        {
            var data = _productQuery.QueryProductPriceTagList(ids);
            ViewBag.NickName = _context.CurrentAccount.NickName;
            return PartialView("PrintPriceTag", data);
        }

        public JsonResult QueryProduct(string productCodeOrBarCode)
        {
            var model = _productQuery.QueryPriceTag(productCodeOrBarCode);
            if (model == null)
            {
                throw new Exception("商品不存在");
            }
            return Json(new { success = true, data = model });
        }


        public JsonResult GenerateBarCode()
        {
            var barCode= _productQuery.GenerateBarCode();
            return Json(new { success = true, data = barCode });
        }

        public ActionResult PriceCheck()
        {
            return View();
        }

        public JsonResult QueryContractProductNoSalePrice(string productCodeOrBarCode)
        {
            var rows = _productQuery.QueryContractProductNoSalePrice(productCodeOrBarCode);
            return Json(new { success = true, data = rows, total = rows.Count() });
        }


            #region 商品SKU 规格设计使用，已经作废
            //public ActionResult Create2() {
            //    LoadCategory();
            //    LoadBrand();
            //    //显示当前品类 规格名
            //    return View();
            //}


            //// 按照商品规格模式开发使用
            //public JsonResult LoadProductSpecification(string categoryId)
            //{
            //    var specs = _query.FindAll<ProductSpecification>(n => n.CategoryId == categoryId);
            //    List<ProductAttrbuteValue> list = new List<ProductAttrbuteValue>();
            //    foreach (var spec in specs)
            //    {
            //        var values = _query.FindAll<ProductSpecificationOption>(n => n.ProductSpecificationId == spec.Id);
            //        list.Add(new ProductAttrbuteValue() { Ppecification = spec, Options = values.ToList() });
            //    }

            //    return Json(new { success = true, productSpecifications = specs, options = list }, JsonRequestBehavior.AllowGet);
            //}

            //public JsonResult LoadProductSpecificationOptions(int id)
            //{
            //    var values = _query.FindAll<ProductSpecificationOption>(n => n.ProductSpecificationId == id);
            //    return Json(new { success = true, productSpecificationOptions = values }, JsonRequestBehavior.AllowGet);
            //}
            #endregion
        }
}