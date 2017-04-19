using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SingleProductSaleDto
    {
       public string StoreName { get; set; }
        public string BarCode { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public int SaleQuantity { get; set; }
        /// <summary>
        /// 销售成本金额
        /// </summary>
        public decimal SaleCostAmount { get; set; }
        /// <summary>
        /// 销售售价金额
        /// </summary>
        public decimal SaleAmount { get; set; }

        /// <summary>
        /// 毛利额
        /// </summary>
        public decimal ProfitAmount
        {
            get
            {
                return SaleAmount - SaleCostAmount;
            }
        }
        /// <summary>
        /// 毛利率
        /// </summary>
        public decimal ProfitPercent
        {
            get
            {
                if (SaleAmount == 0) { return 0; }
                var result = decimal.Round((SaleAmount - SaleCostAmount) / SaleAmount * 100, 2);
                return result;
            }
        }
    }
}
