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
            return View();
        }

        public string LoadChildArea()
        {
            var treeNodes = _areaQuery.GetTree();
            var tree = JsonConvert.SerializeObject(treeNodes);
            return tree;
        }

        [HttpPost]
        public JsonResult Create(PurchaseContractModel model)
        {
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
        public JsonResult Edit(PurchaseContractModel model)
        {
            _purchaseContractFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Delete(string ids)
        {
            _purchaseContractFacade.Delete(ids);
            return Json(new { success = true });
        }

        public JsonResult ImportProduct(string itemsJson)
        {
            //var rows = _query.FindAll<ProductSku>().Select(n => new PurchaseContractItemDto()
            //{
            //    Id = n.Id,
            //    Code = n.Code,
            //    Name = n.Name,
            //    CategoryName = "123",
            //    Price = 123.12m,
            //    Specification = n.Specification
            //});

            var rows =new List<PurchaseContractItemDto>();
            rows.Add(
                new PurchaseContractItemDto()
                {
                    Id = 1,
                    Code = "1111",
                    Name ="eeeee",
                    CategoryName = "123",
                    Price = 123.12m,
                    Specification = "wer"
                });
            rows.Add(
               new PurchaseContractItemDto()
               {
                   Id = 2,
                   Code = "222",
                   Name = "eeeeewwwweee",
                   CategoryName = "55555",
                   Price = 1123.12m,
                   Specification = "6666"
               });


            return Json(new { success = true, data = rows });
        }

    }
}