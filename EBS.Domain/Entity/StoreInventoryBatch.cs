using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class StoreInventoryBatch:BaseEntity
    {
        public StoreInventoryBatch() { }

        public StoreInventoryBatch(int productId, int storeId, int supplierId, int quantity,decimal contractPrice, decimal price, long batchNo,
                   DateTime? productionDate, int shelfLife,  int createdBy)
        {
            this.ProductId = productId;
            this.StoreId = storeId;
            this.SupplierId = supplierId;
            this.Quantity = quantity;
            this.Price = price;
            this.BatchNo = batchNo;
            this.ProductionDate = productionDate;
            this.ShelfLife = shelfLife;
            this.CreatedBy = createdBy;
            this.CreatedOn = DateTime.Now;
            this.ContractPrice = contractPrice;
        }

        /// <summary>
        /// 商品SKUID
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public int StoreId { get; set; }

        public int SupplierId { get; set; }
        /// <summary>
        /// 库存数
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 保质期：单位天
        /// </summary>
        public int ShelfLife { get; set; }
        /// <summary>
        /// 合同价
        /// </summary>
        public decimal ContractPrice { get; set; }

        /// <summary>
        /// 实际进货价 （赠品价位 0）
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public long BatchNo { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        /// <summary>
        /// 行版本，并发控制字段
        /// </summary>
        public DateTime RowVersion { get; private set; }
    }
}
