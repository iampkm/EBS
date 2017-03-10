using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class AdjustStorePriceItem:BaseEntity
    {
        
        public int AdjustStorePriceId { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal StoreSalePrice { get; set; }
        /// <summary>
        /// 调整价
        /// </summary>
        public decimal AdjustPrice { get; set; }
    }
}
