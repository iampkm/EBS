using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class SaleOrderItem:BaseEntity
    {
        public int SaleOrderId { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 销项税率
        /// </summary>
       // public decimal OutRate { get; set; }
        /// <summary>
        /// 平均成本
        /// </summary>
        public decimal AvgCostPrice { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 实际折后价
        /// </summary>
        public decimal RealPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
