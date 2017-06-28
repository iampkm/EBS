using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 库存更新实体
    /// </summary>
   public class StoreInventoryUpdate:BaseEntity
    {
       //public StoreInventoryUpdate() { }
       //public StoreInventoryUpdate(int id,int quantity,int saleQuantity,int SourceQuantity,decimal avgCostPrice=0,decimal lastCostPrice=0) {
       //    this.Id = id;
       //    this.Quantity = quantity;
       //    this.SaleQuantity = saleQuantity;
       //    this.SourceQuantity = SourceQuantity;
       //    this.AvgCostPrice = avgCostPrice;
       //    this.LastCostPrice =lastCostPrice;
       //}

        /// <summary>
        /// 增减数库存数 
        /// </summary>
         public int Quantity { get; set; }
         /// <summary>
         /// 可售库存
         /// </summary>
         public int SaleQuantity { get; set; }

         /// <summary>
         /// 库存商品移动加权平均价
         /// </summary>
         public decimal AvgCostPrice { get; set; }

        /// <summary>
        /// 最后一次进价
        /// </summary>
         public decimal LastCostPrice { get; set; }
         /// <summary>
         /// 更新前库存数
         /// </summary>
         public int SourceQuantity { get; set; }

    }


}
