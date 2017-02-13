using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PurchaseSaleInventorySearch
    {
       public int StoreId { get; set; }

       public DateTime StartDate { get; set; }

       public DateTime EndDate { get; set; }
    }
}
