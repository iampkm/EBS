using EBS.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Application;
using EBS.Query.DTO;
using EBS.Query;
namespace EBS.Admin.Controllers
{
    [Permission]
    public class ToolController : Controller
    {
        IContextService _context;
        IDBContext _db;
        IQuery _iquery;
        IPosSyncFacade _posFacade;      
        public ToolController(IContextService context, IDBContext db, IQuery iquery, IPosSyncFacade posFacade)
        {
            _context = context;
            _db = db;
            _iquery = iquery;
            _posFacade = posFacade;           
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Repair()
        {
            return View();
        }

        public JsonResult QueryStoreInventoryHistory(string code)
        {
            var rows = _iquery.FindAll<StoreInventoryHistory>(n => n.BillCode == code).ToList();
            return Json(new { success = true, data = rows, total = rows.Count });
        }

        public JsonResult RepairInventory(StoreInventoryHistory model)
        {
            try
            {
                var changeQuantity = 0 - model.ChangeQuantity;   //反向操作库存

                // 修复批次库存
                string sqlUpdateBatch = "update storeinventorybatch set quantity =quantity+@ChangeQuantity where storeid = @StoreId  and productid =@ProductId and batchNo =@BatchNo";
                _db.Command.AddExecute(sqlUpdateBatch, new { StoreId = model.StoreId, ProductId = model.ProductId, BatchNo = model.BatchNo, ChangeQuantity = changeQuantity });
                //修复库存
                string sqlUpdateInventory = "update storeinventory set Quantity =Quantity+@ChangeQuantity,SaleQuantity = SaleQuantity+@ChangeQuantity where storeid = @StoreId  and productid =@ProductId";
                _db.Command.AddExecute(sqlUpdateInventory, new { StoreId = model.StoreId, ProductId = model.ProductId, ChangeQuantity = changeQuantity });
                //删除重复库存流水
                string sqlDeleteHistory = @"delete  from storeinventoryhistory where  billcode =@BillCode and id = @Id";
                _db.Command.AddExecute(sqlDeleteHistory, new { BillCode = model.BillCode, Id = model.Id });

                _db.SaveChange();

                return Json(new { success = true, });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, error = ex.Message });
            }

        }

        public ActionResult OrderRepair()
        {
            return View();
        }
        [HttpPost]
        public JsonResult OrderRepair(string saleOrderCodes)
        {

            if (string.IsNullOrEmpty(saleOrderCodes)) { throw new Exception("单据号不能为空"); }
            var codeArray = saleOrderCodes.Trim('\n').Split('\n');
            var html = "";
            foreach (var code in codeArray)
            {
                try
                {
                    _posFacade.SaleOrderInventorySync(code);                    
                    html += string.Format("订单{0},处理成功 </br>", code);
                }
                catch (Exception ex)
                {
                    html += string.Format("订单{0},处理失败{1} </br>", code, ex.Message);
                }
            }

            return Json(new { success = true, message = html });

        }



      
    }
}