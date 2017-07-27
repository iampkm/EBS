using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PurchaseSaleInventoryDetailSearch
    {
       public string ProductCodeOrBarCode { get; set; }

       public string productName { get; set; }
       public string CategoryId { get; set; }

       public string StoreId { get; set; }

       public int Year { get; set; }

       public int Month { get; set; }
    }
}
