using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class OutInOrderSummaryDto
    {
       public string StoreName { get; set; }

       public string TypeName { get; set; }

       public int Quantity { get; set; }

       public decimal Amount { get; set; }
    }
}
