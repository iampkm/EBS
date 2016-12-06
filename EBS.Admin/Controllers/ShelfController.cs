using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper.DBContext;
using EBS.Query;
using EBS.Query.DTO;
using Newtonsoft.Json;
using EBS.Application.DTO;
using EBS.Admin.Services;
using EBS.Application;
namespace EBS.Admin.Controllers
{
    public class ShelfController : Controller
    {    
          IQuery _query;
        IShelfQuery _shelfQuery;
        IShelfFacade _shelfFacade;
        IContextService _context;
        public ShelfController(IContextService context,IQuery query, IShelfQuery shelfQuery, IShelfFacade shelfFacade)
        {
            this._context = context;
            this._query = query;
            this._shelfQuery = shelfQuery;
            this._shelfFacade = shelfFacade;
        }
        public ActionResult Index()
        {
            ViewBag.Tree = "[]";
            if (_context.CurrentAccount.StoreId > 0)
            {
                var treeNodes = _shelfQuery.GetShelfTree(_context.CurrentAccount.StoreId);
                var tree = JsonConvert.SerializeObject(treeNodes);
                ViewBag.Tree = tree;
            }
           
            return View();
        }

        public JsonResult GetShelfTree(int storeId)
        {
            var treeNodes = _shelfQuery.GetShelfTree(storeId);
           // var tree = JsonConvert.SerializeObject(treeNodes);
            return Json(new { success = true, data = treeNodes });
        }

        public JsonResult CreateShelf(int storeId,string name,string code)
        {
           var shelfCode=  _shelfFacade.CreateShelf(storeId, name, code);
           var node = _shelfQuery.QueryShelf(storeId, shelfCode);
           return Json(new { success = true, data = node });
        }
        public JsonResult CreateLayer(int shelfId)
        {
            var layerCode = _shelfFacade.CreateShelfLayer(shelfId);
            var node = _shelfQuery.QueryShelfLayer(shelfId, layerCode);
            return Json(new { success = true, data = node });
        }
        public JsonResult CreateProduct(int storeId, int shelfLayerId, string productCodeOrBarCode)
        {
            var layerProductCode = _shelfFacade.CreateProduct(storeId, shelfLayerId, productCodeOrBarCode);
            var node = _shelfQuery.QueryProduct(shelfLayerId, layerProductCode);
            return Json(new { success = true, data = node });
        }

        public JsonResult Edit(string id, string name)
        {
           // _shelfFacade.Edit(id, text);
            return Json(new { success = true });
        }

        public JsonResult Remove(string id)
        {
           // _shelfFacade.Delete(id);
            return Json(new { success = true });
        }
	}
}