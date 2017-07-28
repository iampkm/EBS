using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class RealTimeSaleReportDto
    {
        [Description("门店")]
        public string Name { get; set; }
       [Description("订单笔数")]
        /// <summary>
        /// 订单笔数
        /// </summary>
        public int OrderCount { get; set; }
       [Description("客单价")]
       /// <summary>
       /// 客单价
       /// </summary>
       public decimal PerCustomerPrice
       {
           get
           {
               return Math.Round(SaleAmount / OrderCount, 2);
           }
       }
       [Description("销售数量")]
        public int SaleQuantity { get; set; }
        [Description("销售成本")]
        public decimal SaleCostAmount { get; set; }
        [Description("销售金额")]
        public decimal SaleAmount { get; set; }
       
        [Description("毛利额")]
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
        [Description("毛利率")]
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
