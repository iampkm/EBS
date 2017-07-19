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

       public string Status { get; set; }

       public int SupplierId { get; set; }

       /// <summary>
       /// 多个逗号分隔
       /// </summary>
       public string StoreId { get; set; }
       public string ProductCodeOrBarCode { get; set; }

       public string ProductName { get; set; }

       public DateTime? StartDate { get; set; }

       public DateTime? EndDate { get; set; }

       /// <summary>
       /// 业务类别
       /// </summary>
       public int OutInOrderTypeId { get; set; }

       /// <summary>
       /// 出入库类别
       /// </summary>
       public int OutInInventory { get; set; }

       public string AuditName { get; set; }
    }
}
