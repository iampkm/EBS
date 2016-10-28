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
using EBS.Query;
using EBS.Query.DTO;
using Newtonsoft.Json;
using EBS.Admin.Services;
namespace EBS.Admin.Controllers
{
    public class PurchaseContractController : Controller
    {
        IQuery _query;
        IPurchaseContractQuery _purchaseContractQuery;
        IPurchaseContractFacade _purchaseContractFacade;
        IAreaQuery _areaQuery;
        public PurchaseContractController(IQuery query, IPurchaseContractQuery purchaseContractQuery, IPurchaseContractFacade purchaseContractFacade, IAreaQuery areaQuery)
        {
            this._query = query;
            this._purchaseContractQuery = purchaseContractQuery;
            this._purchaseContractFacade = purchaseContractFacade;
            this._areaQuery = areaQuery;
        }
        public ActionResult Index()
        {
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;
            return View();
        }

        public JsonResult LoadData(Pager page, string code, string name, int supplierId = 0)
        {
            var rows = _purchaseContractQuery.GetPageList(page, code, name, supplierId);

            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var suppliers = _query.FindAll<Supplier>();
            ViewBag.Suppliers = suppliers;
            ViewBag.CooperateWays = _purchaseContractQuery.GetCooperateWay();
            ViewBag.Stores = _query.FindAll<Store>();
            return View();
        }

        public string LoadChildArea()
        {
            var treeNodes = _areaQuery.GetTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            return tree;
        }

        [HttpPost]
        public JsonResult Create(CreatePurchaseContract model)
        {
           
            if (string.IsNullOrEmpty(model.Items)) throw new Exception("商品明细为空");
            var productPriceList = JsonConvert.DeserializeObject<List<ProductPriceModel>>(model.Items);
             Dictionary<int, decimal> productPriceDic = new Dictionary<int, decimal>();
            productPriceList.ForEach(n => productPriceDic.Add(n.Id, n.Price));
            model.ProductPriceDic = productPriceDic;

            _purchaseContractFacade.Create(model);
            return Json(new { success = true });
        }
        public ActionResult Edit(int id)
        {
            var model = _query.Find<PurchaseContract>(id);
            // ViewBag.areaName = _query.Find<Area>(model.AreaId).FullName;
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(EditPurchaseContract model)
        {
            _purchaseContractFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(string ids)
        {
            _purchaseContractFacade.Delete(ids);
            return Json(new { success = true });
        }

        public JsonResult ImportProduct(string productCodePriceInput)
        {                
            var rows = _purchaseContractQuery.GetPurchaseContractItems(productCodePriceInput);
            return Json(new { success = true, data = rows });
        }

    }
}