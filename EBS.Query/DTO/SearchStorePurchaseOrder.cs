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

       public string StoreId { get;set; }

       public string ProductCodeOrBarCode { get; set; }

       /// <summary>
       /// 逗号分隔数字
       /// </summary>
       public string Status { get; set; }

       public int OrderType { get; set; }

       public DateTime? StartDate { get; set; }

       public DateTime? EndDate { get; set; }
    }
}
