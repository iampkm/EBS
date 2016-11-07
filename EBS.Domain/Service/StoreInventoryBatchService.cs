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
   public class StoreInventoryBatchService
    {
        IDBContext _db;
        public StoreInventoryBatchService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void SaveBatch(StorePurchaseOrder entity)
        {
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == entity.Id);
            var inventoryBatchs = new List<StoreInventoryBatch>();
            foreach (var item in entityItems)
            {
                //批次                   
                var batch = new StoreInventoryBatch(item.ProductId, entity.StoreId, entity.SupplierId, item.ActualQuantity,
                    item.Price, entity.BatchNo, item.ProductionDate, item.ShelfLife, entity.StoragedBy);
                inventoryBatchs.Add(batch);               
            }
            _db.Insert(inventoryBatchs.ToArray());
        }
    }
}
