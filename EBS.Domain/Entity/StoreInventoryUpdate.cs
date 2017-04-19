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
        /// <summary>
        /// 增减数库存数 
        /// </summary>
         public int Quantity { get; set; }
         /// <summary>
         /// 可售库存
         /// </summary>
         public int SaleQuantity { get; set; }
         /// <summary>
         /// 订购库存
         /// </summary>
         public int OrderQuantity { get; set; }
         /// <summary>
         /// 库存商品移动加权平均价
         /// </summary>
         public decimal AvgCostPrice { get; set; }

        /// <summary>
        /// 最后一次进价
        /// </summary>
         public decimal LastCostPrice { get; set; }

    }


}
