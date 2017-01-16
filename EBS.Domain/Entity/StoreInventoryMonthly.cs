using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 库存月报
    /// </summary>
   public class StoreInventoryMonthly:BaseEntity
    {
       public StoreInventoryMonthly()
       {
           this.Monthly = DateTime.Now.ToString("yyyy-MM");
       }

       public string Monthly { get; set; }
        /// <summary>
        /// 商品SKUID
        /// </summary>
        public int ProductId { get; set; }
        public int StoreId { get; set; }       
        /// <summary>
        /// 库存总数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 移动加权平均成本
        /// </summary>
        public decimal AvgCostPrice { get; set; }
    }
}
