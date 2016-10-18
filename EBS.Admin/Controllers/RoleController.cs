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
using Newtonsoft.Json;
using EBS.Application.DTO;
namespace EBS.Admin.Controllers
{
    public class RoleController : Controller
    {
        IQuery _query;
        IRoleQuery _roleQuery;
        IRoleFacade _roleFacade;
        IMenuQuery _menuQuery;
        public RoleController(IQuery query, IRoleQuery roleQuery, IRoleFacade roleFacade, IMenuQuery menuQuery)
        {
            this._query = query;
            this._roleQuery = roleQuery;
            this._roleFacade = roleFacade;
            this._menuQuery = menuQuery;
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadData(Pager page, string name)
        {
            var rows = _roleQuery.GetPageList(page, name);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            //加载权限资源
            var menus = _menuQuery.LoadMenuTree().Select(m => new { id = m.Id, pId = m.ParentId, name = m.Name }).ToList();
            var tree = JsonConvert.SerializeObject(menus);
            ViewBag.menuTree = tree;
            return View();
        }
        [HttpPost]
        public JsonResult Create(RoleModel model)
        {
            _roleFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Role>(id);
            var items = _query.FindAll<RoleMenu>(n => n.RoleId == id).ToList();
            var menus = _menuQuery.LoadMenuTree().Select(m => new { id = m.Id, pId = m.ParentId, name = m.Name, isChecked = items.Exists(n => n.MenuId == m.Id) }).ToList();
            var tree = JsonConvert.SerializeObject(menus);
            ViewBag.menuTree = tree;
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(RoleModel model)
        {
            _roleFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(string ids)
        {
            _roleFacade.Delete(ids);
            return Json(new { success = true });
        }
    }
}