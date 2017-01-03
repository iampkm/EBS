using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SaleOrderItemDto
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
        public string BarCode { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
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

        public decimal DiscountAmount {
            get {
                return (SalePrice - RealPrice) * Quantity;
            }
        }

        public decimal Amount {
            get {
                return RealPrice * Quantity;
            }
        }
    }
}
