using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Application;
using EBS.Application.DTO;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using System.Dynamic;
using EBS.Infrastructure.Extension;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Admin.Services;
namespace EBS.Admin.Controllers
{
     [Permission]
    public class MenuController : Controller
    {
        IQuery _query;
        IMenuQuery _menuQuery;
        IMenuFacade _menuFacade;

        public MenuController(IQuery query,IMenuQuery menuQuery, IMenuFacade menuFacade)
        {
            this._query = query;
            this._menuQuery = menuQuery;
            this._menuFacade = menuFacade;
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult LoadData(Pager page, string name)
        {            
            var rows= _menuQuery.GetList(page, name);
            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadMenu()
        {
            var tree= _menuQuery.LoadMenuTree();
            return Json(new { success = true, data = tree }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {           
            var dic = typeof(MenuUrlType).GetValueToDescription();
            ViewBag.menutypes = dic;
            return View();
        }
        [HttpPost]
        public JsonResult Create(MenuModel model)
        {
            _menuFacade.Create(model);
            return Json(new { success = true });
        }       
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Menu>(id);
            var parentModel = _query.Find<Menu>(model.ParentId);
            var parentName = "";
            if (parentModel != null)
            {
                parentName = parentModel.Name;
            }
            ViewBag.parentName = parentName;
            // 枚举
            var dic = typeof(MenuUrlType).GetValueToDescription();
            ViewBag.menutypes = dic;
            return View(model);
        }

         [HttpPost]
        public JsonResult Edit(MenuModel model)
        {
            _menuFacade.Edit(model);
            return Json(new { success = true });
        }

         public JsonResult Delete(string ids)
         {
             _menuFacade.Delete(ids);
             return Json(new { success = true });
         }


    }
}