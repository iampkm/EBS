using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumStocktakingSummary
    {
       public int TotalCount { get; set; }

       public int TotalDifferentQuantity { get; set; }

       public decimal CostAmountDifferent { get; set; }

       public decimal SaleAmoutDifferent { get; set; }

    }
}
