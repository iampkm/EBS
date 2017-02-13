using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PurchaseSaleInventoryDto
    {
       public string StoreName { get; set; }

       //public string Month { get; set; }

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

       public int EndInventoryQuantity { get; set; }

       public decimal EndInventoryAmount { get; set; }
    }
}
