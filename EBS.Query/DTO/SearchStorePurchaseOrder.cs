using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchStorePurchaseOrder
    {
       public string Code { get; set; }

       public int SupplierId { get; set; }

       public int StoreId { get;set; }

       public string ProductCodeOrBarCode { get; set; }

       public int Status { get; set; }

       public int OrderType { get; set; }
    }
}
