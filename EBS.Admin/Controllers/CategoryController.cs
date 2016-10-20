using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Infrastructure.Extension;
using Dapper.DBContext;
using EBS.Query;
using EBS.Query.DTO;
using Newtonsoft.Json;
using EBS.Application.DTO;
using EBS.Admin.Services;
using EBS.Application;
namespace EBS.Admin.Controllers
{
    public class CategoryController : Controller
    {    
        IQuery _query;
        ICategoryQuery _categoryQuery;
        ICategoryFacade _categoryFacade;
        public CategoryController(IQuery query, ICategoryQuery categoryQuery,ICategoryFacade categoryFacade)
        {
            this._query = query;
            this._categoryQuery = categoryQuery;
            this._categoryFacade = categoryFacade;
        }
        public ActionResult Index()
        {
            var treeNodes = _categoryQuery.GetCategoryTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            ViewBag.Tree = tree;
            return View();
        }

        public JsonResult Create(string parentId, string text)
        {
            var id = _categoryFacade.Create(parentId, text);
            return Json(new { success = true,id =id  });
        }

        public JsonResult Edit(string id, string text)
        {
            _categoryFacade.Edit(id, text);
            return Json(new { success = true });
        }

        public JsonResult Remove(string id)
        {
            _categoryFacade.Delete(id);
            return Json(new { success = true });
        }
	}
}