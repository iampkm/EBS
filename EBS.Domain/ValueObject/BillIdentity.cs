using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public enum BillIdentity
    {
       /// <summary>
       ///  采购合同
       /// </summary>
        PurchaseContract = 10 ,
       /// <summary>
       /// 采购订单
       /// </summary>
        PurchaseOrder = 11,
        /// <summary>
        /// 销售订单
        /// </summary>
        SaleOrder = 12
        
    }
}
