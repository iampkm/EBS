using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumPurchaseSaleInventoryDetail
    {
       public int TotalCount { get; set; }
        public int PreInventoryQuantity { get; set; }
        public decimal PreInventoryAmount { get; set; }

        public int PurchaseQuantity { get; set; }

        public decimal PurchaseAmount { get; set; }

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

        public int EndInventoryQuantity { get; set; }

        public decimal EndInventoryAmount { get; set; }
    }
}
