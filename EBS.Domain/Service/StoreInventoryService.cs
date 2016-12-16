using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
using EBS.Domain.ValueObject;
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

        public IEnumerable<StoreInventory> CheckProductNotInInventory(StorePurchaseOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if(entity.Items.Count()==0) { throw new Exception("单据明细为空"); }
            //查询门店库存中不存在商品
            string sql = @"select i.ProductId ,o.StoreId from storepurchaseorderitem i inner join storepurchaseorder o on i.StorePurchaseOrderId = o.Id 
left join (select * from storeinventory si where si.StoreId = @StoreId ) s on i.ProductId = s.ProductId 
where s.Id is null  and i.StorePurchaseOrderId = @StorePurchaseOrderId";
            var items = _db.Table.FindAll<StoreInventory>(sql, new { StoreId = entity.StoreId, StorePurchaseOrderId = entity.Id });
            return items;     
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


        public void FixedInventory(StocktakingPlan model)
        {
            if (model == null) { throw new Exception("单据不存在"); }
            if (model.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            //小盘不结转入库
            if (model.Method == StocktakingPlanMethod.SmallCap)
            {
                return;
            }
            Dictionary<int, StocktakingPlanItem> productQuantityDic = new Dictionary<int, StocktakingPlanItem>();
            //过滤掉盘点没有差异的产品
            model.Items = model.Items.Where(p => p.GetDifferenceQuantity() != 0).ToList();
            model.Items.ForEach((item) =>
            {
                if (!productQuantityDic.ContainsKey(item.ProductId))
                {
                    productQuantityDic.Add(item.ProductId, item);
                }
            });

            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where storeId=@StoreId and productId in @ProductIds", new { StoreId = model.StoreId, ProductIds = productIdArray });
            var inventoryBatchs = _db.Table.FindAll<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId in @ProductIds and Quantity>0", new { StoreId = model.StoreId, ProductIds = productIdArray });
            var inventoryHistorys = new List<StoreInventoryHistory>();

            var inventoryProfit = new List<StoreInventoryBatch>();  //盘盈批次数据
            BillSequenceService idService = new BillSequenceService(this._db);
            var batchNo = idService.GenerateBatchNo();
            foreach (var product in inventorys)
            {
                if (productQuantityDic.ContainsKey(product.ProductId))
                {
                    var inventoryQuantity = product.Quantity;
                    // 盘点差异数量，盘盈为整数，盘亏为负数
                    var quantity = productQuantityDic[product.ProductId].GetDifferenceQuantity(); 
                   // var price = productQuantityDic[product.ProductId].RealPrice;
                    //库存
                    product.Quantity += quantity;
                    product.SaleQuantity += quantity;

                    // 设置订单明细中的均价成本
                  //  productQuantityDic[product.ProductId].AvgCostPrice = product.AvgCostPrice;
                    // 盘盈数据
                    if (quantity > 0)
                    {
                        var batch = new StoreInventoryBatch(product.ProductId, model.StoreId, 0, quantity,
                product.AvgCostPrice, batchNo, null, 0, model.UpdatedBy);
                        inventoryProfit.Add(batch);

                        //入库历史记录
                        var history = new StoreInventoryHistory(product.ProductId, model.StoreId, inventoryQuantity, quantity,
                            product.AvgCostPrice, batchNo, model.Id, model.Code, ValueObject.BillIdentity.StoreStocktakingPlan, model.UpdatedBy);
                        inventoryHistorys.Add(history);
                        continue;
                    }

                    // 库存批次 盘亏，按照先进先出扣减批次
                    var productBatchs = inventoryBatchs.Where(n => n.ProductId == product.ProductId).OrderBy(n => n.BatchNo).ToList();
                    foreach (var batchItem in productBatchs)
                    {
                        if (batchItem.Quantity - quantity >= 0)
                        {
                            //记录修改历史
                            var history = new StoreInventoryHistory(product.ProductId, model.StoreId, batchItem.Quantity, quantity,
                                product.AvgCostPrice, batchItem.BatchNo, model.Id, model.Code, ValueObject.BillIdentity.SaleOrder, model.CreatedBy);
                            inventoryHistorys.Add(history);
                            break;
                        }
                        else
                        {
                            var history = new StoreInventoryHistory(product.ProductId, model.StoreId, batchItem.Quantity, batchItem.Quantity,
                                                          product.AvgCostPrice, batchItem.BatchNo, model.Id, model.Code, ValueObject.BillIdentity.StoreStocktakingPlan, model.UpdatedBy);
                            inventoryHistorys.Add(history);
                        }
                    }

                }
            }

            // update storeinventory set quantity =quantity+@addQuantity ,saleQuantity=saleQuantity+@addQuantity where Id=@id and quantity=@oldQuantity
            _db.Update(inventorys.ToArray());
            _db.Update(inventoryBatchs.ToArray());
            _db.Insert(inventoryProfit.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
        }
        
    }
}
