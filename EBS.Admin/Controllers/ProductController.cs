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
    public class ProductController : Controller
    {        
        IQuery _query;
        IProductQuery _productQuery;
        IProductFacade _productFacade;
        ICategoryQuery _categoryQuery;

        public ProductController(IQuery query, IProductQuery productQuery, IProductFacade productFacade, ICategoryQuery categoryQuery)
        {
            this._query = query;
            this._productQuery = productQuery;
            this._productFacade = productFacade;
            this._categoryQuery = categoryQuery;
        }
        public ActionResult Index()
        {
            LoadCategory();
            LoadBrand();
            return View();
        }

        public JsonResult LoadData(Pager page, string name)
        {
            var rows = _productQuery.GetPageList(page, name);

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
            LoadBrand();

            //显示当前品类 规格名
          

            return View();
        }

        public JsonResult LoadProductSpecification(string categoryId)
        {
           var specs = _query.FindAll<ProductSpecification>(n=>n.CategoryId == categoryId);
            List<ProductAttrbuteValue> list = new List<ProductAttrbuteValue>();
            foreach (var spec in specs)
            {
                var values = _query.FindAll<ProductSpecificationOption>(n => n.ProductSpecificationId == spec.Id);
                list.Add(new ProductAttrbuteValue() { Ppecification = spec, Options = values.ToList() });
            }

            return Json(new { success = true, productSpecifications = specs,options = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadProductSpecificationOptions(int id)
        {
            var values = _query.FindAll<ProductSpecificationOption>(n=>n.ProductSpecificationId == id);
            return Json(new { success = true,  productSpecificationOptions = values }, JsonRequestBehavior.AllowGet);
        }
    }
}