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

        public int SupplierId { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 保质期：单位天
        /// </summary>
        public int ShelfLife { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 合同价
        /// </summary>
        public decimal ContractPrice { get; set; }

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
