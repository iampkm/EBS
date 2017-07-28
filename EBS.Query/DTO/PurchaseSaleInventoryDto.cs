using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PurchaseSaleInventoryDto
    {
      
       /// <summary>
       /// 门店Id
       /// </summary>
       public int StoreId { get; set; }
       [Description("日期")]
       public string YearMonth { get; set; }
        [Description("门店名")]
       public string StoreName { get; set; }      
         [Description("期初库存")]
       public int PreInventoryQuantity { get; set; }
         [Description("期初库存金额")]
       public decimal PreInventoryAmount { get; set; }
         [Description("本期入库")]
       public int PurchaseQuantity { get; set; }
         [Description("本期入库金额")]
       public decimal PurchaseAmount { get; set; }
         [Description("本期销售数")]
       public int SaleQuantity { get; set; }
         [Description("本期销售成本金额")]
       /// <summary>
       /// 销售成本金额
       /// </summary>
       public decimal SaleCostAmount { get; set; }
         [Description("本期销售金额")]
       /// <summary>
       /// 销售售价金额
       /// </summary>
       public decimal SaleAmount { get; set; }
         [Description("毛利额")]
       /// <summary>
       /// 毛利额
       /// </summary>
       public decimal ProfitAmount { get {
           return SaleAmount - SaleCostAmount;
       } }
         [Description("毛利率")]
       /// <summary>
       /// 毛利率
       /// </summary>
       public decimal ProfitPercent { get {
           if (SaleAmount == 0) { return 0; }
           var result = decimal.Round((SaleAmount - SaleCostAmount) / SaleAmount * 100,2);
           return result;
       } }
         [Description("期末库存")]
       public int EndInventoryQuantity { get; set; }
         [Description("期末库存金额")]
       public decimal EndInventoryAmount { get; set; }

       public string StartDate { get; set; }

       public string EndDate { get; set; }
    }
}
