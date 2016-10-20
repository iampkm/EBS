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
    public class BrandController : Controller
    {      
         IQuery _query;
        IBrandQuery _brandQuery;
        IBrandFacade _brandFacade;
        public BrandController(IQuery query,IBrandQuery brandQuery, IBrandFacade brandFacade)
        {
            this._query = query;
            this._brandQuery = brandQuery;
            this._brandFacade = brandFacade;
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult LoadData(Pager page, string name)
        {  
            var rows= _brandQuery.GetPageList(page, name);
            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {           
            return View();
        }
        [HttpPost]
        public JsonResult Create(string name)
        {
            _brandFacade.Create(name);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Brand>(id); 
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(int id,string name)
        {
            _brandFacade.Edit(id,name);
            return Json(new { success = true });
        }

        public JsonResult Delete(string ids)
        {
            _brandFacade.Delete(ids);
            return Json(new { success = true });
        }
	}
}