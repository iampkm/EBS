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
        BillSequenceService _sequenceService;
        public StoreInventoryBatchService(IDBContext dbContext)
        {
            this._db = dbContext;
            _sequenceService = new BillSequenceService(this._db);
        }

        public List<StoreInventoryBatch> SaveBatch(StorePurchaseOrder entity)
        {
            var inventoryBatchs = new List<StoreInventoryBatch>();
            var batchNo = _sequenceService.GenerateBatchNo(entity.StoreId);
            foreach (var item in entity.Items)
            {
                //批次 
                item.BatchNo = batchNo;                  
                var batch = new StoreInventoryBatch(item.ProductId, entity.StoreId, entity.SupplierId, item.ActualQuantity,
                    item.ContractPrice,item.Price, item.BatchNo, item.ProductionDate, item.ShelfLife, entity.StoragedBy);
                inventoryBatchs.Add(batch);               
            }
            return inventoryBatchs;
        }
    }
}
