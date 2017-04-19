using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SaleReportDto
    {
       public string Name { get; set; }

       /// <summary>
       /// 订单笔数
       /// </summary>
       public int OrderCount { get; set; }

       public int SaleQuantity { get; set; }

       public decimal SaleCostAmount { get; set; }

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
