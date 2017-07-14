using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchOutInOrder
   {
       public string Code { get; set; }

       public int Status { get; set; }

       /// <summary>
       /// 多个逗号分隔
       /// </summary>
       public string StoreId { get; set; }
       public string ProductCodeOrBarCode { get; set; }

       public string ProductName { get; set; }

       public DateTime? StartDate { get; set; }

       public DateTime? EndDate { get; set; }
    }
}
