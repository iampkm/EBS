using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class OutInOrderType:BaseEntity
    {
       public string TypeName { get; set; }

       public OutInInventoryType OutInInventory { get; set; }
    }
}
