using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchTransferOrder
    {
       public string Code { get; set; }

       public int Status { get; set; }

       /// <summary>
       /// 多个逗号分隔
       /// </summary>
       public string StoreId { get; set; }
       /// <summary>
       /// 调出
       /// </summary>
       public bool? From { get; set; }
       /// <summary>
       /// 调入
       /// </summary>
       public bool? To { get; set; }

       public DateTime? StartDate { get; set; }

       public DateTime? EndDate { get; set; }

       public string ProductCodeOrBarCode { get; set; }
    }
}
