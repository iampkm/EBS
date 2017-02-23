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

       public int StoreId { get; set; }

       public int Year { get; set; }

       public int Month { get; set; }
    }
}
