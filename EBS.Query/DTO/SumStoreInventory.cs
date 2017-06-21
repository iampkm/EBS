using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumStoreInventory
    {
       public int Quantity { get; set; }

       public decimal Amount { get; set; }

       public decimal SaleAmount { get; set; }

       public int TotalCount { get; set; }
    }
}
