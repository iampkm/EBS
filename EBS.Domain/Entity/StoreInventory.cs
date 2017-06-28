using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    public class StoreInventory : BaseEntity
    {
        public StoreInventory() {
            this.IsQuit = false;
            this.Status = StoreInventoryStatus.Normal;
        }
        public StoreInventory(int storeId,int productId,int quantity):this() {
            this.StoreId = storeId;
            this.ProductId = productId;         
            this.Quantity += quantity;
        }
        /// <summary>
        /// 商品SKUID
        /// </summary>
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        /// <summary>
        /// 可售总数量
        /// </summary>
        public int SaleQuantity { get; set; }
        /// <summary>
        /// 预订总数量
        /// </summary>
        public int OrderQuantity { get; set; }
        /// <summary>
        /// 库存总数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 移动加权平均成本
        /// </summary>
        public decimal AvgCostPrice { get; set; }
        /// <summary>
        /// 预警数量
        /// </summary>
        public int WarnQuantity { get; set; }
        /// <summary>
        /// 是否退市（例如不再进货销售）
        /// </summary>
        public bool IsQuit { get; set; }
        /// <summary>
        /// 库存状态
        /// </summary>
        public StoreInventoryStatus Status { get; set; }
        /// <summary>
        /// 最后一次进价，如果是赠品，价格为0 的，不更新此价格
        /// </summary>
        public decimal LastCostPrice { get; set; }
        /// <summary>
        /// 门店售价
        /// </summary>
        public decimal StoreSalePrice { get; set; }

        /// <summary>
        /// 行版本，并发控制字段
        /// </summary>
        public DateTime RowVersion { get; private set; }
    }
}
