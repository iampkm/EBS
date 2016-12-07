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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">选择的节点id</param>
        /// <param name="code">代码</param>
        /// <param name="storeId"></param>
        /// <param name="productCodeOrBarCode"></param>
        /// <returns></returns>
        public JsonResult CreateProduct(int id,string code,int storeId,string productCodeOrBarCode)
        {
            
            if (code.Length == 6)
            {
                // 货架层
                var layerProductCode = _shelfFacade.CreateProduct(storeId, id, productCodeOrBarCode);
                var node = _shelfQuery.QueryProduct(id, layerProductCode);
                return Json(new { success = true, data = node });
            }
            else if (code.Length == 8)
            { 
                //商品 插入模式
                _shelfFacade.InsertBefore(productCodeOrBarCode,id);
            }
           
           //var 
          return Json(new { success = true, data = "" });
        }

        public JsonResult Edit(int id, string name)
        {
            _shelfFacade.EditShelf(id, name);
            return Json(new { success = true });
        }

        public JsonResult Remove(int id,string code)
        {
            _shelfFacade.DeleteAll(id,code);
            return Json(new { success = true });
        }

        public JsonResult QueryShelfProduct(int storeId, string code)
        {
            var rows= _shelfQuery.QueryShelfProduct(storeId, code);
            return Json(new { success = true, data = rows, total = rows.Count() });
        }

        /// <summary>
        /// 打印棚格图 模板
        /// </summary>
        /// <param name="shelfIds"></param>
        /// <returns></returns>
        public PartialViewResult PrintShelfGrid(string shelfIds)
        {
            var models = _shelfQuery.GetShelfGridInfo(shelfIds);
            return PartialView(models);
        }
        /// <summary>
        /// 打印商品盘点表 模板
        /// </summary>
        /// <param name="shelfIds"></param>
        /// <returns></returns>      
        public PartialViewResult PrintShelfStocktaking(string shelfIds)
        {
            var models = _shelfQuery.GetPrintShelfInfo(shelfIds);
            return PartialView(models);
        }
	}
}