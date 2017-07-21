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

       /// <summary>
       /// 出入库类型：  1 入库，-1 出库
       /// </summary>
       public OutInInventoryType OutInInventory { get; set; }
    }
}
