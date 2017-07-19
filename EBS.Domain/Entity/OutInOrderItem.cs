using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure;
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

       public decimal LastCostPrice { get; set; }

       public decimal CostPrice { get; set; }

       /// <summary>
       ///  加减：入库 1 ，出库  -1 
       /// </summary>
       public int PlusMinus { get;private set; }

       public void SetPlusMinus(OutInOrderType orderType)
       {
           if (orderType.OutInInventory == 1)
           {
               this.PlusMinus = 1;
           }
           else if (orderType.OutInInventory == 2)
           {
               this.PlusMinus = -1;
           }
           else {
               throw new FriendlyException("出入库类别参数异常");
           }
       }
    }
}
