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
       public int ProductSkuId { get; set; }
       public decimal CostPrice { get; set; }
    }
}
