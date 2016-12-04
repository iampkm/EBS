﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
namespace EBS.Domain.Service
{
   public class StoreInventoryService
    {
        IDBContext _db;
        public StoreInventoryService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void StockInProducts(StorePurchaseOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == entity.Id);
            Dictionary<int, StorePurchaseOrderItem> productQuantityDic = new Dictionary<int, StorePurchaseOrderItem>();
            entityItems.ToList().ForEach(item => productQuantityDic.Add(item.ProductId,item));
            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds", new { ProductIds = productIdArray });
            var inventoryHistorys = new List<StoreInventoryHistory>();
            foreach (var product in inventorys)
            {
                if (productQuantityDic.ContainsKey(product.ProductId))
                {
                    var inventoryQuantity = product.Quantity;
                    var addQuantity = productQuantityDic[product.ProductId].ActualQuantity;
                    var price = productQuantityDic[product.ProductId].Price;
                    //库存
                    product.Quantity += addQuantity;
                    product.SaleQuantity += addQuantity;
                    // 计算移动加权平均成本
                    product.AvgCostPrice = Math.Round((product.AvgCostPrice * inventoryQuantity + price * addQuantity) / product.Quantity); 

                    //记录修改历史
                    var history = new StoreInventoryHistory(product.ProductId,entity.StoreId, inventoryQuantity, addQuantity, 
                        price,entity.BatchNo,entity.Id,entity.Code,ValueObject.BillIdentity.StorePurchaseOrder,entity.StoragedBy);
                    inventoryHistorys.Add(history);
                }
            }
            // update storeinventory set quantity =quantity+@addQuantity ,saleQuantity=saleQuantity+@addQuantity where Id=@id and quantity=@oldQuantity
            _db.Update(inventorys.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
        }

        public void CreateProductNotInInventory(StorePurchaseOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if(entity.Items.Count()==0) { throw new Exception("单据明细为空"); }
            //查询门店库存中不存在商品
            string sql = @"select i.ProductId ,o.StoreId from storepurchaseorderitem i inner join storepurchaseorder o on i.StorePurchaseOrderId = o.Id 
left join (select * from storeinventory si where si.StoreId = @StoreId ) s on i.ProductId = s.ProductId 
where s.Id is null  and i.StorePurchaseOrderId = @StorePurchaseOrderId";
            var items = _db.Table.FindAll<StoreInventory>(sql, new { StoreId = entity.StoreId, StorePurchaseOrderId = entity.Id });
            if (items.Count() > 0)
            {
                _db.Insert<StoreInventory>(items.ToArray());
            }          
        }

        // 扣减库存 
        public void MinusInventory(SaleOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            Dictionary<int, SaleOrderItem> productQuantityDic = new Dictionary<int, SaleOrderItem>();
            entity.Items.ToList().ForEach(item => productQuantityDic.Add(item.ProductId, item));
            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where storeId=@StoreId and productId in @ProductIds", new { StoreId=entity.StoreId, ProductIds = productIdArray });
            var inventoryBatchs = _db.Table.FindAll<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId in @ProductIds and Quantity>0", new { StoreId = entity.StoreId, ProductIds = productIdArray });
            var inventoryHistorys = new List<StoreInventoryHistory>();
            foreach (var product in inventorys)
            {
                if (productQuantityDic.ContainsKey(product.ProductId))
                {
                    var inventoryQuantity = product.Quantity;
                    // 销售数量
                    var quantity = productQuantityDic[product.ProductId].Quantity;
                    var price = productQuantityDic[product.ProductId].RealPrice;                   
                    //库存
                    product.Quantity -= quantity;
                    product.SaleQuantity -= quantity;

                    // 设置订单明细中的均价成本
                    productQuantityDic[product.ProductId].AvgCostPrice = product.AvgCostPrice;

                    // 扣减库存批次
                    var productBatchs= inventoryBatchs.Where(n => n.ProductId == product.ProductId).OrderBy(n => n.BatchNo).ToList();
                    foreach (var batchItem in productBatchs)
                    {
                        if (batchItem.Quantity - quantity >= 0)
                        {
                            //记录修改历史
                            var history = new StoreInventoryHistory(product.ProductId, entity.StoreId, batchItem.Quantity, quantity,
                                price, batchItem.BatchNo, entity.Id, entity.Code, ValueObject.BillIdentity.SaleOrder, entity.CreatedBy);
                            inventoryHistorys.Add(history);
                            break;
                        }
                        else {
                            var history = new StoreInventoryHistory(product.ProductId, entity.StoreId, batchItem.Quantity, batchItem.Quantity,
                                                         price, batchItem.BatchNo, entity.Id, entity.Code, ValueObject.BillIdentity.SaleOrder, entity.CreatedBy);
                            inventoryHistorys.Add(history);
                        }
                    }
                    
                }
            }
            // update storeinventory set quantity =quantity+@addQuantity ,saleQuantity=saleQuantity+@addQuantity where Id=@id and quantity=@oldQuantity
            _db.Update(inventorys.ToArray());
            _db.Update(inventoryBatchs.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
        }
        
    }
}