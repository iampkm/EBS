using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchSingleProductSale
    {
       public string ProductCodeOrBarCode { get; set; }
       /// <summary>
       /// 门店Id，多个逗号分隔
       /// </summary>
       public string StoreId { get; set; }

       public DateTime? StartDate { get; set; }

       public DateTime? EndDate { get; set; }
    }
}
