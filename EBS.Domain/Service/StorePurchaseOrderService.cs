using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
using EBS.Infrastructure;
namespace EBS.Domain.Service
{
   public class StorePurchaseOrderService
    {
        IDBContext _db;
        BillSequenceService _sequenceService;
        StoreInventoryService _storeInventoryService;
        public StorePurchaseOrderService(IDBContext dbContext)
        {
            this._db = dbContext;
            _sequenceService = new BillSequenceService(_db);
            _storeInventoryService = new StoreInventoryService(_db);
        }
        public List<StorePurchaseOrder> SplitOrderItem(StorePurchaseOrder model)
        {
            var splitNumber = 10;
            List<StorePurchaseOrderItem> items = new List<StorePurchaseOrderItem>();
            List<StorePurchaseOrder> entitys = new List<StorePurchaseOrder>();
            foreach (var item in model.Items)
            {
                if (items.Count == splitNumber)
                {
                    StorePurchaseOrder entity = new StorePurchaseOrder();
                    entity.SetItems(items);
                    entitys.Add(entity);
                    items = new List<StorePurchaseOrderItem>(); // 重新分配一个
                }
                var product = _db.Table.Find<Product>(item.ProductId);
                if (model.OrderType == ValueObject.OrderType.Refund)
                {
                    //检查库存
                    var inventoryModel = _db.Table.Find<StoreInventory>(n => n.ProductId == item.ProductId && n.StoreId == model.StoreId);                   
                    if (item.Quantity > inventoryModel.Quantity)
                    {
                        throw new FriendlyException(string.Format("商品{0}退货数量{1} > 库存数{2}", product.Code, item.Quantity, inventoryModel.Quantity));
                    }
                }
                else {
                    if (item.Quantity <= 0) { throw new FriendlyException(string.Format("{0}:数量不能小于等于0", product.Code)); }
                }
                var itemProduct = items.FirstOrDefault(n => n.ProductId == item.ProductId);
                if (itemProduct == null)
                {
                    items.Add(item);
                }
                else {
                    itemProduct.Quantity += item.Quantity;
                }               
            }
            if (items.Count > 0 && items.Count <= splitNumber)
            {
                StorePurchaseOrder entity = new StorePurchaseOrder();
                entity.SetItems(items);
                entitys.Add(entity);
            }
            return entitys;
        }

        public void UpdateWithItem(StorePurchaseOrder model)
        {
            if (_db.Table.Exists<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == model.Id))
            {
                _db.Delete<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == model.Id);
            }          
            _db.Insert<StorePurchaseOrderItem>(model.Items.ToArray());
            _db.Update(model);
        }
      
    }
}
