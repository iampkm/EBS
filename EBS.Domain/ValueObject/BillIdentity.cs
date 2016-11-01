using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
    /// <summary>
    /// 单据类型
    /// </summary>
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
        /// 采购退单订单
        /// </summary>
        PurchaseBackOrder = 12,
        /// <summary>
        /// 销售订单
        /// </summary>
        SaleOrder = 13,
        /// <summary>
        /// 销售退订单
        /// </summary>
        SaleBackOrder = 14,
        // 通用单据  10~49 ， 门店单据50~-99     
        /// <summary>
        /// 门店采购订单
        /// </summary>
        StorePurchaseOrder = 50,
        /// <summary>
        /// 门店采购退单订单
        /// </summary>
        StorePurchaseBackOrder = 51,
       
        
    }
}
