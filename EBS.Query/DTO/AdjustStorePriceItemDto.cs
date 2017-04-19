using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class AdjustStorePriceItemDto
    {
        public int ProductId { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        public string BarCode { get; set; }

        public string Specification { get; set; }

        public string Unit { get; set; }
        /// <summary>
        /// 最新进价
        /// </summary>
        public decimal LastCostPrice { get; set; }
       /// <summary>
       /// 总部指导价
       /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 门店售价
        /// </summary>
        public decimal StoreSalePrice { get; set; }
       /// <summary>
       /// 调整价
       /// </summary>
        public decimal AdjustPrice { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal Profit { get {
            var result = AdjustPrice - LastCostPrice;
            return result;
        } }

        /// <summary>
        /// 毛利率
        /// </summary>
        public decimal ProfitMargin { get {
            if (this.AdjustPrice == 0)
            {
                return 0;
            }
            return Math.Round(this.Profit / this.AdjustPrice, 2);
        } }
    }
}
