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
        BillSequenceService _sequenceService;
        public StoreInventoryService(IDBContext dbContext)
        {
            this._db = dbContext;
            _sequenceService = new BillSequenceService(this._db);
        }

        public void StockInProducts(StorePurchaseOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            var entityItems = entity.Items;
            //记录库存批次
            var inventoryBatchs = new List<StoreInventoryBatch>();
            var batchNo = _sequenceService.GenerateBatchNo(entity.StoreId);
            foreach (var item in entityItems)
            {
                //同一批入库的商品，批次号一样
                item.BatchNo = batchNo;
                var batch = new StoreInventoryBatch(item.ProductId, entity.StoreId, entity.SupplierId, item.ActualQuantity,
                    item.ContractPrice,item.Price, item.BatchNo, item.ProductionDate, item.ShelfLife, entity.StoragedBy);
                inventoryBatchs.Add(batch);
            }
            _db.Insert(inventoryBatchs.ToArray());

            // 更新总库存
            Dictionary<int, StorePurchaseOrderItem> productQuantityDic = new Dictionary<int, StorePurchaseOrderItem>();
            entityItems.ToList().ForEach(item => productQuantityDic.Add(item.ProductId, item));

            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.StoreId });
            var inventoryUpdates = new List<StoreInventoryUpdate>();
            var inventoryHistorys = new List<StoreInventoryHistory>();

            foreach (var inventory in inventorys)
            {
                if (productQuantityDic.ContainsKey(inventory.ProductId))
                {
                    var purchaseOrderItem = productQuantityDic[inventory.ProductId];
                    var inventoryUpdateModel = new StoreInventoryUpdate();

                    inventoryUpdateModel.Id = inventory.Id;
                    inventoryUpdateModel.Quantity = purchaseOrderItem.ActualQuantity; //要更新的库存数量
                    inventoryUpdateModel.SaleQuantity = purchaseOrderItem.ActualQuantity; // 更新可售数量
                    // 计算移动加权平均成本
                    int totalQuantity = inventory.Quantity + purchaseOrderItem.ActualQuantity;
                    inventoryUpdateModel.AvgCostPrice = Math.Round((inventory.AvgCostPrice * inventory.Quantity + purchaseOrderItem.Price * purchaseOrderItem.ActualQuantity) / totalQuantity, 2);
                    inventoryUpdates.Add(inventoryUpdateModel);
                    //记录库存流水
                    var history = new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, purchaseOrderItem.ActualQuantity,
                        purchaseOrderItem.Price, purchaseOrderItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseOrder, entity.StoragedBy);
                    inventoryHistorys.Add(history);
                }
            }
            // update storeinventory set quantity =quantity+@addQuantity ,saleQuantity=saleQuantity+@addQuantity where Id=@id and quantity=@oldQuantity
            var updateInventorySql = UpdateQuantityAndAvgCostPriceSql();
            _db.Command.AddExecute(updateInventorySql, inventoryUpdates.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
        }
        /// <summary>
        /// 更新库存数量和平均成本 （入库使用)
        /// </summary>
        /// <returns></returns>
        private string UpdateQuantityAndAvgCostPriceSql()
        {
            string sql = " update storeinventory set quantity =quantity+@Quantity ,saleQuantity=saleQuantity+@SaleQuantity,AvgCostPrice=@AvgCostPrice where Id=@Id";
            return sql;
        }

        /// <summary>
        /// 采购出库。 采购退单，数量为负
        /// </summary>
        /// <param name="entity"></param>
        public void StockOutInventory(StorePurchaseOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            var entityItems = entity.Items;
            Dictionary<int, StorePurchaseOrderItem> productQuantityDic = new Dictionary<int, StorePurchaseOrderItem>();
            entityItems.ToList().ForEach(item => productQuantityDic.Add(item.ProductId, item));
            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.StoreId });
            var inventoryBatchs = _db.Table.FindAll<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId in @ProductIds and Quantity>0", new { StoreId = entity.StoreId, ProductIds = productIdArray });
            var inventoryHistorys = new List<StoreInventoryHistory>();
            var inventoryUpdates = new List<StoreInventoryUpdate>();  // 库存更新
            var inventoryBatchUpdates = new List<StoreInventoryBatchUpdate>(); //批次更新
            foreach (var inventory in inventorys)
            {
                if (productQuantityDic.ContainsKey(inventory.ProductId))
                {
                    var purchaseOrderItem = productQuantityDic[inventory.ProductId];
                    // 先检查总库存是否够扣减
                    if (inventory.Quantity < Math.Abs(purchaseOrderItem.ActualQuantity))
                    {
                        var product = _db.Table.Find<Product>(inventory.ProductId);
                        throw new Exception(string.Format("{0}库存不足！", product.Code));
                    }
                    // 扣减总库存
                    var inventoryUpdateModel = new StoreInventoryUpdate();
                    inventoryUpdateModel.Id = inventory.Id;
                    inventoryUpdateModel.Quantity = purchaseOrderItem.ActualQuantity; //要更新的库存数量  为负数
                    inventoryUpdateModel.SaleQuantity = purchaseOrderItem.ActualQuantity; // 更新可售数量 为负数
                    // 退货不重新算移动平均成本 ，保持不变                  
                    inventoryUpdateModel.AvgCostPrice = inventory.AvgCostPrice;
                    inventoryUpdates.Add(inventoryUpdateModel);

                    // 扣减库存批次

                    //1 如果商品是指定批次，先扣减指定批次，数量不够再按批次顺序扣减
                    var batchProduct = inventoryBatchs.FirstOrDefault(n => n.ProductId == inventory.ProductId && n.BatchNo == purchaseOrderItem.BatchNo);
                    if (batchProduct == null)
                    {
                        throw new Exception("商品批次数据错误");

                    }
                    if (batchProduct.Quantity > Math.Abs(purchaseOrderItem.ActualQuantity))
                    {
                        inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchProduct.Id, purchaseOrderItem.ActualQuantity));
                        var history = new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, purchaseOrderItem.ActualQuantity,
                    purchaseOrderItem.Price, purchaseOrderItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseOrder, entity.StoragedBy);
                        inventoryHistorys.Add(history);
                    }
                    else
                    {
                        //数量不足                       
                        inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchProduct.Id, -batchProduct.Quantity));

                        var history = new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -batchProduct.Quantity,
                    purchaseOrderItem.Price, purchaseOrderItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseOrder, entity.StoragedBy);
                        inventoryHistorys.Add(history);
                        // 总库存  
                        inventory.Quantity =inventory.Quantity - batchProduct.Quantity;  // 第一次扣减后总库存
                        //剩余还要扣减数
                        var leftQuantity = Math.Abs(batchProduct.Quantity + purchaseOrderItem.ActualQuantity);
                        batchProduct.Quantity = batchProduct.Quantity - batchProduct.Quantity;  // 扣减第一个批次
                        //再按批次顺序扣减剩余部分
                        var productBatchs = inventoryBatchs.Where(n => n.ProductId == inventory.ProductId && n.Quantity > 0).OrderBy(n => n.BatchNo);
                        foreach (var batchItem in productBatchs)
                        {
                            if (batchItem.Quantity > leftQuantity)
                            {
                                inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -leftQuantity));
                                //记录修改历史
                                inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                    purchaseOrderItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseOrder, entity.CreatedBy));
                                break;
                            }
                            else
                            {
                                inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -batchItem.Quantity));
                                // 剩余扣减数
                                inventory.Quantity = inventory.Quantity - batchItem.Quantity;  // 第1+N次扣减后总库存
                                leftQuantity =leftQuantity -batchItem.Quantity  ;
                                inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -batchItem.Quantity,
                                                             purchaseOrderItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseOrder, entity.CreatedBy));
                            }
                        }
                    }

                }
            }

            _db.Command.AddExecute(UpdateQuantityAndAvgCostPriceSql(), inventoryUpdates.ToArray());
            _db.Command.AddExecute(updateInventoryBatchQuantitySql(), inventoryBatchUpdates.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
        }

        private string updateInventoryBatchQuantitySql()
        {
            string sql = "update StoreInventoryBatch set Quantity=Quantity+@Quantity where Id=@Id";
            return sql;
        }

        public IEnumerable<StoreInventory> CheckProductNotInInventory(StorePurchaseOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            //查询门店库存中不存在商品
            string sql = @"select i.ProductId ,o.StoreId from storepurchaseorderitem i inner join storepurchaseorder o on i.StorePurchaseOrderId = o.Id 
left join (select * from storeinventory si where si.StoreId = @StoreId ) s on i.ProductId = s.ProductId 
where s.Id is null  and i.StorePurchaseOrderId = @StorePurchaseOrderId";
            var items = _db.Table.FindAll<StoreInventory>(sql, new { StoreId = entity.StoreId, StorePurchaseOrderId = entity.Id });
            return items;
        }

        /// <summary>
        /// 销售单出库
        /// </summary>
        /// <param name="entity"></param>
        public void StockOutSaleOrder(SaleOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            var entityItems = entity.Items;
            Dictionary<int, SaleOrderItem> productQuantityDic = new Dictionary<int, SaleOrderItem>();
            entityItems.ToList().ForEach(item => productQuantityDic.Add(item.ProductId, item));
            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.StoreId });
            var inventoryBatchs = _db.Table.FindAll<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId in @ProductIds and Quantity>0", new { StoreId = entity.StoreId, ProductIds = productIdArray });
            var inventoryHistorys = new List<StoreInventoryHistory>();
            var inventoryUpdates = new List<StoreInventoryUpdate>();  // 库存更新
            var inventoryBatchUpdates = new List<StoreInventoryBatchUpdate>(); //批次更新
            foreach (var inventory in inventorys)
            {
                if (productQuantityDic.ContainsKey(inventory.ProductId))
                {
                    var purchaseOrderItem = productQuantityDic[inventory.ProductId];
                    // 记录销售明细的平均成本
                    purchaseOrderItem.AvgCostPrice = inventory.AvgCostPrice;

                    // 扣减总库存, 允许负库存销售
                    var inventoryUpdateModel = new StoreInventoryUpdate();
                    inventoryUpdateModel.Id = inventory.Id;
                    inventoryUpdateModel.Quantity = -purchaseOrderItem.Quantity; //要更新的库存数量
                    inventoryUpdateModel.SaleQuantity = -purchaseOrderItem.Quantity; // 更新可售数量
                    // 退货不重新算移动平均成本 ，保持不变                  
                    inventoryUpdateModel.AvgCostPrice = inventory.AvgCostPrice;
                    inventoryUpdates.Add(inventoryUpdateModel);

                    // 扣减库存批次,销售单，依次按先进先出扣减库存
                    //再按批次顺序扣减剩余部分
                    var productBatchs = inventoryBatchs.Where(n => n.ProductId == inventory.ProductId && n.Quantity > 0).OrderBy(n => n.BatchNo);
                    var leftQuantity = purchaseOrderItem.Quantity;
                    foreach (var batchItem in productBatchs)
                    {
                        if (batchItem.Quantity > leftQuantity)
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -leftQuantity));                          
                            //记录修改历史
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy));
                            leftQuantity = 0; //扣完
                            break;
                        }
                        else
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -batchItem.Quantity));
                            // 剩余扣减数
                            inventory.Quantity = inventory.Quantity - batchItem.Quantity;  // 第1+N次扣减后总库存
                            leftQuantity = leftQuantity - batchItem.Quantity;
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -batchItem.Quantity,
                                                         batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy));
                        }
                    }

                    if (leftQuantity > 0)
                    { 
                        //如果所有批次都扣完了，依然不够，用最后一个批次商品再扣一次负库存
                        inventory.Quantity = inventory.Quantity - leftQuantity;  // 第1+N次扣减后总库存
                        var lastItem = productBatchs.LastOrDefault();
                        inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                                       lastItem.Price, lastItem.BatchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy));
                    }

                }
            }

            _db.Command.AddExecute(UpdateQuantityAndAvgCostPriceSql(), inventoryUpdates.ToArray());
            _db.Command.AddExecute(updateInventoryBatchQuantitySql(), inventoryBatchUpdates.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
            // 更新销售明细平均成本
            _db.Update(entity.Items.ToArray());
        }

        /// <summary>
        /// 销售退单，入库
        /// </summary>
        /// <param name="entity"></param>
        public void StockInRefundOrder(SaleOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            var entityItems = entity.Items;
            //记录库存批次
            var inventoryBatchs = new List<StoreInventoryBatch>();
            var batchNo = _sequenceService.GenerateBatchNo(entity.StoreId);
            foreach (var item in entityItems)
            {
                // 查询商品合同价
                var contract = _db.Table.Find<PurchaseContract>("select * from PurchaseContract where FIND_IN_SET(@StoreId,StoreIds) and Status=3 and  EndDate>=@Today", new { StoreId = entity.StoreId, Today = DateTime.Now.Date });
                var contractItem = _db.Table.Find<PurchaseContractItem>("select * from purchasecontractitem where PurchaseContractId=@PurchaseContractId and ProductId=@ProductId", new { PurchaseContractId=contract.Id,ProductId=item.ProductId });

                var batch = new StoreInventoryBatch(item.ProductId, entity.StoreId, contract.SupplierId, Math.Abs(item.Quantity),
                    contractItem.ContractPrice, contractItem.ContractPrice, batchNo, null, 0, entity.UpdatedBy);
                inventoryBatchs.Add(batch);
                // 更新订单成本
                item.AvgCostPrice = contractItem.ContractPrice;
            }
            _db.Insert(inventoryBatchs.ToArray());

            Dictionary<int, SaleOrderItem> productQuantityDic = new Dictionary<int, SaleOrderItem>();
            entityItems.ToList().ForEach(item => productQuantityDic.Add(item.ProductId, item));

            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.StoreId });
            var inventoryUpdates = new List<StoreInventoryUpdate>();
            var inventoryHistorys = new List<StoreInventoryHistory>();

            foreach (var inventory in inventorys)
            {
                if (productQuantityDic.ContainsKey(inventory.ProductId))
                {
                    var purchaseOrderItem = productQuantityDic[inventory.ProductId];
                    var contractPrice = purchaseOrderItem.AvgCostPrice;
                    var inventoryUpdateModel = new StoreInventoryUpdate();
                    var quantity = Math.Abs(purchaseOrderItem.Quantity);
                    inventoryUpdateModel.Id = inventory.Id;
                    inventoryUpdateModel.Quantity = quantity; //要更新的库存数量 
                    inventoryUpdateModel.SaleQuantity = quantity; // 更新可售数量 

                    // 计算移动加权平均成本
                    int totalQuantity = inventory.Quantity + quantity;
                    inventoryUpdateModel.AvgCostPrice = Math.Round((inventory.AvgCostPrice * inventory.Quantity + purchaseOrderItem.AvgCostPrice * quantity) / totalQuantity, 2);
                    inventoryUpdates.Add(inventoryUpdateModel);


                    //记录库存流水
                    var history = new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -purchaseOrderItem.Quantity,
                        contractPrice, batchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.UpdatedBy);
                    inventoryHistorys.Add(history);
                }
            }
            // update storeinventory set quantity =quantity+@addQuantity ,saleQuantity=saleQuantity+@addQuantity where Id=@id and quantity=@oldQuantity
            var updateInventorySql = UpdateQuantityAndAvgCostPriceSql();
            _db.Command.AddExecute(updateInventorySql, inventoryUpdates.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
            _db.Update(entity.Items.ToArray());  // 更新订单成本
        }


        // 扣减库存 
        public void MinusInventory(SaleOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            Dictionary<int, SaleOrderItem> productQuantityDic = new Dictionary<int, SaleOrderItem>();
            entity.Items.ToList().ForEach(item => productQuantityDic.Add(item.ProductId, item));
            var productIdArray = productQuantityDic.Keys.ToArray();
            // 商品查询，要考虑供应商
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where storeId=@StoreId and productId in @ProductIds", new { StoreId = entity.StoreId, ProductIds = productIdArray });
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
                    var productBatchs = inventoryBatchs.Where(n => n.ProductId == product.ProductId).OrderBy(n => n.BatchNo).ToList();
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
                        else
                        {
                            var history = new StoreInventoryHistory(product.ProductId, entity.StoreId, batchItem.Quantity, batchItem.Quantity,
                                                         price, batchItem.BatchNo, entity.Id, entity.Code, ValueObject.BillIdentity.SaleOrder, entity.CreatedBy);
                            inventoryHistorys.Add(history);
                        }
                    }

                }
            }
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
            var batchNo = idService.GenerateBatchNo(model.StoreId);
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
                0,product.AvgCostPrice, batchNo, null, 0, model.UpdatedBy);
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
