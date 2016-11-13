using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    public class AdjustSalePriceItemDto
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

        public decimal ContractPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal AdjustPrice { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal Profit { get; set; }
       
        /// <summary>
        /// 毛利率
        /// </summary>
        public decimal ProfitMargin { get; set; }
        
    }
}
