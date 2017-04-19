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

        public void CheckIsExists(string code)
        {
            if (_db.Table.Exists<StoreInventoryHistory>(n => n.BillCode == code))
            {
                throw new Exception(string.Format("库存流水{0}已经存在了", code));
            }
        }

        /// <summary>
        /// 采购入库
        /// </summary>
        /// <param name="entity"></param>
        public void StockInProducts(StorePurchaseOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            //记录库存批次
            var productIdArray = entity.Items.Select(n => n.ProductId).ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.StoreId });
            var inventoryBatchs = new List<StoreInventoryBatch>();
            var inventoryUpdates = new List<StoreInventoryUpdate>();
            var inventoryHistorys = new List<StoreInventoryHistory>();
            var batchNo = _sequenceService.GenerateBatchNo(entity.StoreId);

            foreach (var item in entity.Items)
            {
                // 采购单允许实收为0，这种商品需要跳过，不处理
                if (item.ActualQuantity == 0) { continue; }
                item.BatchNo = batchNo; //更新单据明细批次
                var inventoryItem = inventorys.FirstOrDefault(n => n.ProductId == item.ProductId);
                if (inventoryItem == null) throw new Exception(string.Format("商品{0}不存在", item.ProductId));

                // 入库      
                var inventoryUpdateModel = new StoreInventoryUpdate();
                inventoryUpdateModel.Id = inventoryItem.Id;
                inventoryUpdateModel.Quantity = item.ActualQuantity; //要更新的库存数量
                inventoryUpdateModel.SaleQuantity = item.ActualQuantity; // 更新可售数量
                inventoryUpdateModel.LastCostPrice = item.Price > 0 ? item.Price : inventoryItem.LastCostPrice;
                inventoryUpdateModel.AvgCostPrice = CalculatedAveragePrice(inventoryItem, item.Price, item.ActualQuantity);  // 修改库存均价
                inventoryUpdates.Add(inventoryUpdateModel);

                //记录库存流水
                var history = new StoreInventoryHistory(item.ProductId, entity.StoreId, inventoryItem.Quantity, item.ActualQuantity,
                    item.Price, item.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseOrder, entity.StoragedBy,entity.SupplierId);
                inventoryHistorys.Add(history);
                //记录库存批次
                var batchQuantity = CalculatedBatchQuantity(inventoryItem.Quantity, item.ActualQuantity);
                var batch = new StoreInventoryBatch(item.ProductId, entity.StoreId, entity.SupplierId, batchQuantity,
                    item.ContractPrice, item.Price, item.BatchNo, item.ProductionDate, item.ShelfLife, entity.StoragedBy);
                inventoryBatchs.Add(batch);
            }
            
            _db.Update(entity.Items.ToArray());  
            _db.Command.AddExecute(UpdateQuantityAndLastCostPriceSql(), inventoryUpdates.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
            _db.Insert(inventoryBatchs.ToArray());            
        }
        /// <summary>
        /// 更新库存数量和最新入库成本 （入库使用)
        /// </summary>
        /// <returns></returns>
        /// 
        private string UpdateQuantityAndLastCostPriceSql()
        {
            string sql = " update storeinventory set quantity =quantity+@Quantity ,saleQuantity=saleQuantity+@SaleQuantity,AvgCostPrice=@AvgCostPrice,LastCostPrice=@LastCostPrice where Id=@Id";
            return sql;
        }
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
                    if (inventory.Quantity < purchaseOrderItem.ActualQuantity)
                    {
                        var product = _db.Table.Find<Product>(inventory.ProductId);
                        throw new Exception(string.Format("{0}库存不足！", product.Code));
                    }
                    // 扣减总库存
                    var inventoryUpdateModel = new StoreInventoryUpdate();
                    inventoryUpdateModel.Id = inventory.Id;
                    inventoryUpdateModel.Quantity = -purchaseOrderItem.ActualQuantity;
                    inventoryUpdateModel.SaleQuantity = -purchaseOrderItem.ActualQuantity;
                    // 退货不重新算移动平均成本 ，保持不变                  
                    inventoryUpdateModel.AvgCostPrice = inventory.AvgCostPrice;
                    inventoryUpdates.Add(inventoryUpdateModel);

                    //1 如果商品是指定批次，先扣减指定批次
                    var batchProduct = inventoryBatchs.FirstOrDefault(n => n.ProductId == inventory.ProductId && n.BatchNo == purchaseOrderItem.BatchNo);
                    var leftQuantity = purchaseOrderItem.ActualQuantity;
                    if (batchProduct != null)
                    {
                        if (batchProduct.Quantity >= purchaseOrderItem.ActualQuantity)
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchProduct.Id, -purchaseOrderItem.ActualQuantity));
                            var history = new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -purchaseOrderItem.ActualQuantity,
                        batchProduct.Price, purchaseOrderItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseRefundOrder, entity.StoragedBy,entity.SupplierId);
                            inventoryHistorys.Add(history);
                            continue;
                        }
                        else
                        {
                            //数量不足                       
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchProduct.Id, -batchProduct.Quantity));

                            var history = new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -batchProduct.Quantity,
                        batchProduct.Price, purchaseOrderItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseRefundOrder, entity.StoragedBy, entity.SupplierId);
                            inventoryHistorys.Add(history);
                            // 总库存  
                            inventory.Quantity = inventory.Quantity - batchProduct.Quantity;  // 第一次扣减后总库存
                            //剩余还要扣减数
                            leftQuantity = purchaseOrderItem.ActualQuantity - batchProduct.Quantity;
                            batchProduct.Quantity = batchProduct.Quantity - batchProduct.Quantity;  // 扣减第一个批次为 0
                        }

                    }
                    // 2 按照先进先出顺序扣减批次库存
                    var productBatchs = inventoryBatchs.Where(n => n.ProductId == inventory.ProductId && n.Quantity > 0).OrderBy(n => n.BatchNo);
                    foreach (var batchItem in productBatchs)
                    {
                        if (batchItem.Quantity >= leftQuantity)
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -leftQuantity));
                            //记录修改历史
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseRefundOrder, entity.CreatedBy, entity.SupplierId));
                            break;
                        }
                        else
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -batchItem.Quantity));
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -batchItem.Quantity,
                                                         batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.StorePurchaseRefundOrder, entity.CreatedBy, entity.SupplierId));
                            // 剩余扣减数
                            inventory.Quantity = inventory.Quantity - batchItem.Quantity;  // 第1+N次扣减后总库存
                            leftQuantity = leftQuantity - batchItem.Quantity;
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

        public IEnumerable<StoreInventory> CheckNotExistsProduct(TransferOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            //查询门店库存中不存在商品
            string sql = @"select i.ProductId ,o.toStoreId as StoreId from transferorderitem i inner join transferorder o on i.transferorderId = o.Id 
left join (select * from storeinventory si where si.StoreId = @StoreId ) s on i.ProductId = s.ProductId 
where s.Id is null  and i.`TransferOrderId`=@TransferOrderId";
            var items = _db.Table.FindAll<StoreInventory>(sql, new { StoreId = entity.ToStoreId, TransferOrderId = entity.Id });
            return items;
        }

        /// <summary>
        /// 销售单出库
        /// </summary>
        /// <param name="entity"></param>
        public void StockOutSaleOrder(SaleOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count == 0) { throw new Exception("单据明细为空"); }
            var entityItems = entity.Items;
            Dictionary<int, SaleOrderItem> productQuantityDic = new Dictionary<int, SaleOrderItem>();
            entityItems.ForEach(item => productQuantityDic.Add(item.ProductId, item));
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

                    //按照先进先出扣减批次库存
                    var productBatchs = inventoryBatchs.Where(n => n.ProductId == inventory.ProductId && n.Quantity > 0).OrderBy(n => n.BatchNo);
                    var leftQuantity = purchaseOrderItem.Quantity;
                    foreach (var batchItem in productBatchs)
                    {
                        if (batchItem.Quantity >= leftQuantity)
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -leftQuantity));
                            //记录修改历史
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy, entity.UpdatedOn,batchItem.SupplierId));
                            leftQuantity = 0; //扣完
                            break;
                        }
                        else
                        {
                            //记录批次扣减
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -batchItem.Quantity));
                            // 记录流水
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -batchItem.Quantity,
                                                        batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy, entity.UpdatedOn, batchItem.SupplierId));
                            //扣减库存
                            inventory.Quantity = inventory.Quantity - batchItem.Quantity;
                            leftQuantity = leftQuantity - batchItem.Quantity;
                            batchItem.Quantity = batchItem.Quantity - batchItem.Quantity;  // 因为批次不够扣，所以全部扣减为 0

                        }
                    }

                    if (leftQuantity > 0)  // 这里有两种情况，一种是批次商品数量全都为0，第二种情况是有批次数据,增加一个判断，可以少查一次数据库
                    {
                        if (productBatchs.Count() > 0)
                        {
                            var lastItem = inventoryBatchs.Where(n => n.ProductId == inventory.ProductId).OrderBy(n => n.BatchNo).LastOrDefault();
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                                           lastItem.Price, lastItem.BatchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy, entity.UpdatedOn, lastItem.SupplierId));
                        }
                        else
                        {
                            var lastItem = _db.Table.Find<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId=@ProductId order by batchNo desc LIMIT 1", new { StoreId = entity.StoreId, ProductId = inventory.ProductId });
                            if (lastItem != null) // 只收货，不入库，会为空
                            {
                                inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                                          lastItem.Price, lastItem.BatchNo, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy, entity.UpdatedOn, lastItem.SupplierId));
                            }
                            else
                            {
                                //当无批次库存数据时，用最新的合同商品价来记录
                                // 退货商品按照最新的合同进行入库，查询商品合同价
                                var contract = _db.Table.Find<PurchaseContract>(@"SELECT c.Id,c.SupplierId from purchasecontract c inner join purchasecontractitem i on c.id = i.PurchaseContractId where FIND_IN_SET(@StoreId,c.StoreIds) and c.`Status`=3 and i.ProductId = @productId order by c.Id desc", new { StoreId = entity.StoreId, ProductId = inventory.ProductId });
                                var contractItem = _db.Table.Find<PurchaseContractItem>("select * from purchasecontractitem where PurchaseContractId=@PurchaseContractId and ProductId=@ProductId", new { PurchaseContractId = contract.Id, ProductId = inventory.ProductId });
                                //var contract = _db.Table.Find<PurchaseContractItem>(@"SELECT i.ContractPrice from purchasecontract c inner join purchasecontractitem i on c.id = i.PurchaseContractId where FIND_IN_SET(@StoreId,c.StoreIds) and c.`Status`=3 and i.ProductId = @productId order by c.Id desc", new { StoreId = entity.StoreId, ProductId = inventory.ProductId });
                                inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                                         contractItem.ContractPrice, 0, entity.Id, entity.Code, BillIdentity.SaleOrder, entity.CreatedBy, entity.UpdatedOn, contract.SupplierId));
                            }

                        }
                        // 第1+N次扣减后总库存
                        inventory.Quantity = inventory.Quantity - leftQuantity;
                    }

                }
            }

            _db.Command.AddExecute(UpdateQuantityAndAvgCostPriceSql(), inventoryUpdates.ToArray());
            //负库存销售时，是没有批次数据需要更新
            if (inventoryBatchUpdates.Count > 0)
            {
                _db.Command.AddExecute(updateInventoryBatchQuantitySql(), inventoryBatchUpdates.ToArray());
            }
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
            //记录库存批次
            var productIdArray = entity.Items.Select(n => n.ProductId).ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.StoreId });
            var inventoryBatchs = new List<StoreInventoryBatch>();
            var inventoryUpdates = new List<StoreInventoryUpdate>();
            var inventoryHistorys = new List<StoreInventoryHistory>();          
            var batchNo = _sequenceService.GenerateBatchNo(entity.StoreId);
            foreach (var item in entity.Items)
            {
                var inventoryItem = inventorys.FirstOrDefault(n => n.ProductId == item.ProductId);
                if (inventoryItem == null) throw new Exception(string.Format("商品{0}不存在", item.ProductId));
                var returnQuantity = Math.Abs(item.Quantity); //  销售退单的数量，前端用负数记录               
                // 退货商品按照最新的合同进行入库，查询商品合同价
                var contract = _db.Table.Find<PurchaseContract>(@"SELECT c.Id,c.SupplierId from purchasecontract c inner join purchasecontractitem i on c.id = i.PurchaseContractId where FIND_IN_SET(@StoreId,c.StoreIds) and c.`Status`=3 and i.ProductId = @productId order by c.Id desc", new { StoreId = entity.StoreId, ProductId = item.ProductId });
                var contractItem = _db.Table.Find<PurchaseContractItem>("select * from purchasecontractitem where PurchaseContractId=@PurchaseContractId and ProductId=@ProductId", new { PurchaseContractId = contract.Id, ProductId = item.ProductId });

                // 入库      
                var inventoryUpdateModel = new StoreInventoryUpdate();
                inventoryUpdateModel.Id = inventoryItem.Id;
                inventoryUpdateModel.Quantity = returnQuantity; //要更新的库存数量
                inventoryUpdateModel.SaleQuantity = returnQuantity; // 更新可售数量
                inventoryUpdateModel.AvgCostPrice = CalculatedAveragePrice(inventoryItem, contractItem.ContractPrice, returnQuantity);  // 修改库存均价
                inventoryUpdates.Add(inventoryUpdateModel);

                //记录库存流水
                var history = new StoreInventoryHistory(item.ProductId, entity.StoreId, inventoryItem.Quantity, returnQuantity,
                        contractItem.ContractPrice, batchNo, entity.Id, entity.Code, BillIdentity.SaleRefund, entity.UpdatedBy, entity.UpdatedOn, contract.SupplierId);
                inventoryHistorys.Add(history);
                //记录库存批次
                var batchQuantity = CalculatedBatchQuantity(inventoryItem.Quantity, returnQuantity);
                var batch = new StoreInventoryBatch(item.ProductId, entity.StoreId, contract.SupplierId, batchQuantity,
                    contractItem.ContractPrice, contractItem.ContractPrice, batchNo, null, 0, entity.UpdatedBy);
                inventoryBatchs.Add(batch);
                // 更新订单成本
                item.AvgCostPrice = inventoryUpdateModel.AvgCostPrice;
            }            
            _db.Command.AddExecute(UpdateQuantityAndAvgCostPriceSql(), inventoryUpdates.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
            _db.Insert(inventoryBatchs.ToArray());
            _db.Update(entity.Items.ToArray());  // 更新订单成本

        }


        /// <summary>
        /// 调拨单增减库存
        /// </summary>
        /// <param name="entity"></param>
        public void TransaferInventory(TransferOrder entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            var productIdArray = entity.Items.Select(n => n.ProductId).ToArray();
            //出库商品
            var fromInventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.FromStoreId });
            var fromInventoryBatchs = _db.Table.FindAll<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId in @ProductIds and Quantity>0 ", new { StoreId = entity.FromStoreId, ProductIds = productIdArray });
            //入库商品
            var toInventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where productId in @ProductIds and StoreId=@StoreId", new { ProductIds = productIdArray, StoreId = entity.ToStoreId });

            // 出库是按照先进先出扣减库存批次，入库同样按照对应批次价格入库；不按照单据进价入库
            var inventoryHistorys = new List<StoreInventoryHistory>();
            var inventoryUpdates = new List<StoreInventoryUpdate>();  // 库存更新
            var inventoryBatchUpdates = new List<StoreInventoryBatchUpdate>(); //批次更新
            var inventoryBatchInserts = new List<StoreInventoryBatch>(); //增加批次
            var toBatchNo = _sequenceService.GenerateBatchNo(entity.ToStoreId); //统一入库批次号
            foreach (var item in entity.Items)
            {

                var fromInventoryItem = fromInventorys.FirstOrDefault(n => n.ProductId == item.ProductId);  //fromInventoryDic[item.ProductId];
                if (fromInventoryItem == null) throw new Exception(string.Format("调出商品{0}不存在", item.ProductId));
                var toInventoryItem = toInventorys.FirstOrDefault(n => n.ProductId == item.ProductId);
                if (toInventoryItem == null) throw new Exception(string.Format("调入商品{0}不存在", item.ProductId));
                if (item.Quantity == 0) { continue; }
                // 先检查总库存是否够扣减
                if (fromInventoryItem.Quantity < item.Quantity)
                {
                    var product = _db.Table.Find<Product>(item.ProductId);
                    throw new Exception(string.Format("商品[{0}]{1}库存不足！", product.Code, product.Name));
                }
                // 出库总库存
                var fromInventoryUpdateModel = new StoreInventoryUpdate();
                fromInventoryUpdateModel.Id = fromInventoryItem.Id;
                fromInventoryUpdateModel.Quantity = -item.Quantity;
                fromInventoryUpdateModel.SaleQuantity = -item.Quantity;
                // 出货不改价                 
                fromInventoryUpdateModel.AvgCostPrice = fromInventoryItem.AvgCostPrice;
                fromInventoryUpdateModel.LastCostPrice = fromInventoryItem.LastCostPrice;
                inventoryUpdates.Add(fromInventoryUpdateModel);

                //入库总库存
                //var ite = productQuantityDic[inventory.ProductId];

                var toInventoryUpdateModel = new StoreInventoryUpdate();
                toInventoryUpdateModel.Id = toInventoryItem.Id;
                toInventoryUpdateModel.Quantity = item.Quantity; //要更新的库存数量
                toInventoryUpdateModel.SaleQuantity = item.Quantity; // 更新可售数量
                inventoryUpdates.Add(toInventoryUpdateModel);

                //把当前商品和它的所有批次重新排序：指定批次排第一，其他按照先进先出排序
                var outInventoryBatchs = SortBatchByFIFO(fromInventoryBatchs.ToList(), item.ProductId, item.BatchNo);
                var leftQuantity = item.Quantity;
                foreach (var batchProduct in outInventoryBatchs)
                {
                    //当前批次可调拨数量
                    var transaferQuantity = batchProduct.Quantity >= leftQuantity ? leftQuantity : batchProduct.Quantity;
                    // 出库                   
                    inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchProduct.Id, -transaferQuantity));
                    var fromHistory = new StoreInventoryHistory(item.ProductId, entity.FromStoreId, fromInventoryItem.Quantity, -transaferQuantity,
                batchProduct.Price, batchProduct.BatchNo, entity.Id, entity.Code, BillIdentity.TransferOrder, entity.UpdatedBy,batchProduct.SupplierId);
                    inventoryHistorys.Add(fromHistory);
                    // 入库                    
                    // 记录入库价格, // 修改最后一次进价,如果是赠品，不修改价格
                    toInventoryUpdateModel.LastCostPrice = batchProduct.Price > 0 ? batchProduct.Price : toInventoryItem.LastCostPrice;
                    // 修改库存均价
                    toInventoryUpdateModel.AvgCostPrice = CalculatedAveragePrice(toInventoryItem, batchProduct.Price, transaferQuantity);

                    //记录库存流水
                    var toHistory = new StoreInventoryHistory(item.ProductId, entity.ToStoreId, toInventoryItem.Quantity, transaferQuantity,
                   batchProduct.Price, toBatchNo, entity.Id, entity.Code, BillIdentity.TransferOrder, entity.UpdatedBy,batchProduct.SupplierId);
                    inventoryHistorys.Add(toHistory);
                    // 入库批次时，先检查总库存是否为负库存，有负库存先抵扣负库存。抵扣后库存依然为0，入库批次数量为0，抵扣后有剩余，按剩余数记录入库批次库存                    
                    var batchQuantity = CalculatedBatchQuantity(toInventoryItem.Quantity, transaferQuantity);

                    var batch = new StoreInventoryBatch(item.ProductId, entity.ToStoreId, batchProduct.SupplierId, batchQuantity,
                batchProduct.ContractPrice, batchProduct.Price, toBatchNo, item.ProductionDate, item.ShelfLife, entity.UpdatedBy);
                    inventoryBatchInserts.Add(batch);

                    //总库存
                    fromInventoryItem.Quantity = fromInventoryItem.Quantity - transaferQuantity;
                    toInventoryItem.Quantity = toInventoryItem.Quantity + transaferQuantity;
                    batchProduct.Quantity = batchProduct.Quantity - transaferQuantity;  // 扣减第一个批次为 0
                    //剩余还要扣减数
                    leftQuantity = leftQuantity - transaferQuantity;
                    if (leftQuantity == 0) break;  // 剩余库存扣减完毕，结束当前商品

                }
            }

            _db.Command.AddExecute(UpdateQuantityAndLastCostPriceSql(), inventoryUpdates.ToArray());
            //出批次
            _db.Command.AddExecute(updateInventoryBatchQuantitySql(), inventoryBatchUpdates.ToArray());
            //入批次
            _db.Insert(inventoryBatchInserts.ToArray());
            _db.Insert(inventoryHistorys.ToArray());


        }

        /// <summary>
        /// 按照先进先出重新排序批次，指定批次排第一
        /// </summary>
        /// <param name="inventoryBatchs"></param>
        /// <param name="productId">指定批次商品</param>
        /// <param name="batchNo">指定批次号</param>
        /// <returns></returns>
        private List<StoreInventoryBatch> SortBatchByFIFO(List<StoreInventoryBatch> inventoryBatchs, int productId, long batchNo)
        {
            var currentProductBatchs = inventoryBatchs.Where(n => n.ProductId == productId).OrderBy(n => n.BatchNo).ToList();
            if (currentProductBatchs.Count == 1) return currentProductBatchs;
            if (currentProductBatchs.FirstOrDefault().BatchNo == batchNo) return currentProductBatchs; //指定批次商品就是第一个
            List<StoreInventoryBatch> reSortBatchs = new List<StoreInventoryBatch>();
            var thisBatch = currentProductBatchs.FirstOrDefault(n => n.BatchNo == batchNo);
            if (thisBatch != null) { reSortBatchs.Add(thisBatch); } // 有可能做单子指定的批次已经没库存了          
            // 其他批次按先进先出顺序
            foreach (var item in currentProductBatchs)
            {
                if (item.BatchNo != batchNo)
                {
                    reSortBatchs.Add(item);
                }
            }
            return reSortBatchs;
        }

        /// <summary>
        /// 计算库存移动平均价
        /// </summary>
        /// <param name="inventory">库存数据项</param>
        /// <param name="price">新进商品价格</param>
        /// <param name="quantity">新进数量</param>
        /// <returns></returns>
        private decimal CalculatedAveragePrice(StoreInventory inventory, decimal price, int quantity)
        {
            // 修改库存均价
            int totalQuantity = inventory.Quantity + quantity;
            inventory.AvgCostPrice = totalQuantity == 0 ? price : Math.Round((inventory.AvgCostPrice * inventory.Quantity + price * quantity) / totalQuantity, 4);
            return inventory.AvgCostPrice;
        }

        /// <summary>
        /// 计算批次入库数量。批次入库需考虑库存为负时，抵扣负库存情况
        /// </summary>
        /// <param name="inventoryQuantity">商品当前总库存数</param>
        /// <param name="quantity">入库数</param>
        /// <returns></returns>
        private int CalculatedBatchQuantity(int inventoryQuantity, int quantity)
        {
            var batchQuantity = quantity;
            if (inventoryQuantity < 0)
            {
                batchQuantity = inventoryQuantity + quantity;
                batchQuantity = batchQuantity > 0 ? batchQuantity : 0;
            }
            return batchQuantity;
        }
        public void FixedInventory(StocktakingPlan entity)
        {
            if (entity == null) { throw new Exception("单据不存在"); }
            if (entity.Items.Count() == 0) { throw new Exception("单据明细为空"); }
            //小盘不结转入库
            if (entity.Method == StocktakingPlanMethod.SmallCap)
            {
                return;
            }
            Dictionary<int, StocktakingPlanItem> productQuantityDic = new Dictionary<int, StocktakingPlanItem>();
            //过滤掉盘点没有差异的产品
            var differenceItems = entity.Items.Where(p => p.GetDifferenceQuantity() != 0).ToList();
            if (differenceItems.Count == 0)
            {
                return; //没有差异商品，直接结束
            }
            differenceItems.ForEach((item) =>
            {
                if (!productQuantityDic.ContainsKey(item.ProductId))
                {
                    productQuantityDic.Add(item.ProductId, item);
                }
            });

            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeinventory where storeId=@StoreId and productId in @ProductIds", new { StoreId = entity.StoreId, ProductIds = productIdArray });
            var inventoryBatchs = _db.Table.FindAll<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId in @ProductIds and Quantity>0", new { StoreId = entity.StoreId, ProductIds = productIdArray });
            var inventoryHistorys = new List<StoreInventoryHistory>();
            var inventoryUpdates = new List<StoreInventoryUpdate>();  // 库存更新
            var inventoryBatchUpdates = new List<StoreInventoryBatchUpdate>(); //盘亏批次更新

            var inventoryProfit = new List<StoreInventoryBatch>();  //盘盈批次数据

            long batchNo = 0;
            foreach (var inventory in inventorys)
            {
                if (productQuantityDic.ContainsKey(inventory.ProductId))
                {
                    var purchaseOrderItem = productQuantityDic[inventory.ProductId];
                    // 盘点差异数量，盘盈为整数，盘亏为负数
                    var differenceQuantity = productQuantityDic[inventory.ProductId].GetDifferenceQuantity();

                    // 记录总库存变动
                    var inventoryUpdateModel = new StoreInventoryUpdate();
                    inventoryUpdateModel.Id = inventory.Id;
                    inventoryUpdateModel.Quantity = differenceQuantity;
                    inventoryUpdateModel.SaleQuantity = differenceQuantity;
                    inventoryUpdateModel.AvgCostPrice = inventory.AvgCostPrice;
                    inventoryUpdates.Add(inventoryUpdateModel);

                    var inventoryQuantity = inventory.Quantity;

                    // 盘盈商品，按照最新合同价重新入库
                    if (differenceQuantity > 0)
                    {
                        // 查询商品合同价
                        var contract = _db.Table.Find<PurchaseContract>(@"SELECT c.Id,c.SupplierId from purchasecontract c inner join purchasecontractitem i on c.id = i.PurchaseContractId where FIND_IN_SET(@StoreId,c.StoreIds) and c.`Status`=3 and i.ProductId = @productId order by c.Id desc", new { StoreId = entity.StoreId, ProductId = inventory.ProductId });
                        var contractItem = _db.Table.Find<PurchaseContractItem>("select * from purchasecontractitem where PurchaseContractId=@PurchaseContractId and ProductId=@ProductId", new { PurchaseContractId = contract.Id, ProductId = inventory.ProductId });
                        //重新计算均价成本
                        inventoryUpdateModel.AvgCostPrice = CalculatedAveragePrice(inventory, contractItem.ContractPrice, differenceQuantity);                      

                        if (batchNo == 0)
                        {
                            batchNo = _sequenceService.GenerateBatchNo(entity.StoreId);
                        }

                        //入库批次
                        var batchQuantity = CalculatedBatchQuantity(inventory.Quantity, differenceQuantity);
                        var batch = new StoreInventoryBatch(inventory.ProductId, entity.StoreId, contract.SupplierId, batchQuantity,
                contractItem.ContractPrice, contractItem.ContractPrice, batchNo, null, 0, entity.UpdatedBy);
                        inventoryProfit.Add(batch);

                        //入库历史记录
                        var history = new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventoryQuantity, differenceQuantity,
                            contractItem.ContractPrice, batchNo, entity.Id, entity.Code, ValueObject.BillIdentity.StoreStocktakingPlan, entity.UpdatedBy,contract.SupplierId);
                        inventoryHistorys.Add(history);
                        continue;
                    }

                    //盘亏商品：账面库存大于实际库存，肯定有批次数据可扣减 （此处leftQuantity 为负）
                    var leftQuantity = Math.Abs(differenceQuantity);

                    var productBatchs = inventoryBatchs.Where(n => n.ProductId == inventory.ProductId).OrderBy(n => n.BatchNo);
                    foreach (var batchItem in productBatchs)
                    {
                        if (batchItem.Quantity >= leftQuantity)
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -leftQuantity));
                            //记录修改历史
                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -leftQuantity,
                                batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.StoreStocktakingPlan, entity.UpdatedBy,batchItem.SupplierId));
                            break;
                        }
                        else
                        {
                            inventoryBatchUpdates.Add(new StoreInventoryBatchUpdate(batchItem.Id, -batchItem.Quantity));

                            inventoryHistorys.Add(new StoreInventoryHistory(inventory.ProductId, entity.StoreId, inventory.Quantity, -batchItem.Quantity,
                                                         batchItem.Price, batchItem.BatchNo, entity.Id, entity.Code, BillIdentity.StoreStocktakingPlan, entity.UpdatedBy,batchItem.SupplierId));
                            // 剩余扣减数
                            inventory.Quantity = inventory.Quantity - batchItem.Quantity;  // 第1+N次扣减后总库存
                            leftQuantity = leftQuantity - batchItem.Quantity;
                        }
                    }
                }
            }

            if (inventoryProfit.Count > 0)
            {
                _db.Insert(inventoryProfit.ToArray()); //盘盈批次
            }
            if (inventoryBatchUpdates.Count > 0)
            {
                _db.Command.AddExecute(updateInventoryBatchQuantitySql(), inventoryBatchUpdates.ToArray()); //盘亏批次
            }
            _db.Command.AddExecute(UpdateQuantityAndAvgCostPriceSql(), inventoryUpdates.ToArray()); //总库存
            _db.Insert(inventoryHistorys.ToArray());
        }

        public void UpdateStoreSalePrice(AdjustStorePrice entity)
        {
            if (entity.Items.Count() == 0)
            {
                throw new Exception("明细为空");
            }
            var parma = entity.Items.Select(n => new { StoreId = entity.StoreId, ProductId = n.ProductId, AdjustPrice = n.AdjustPrice }).ToArray();
            string sql = "update StoreInventory set StoreSalePrice =@AdjustPrice where StoreId=@StoreId and ProductId=@ProductId";
            _db.Command.AddExecute(sql, parma);
        }

    }
}
