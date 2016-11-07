using System;
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
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == entity.Id);
            Dictionary<int, StorePurchaseOrderItem> productQuantityDic = new Dictionary<int, StorePurchaseOrderItem>();
            entityItems.ToList().ForEach(item => productQuantityDic.Add(item.ProductId,item));
            var productIdArray = productQuantityDic.Keys.ToArray();
            var inventorys = _db.Table.FindAll<StoreInventory>("select * from storeInventorty where productId in @ProductIds", new { ProductIds = productIdArray });
            var inventoryHistorys = new List<StoreInventoryHistory>();
            foreach (var product in inventorys)
            {
                if (productQuantityDic.ContainsKey(product.ProductId))
                {
                    var addQuantity = productQuantityDic[product.ProductId].Quantity;
                    var price = productQuantityDic[product.ProductId].Price;
                    product.Quantity += addQuantity;
                    product.SaleQuantity += addQuantity;

                    //记录修改历史
                    var history = new StoreInventoryHistory(product.ProductId,entity.StoreId,product.Quantity, addQuantity, 
                        price,entity.BatchNo,entity.Id,entity.Code,ValueObject.BillIdentity.StorePurchaseOrder,entity.StoragedBy);
                    inventoryHistorys.Add(history);
                }
            }
            _db.Update(inventorys.ToArray());
            _db.Insert(inventoryHistorys.ToArray());
        }

        
    }
}
