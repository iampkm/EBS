using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class TransferOrderItem:BaseEntity
    {
        public int TransferOrderId { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
       /// <summary>
       /// 库存成本
       /// </summary>
        public decimal Price { get; set; }
        
       /// <summary>
       /// 调拨批次
       /// </summary>
        public long BatchNo { get; set; }

    }
}
