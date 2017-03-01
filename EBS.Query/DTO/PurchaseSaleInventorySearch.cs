using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PurchaseSaleInventorySearch
    {
       /// <summary>
       /// 门店id 多个逗号分隔
       /// </summary>
       public string StoreId { get; set; }

       public int Year { get; set; }

       public int Month { get; set; }
    }
}
