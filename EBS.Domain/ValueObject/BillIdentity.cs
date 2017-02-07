using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("销售订单")]
        /// <summary>
        /// 销售订单
        /// </summary>
        SaleOrder = 1,
        [Description("销售退单")]
        /// <summary>
        /// 销售退订单
        /// </summary>
        SaleRefund = 2,

        [Description("采购合同")]
        /// <summary>
        ///  采购合同
        /// </summary>
        PurchaseContract = 10 ,
        [Description("采购订单")]
        /// <summary>
        /// 采购订单
        /// </summary>
        PurchaseOrder = 11,
        [Description("采购退单")]
        /// <summary>
        /// 采购退单订单
        /// </summary>
        PurchaseBackOrder = 12,
        
        [Description("合同调价单")]
        /// <summary>
        /// 合同调价单
        /// </summary>
        AdjustContractPrice = 15,
        [Description("商品调价单")]
        /// <summary>
        /// 商品调价单
        /// </summary>
        AdjustSalePrice = 16,


        // 通用单据  10~49 ， 门店单据50~-99
        [Description("采购订单")]      
        /// <summary>
        /// 门店采购订单
        /// </summary>
        StorePurchaseOrder = 51,
        [Description("采购退单")]
        /// <summary>
        /// 门店采购退单订单
        /// </summary>
        StorePurchaseRefundOrder = 52,
        [Description("门店盘点")]
        StoreStocktakingPlan = 53,
        [Description("盘点单")]
        StoreStocktaking = 54,
        [Description("盘点修正单")]
        StoreStocktakingEdit = 55,

        [Description("调拨单")]
        TransferOrder = 60,
        
       
        
    }
}
