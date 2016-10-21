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
            var treeNodes = _categoryQuery.GetCategoryTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            ViewBag.Tree = tree;
            var brands= _query.FindAll<Brand>();
            ViewBag.Brands = brands;
            return View();
        }

        public JsonResult LoadData(Pager page, string name)
        {
            var rows = _productQuery.GetPageList(page, name);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }
	}
}