using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class PurchaseContractItem:BaseEntity
    {
       public int PurchaseContractId {get;set;}
       public int ProductId { get; set; }
       /// <summary>
       /// 合同价
       /// </summary>
       public decimal ContractPrice { get; set; }

        // 淘汰，正常，滞销，只销不进，只进不销
    }
}
