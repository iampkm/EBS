using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class OutInOrderItem:BaseEntity
    {
       public int OutInOrderId { get; set; }
       public int ProductId { get; set; }
       /// <summary>
       /// 数量
       /// </summary>
       public int Quantity { get; set; }

       public decimal CostPrice { get; set; }

       /// <summary>
       ///  加减：入库 1 ，出库  -1 
       /// </summary>
       public int PlusMinus { get; set; }
    }
}
