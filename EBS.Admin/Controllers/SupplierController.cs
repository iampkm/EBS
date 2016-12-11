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
    public class SupplierController : Controller
    {
         IQuery _query;
        ISupplierQuery _supplierQuery;
        ISupplierFacade _supplierFacade;
        IAreaQuery _areaQuery;
        ICategoryQuery _categoryQuery;
        IContextService _context;
        public SupplierController(IContextService context,IQuery query, ISupplierQuery supplierQuery, ISupplierFacade supplierFacade,IAreaQuery areaQuery,ICategoryQuery _categoryQuery)
        {
            this._context = context;
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

        //public string LoadChildArea()
        //{
        //    var treeNodes = _areaQuery.GetTree();
        //    var tree = JsonConvert.SerializeObject(treeNodes);
        //    return tree;
        //}
        public ActionResult Create()
        {
            ViewBag.SupplierTypes = _supplierQuery.GetSupplierType();
            return View();
        }

        [HttpPost]
        public JsonResult Create(SupplierModel model)
        {
            model.editedBy = _context.CurrentAccount.AccountId;
            _supplierFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Supplier>(id);
            ViewBag.SupplierTypes = _supplierQuery.GetSupplierType();
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(SupplierModel model)
        {
            model.editedBy = _context.CurrentAccount.AccountId;
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

        public JsonResult LoadSupplierProducts(Pager page, string name,string codeOrBarCode,string categoryId,int brandId,string supplierIds)
        {
            var rows = _supplierQuery.QuerySupplierProducts(page, name,codeOrBarCode,categoryId, brandId,supplierIds);

            return Json(new { success = true, data = rows, total = rows.Count() });
        }

        public ActionResult ImportProduct()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ImportProduct(string items)
        {
            _supplierFacade.ImportProduct(items,this._context.CurrentAccount.AccountId);
            return Json(new { success = true });
        }

        public JsonResult QueryProduct(string productCodePriceInput)
        {
            var rows = _supplierQuery.GetSupplierProducts(productCodePriceInput);
            return Json(new { success = true, data = rows });
        }

        public JsonResult QuerySupplierProductCompare(int supplierId1,int supplierId2,string productIds)
        {
            var rows = _supplierQuery.QuerySupplierProductCompare( supplierId1, supplierId2,  productIds);
            return Json(new { success = true, data = rows });
        }
        /// <summary>
        /// 标记供货
        /// </summary>
        /// <param name="markId"></param>
        /// <param name="unMarkId"></param>
        /// <returns></returns>
        public JsonResult MarkWaitSuppply(int markId, int unMarkId)
        {
            _supplierFacade.MarkWaitSuppply(markId,unMarkId,_context.CurrentAccount.AccountId);
            return Json(new { success = true});
        }
        /// <summary>
        /// 取消标记供货
        /// </summary>
        /// <param name="markId"></param>
        /// <returns></returns>
        public JsonResult UnMarkWaitSuppply(int markId, int unMarkId)
        {
            _supplierFacade.UnMarkWaitSuppply(markId, unMarkId, _context.CurrentAccount.AccountId);
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