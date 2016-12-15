using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
    public enum PurchaseOrderStatus
    {
        [Description("作废")]
        Cancel = -1,
        //[Description("初始")]
        //Create = 1,
        //[Description("待收货")]
        //WaitReceivedGoods = 2,
        [Description("待入库")]
        WaitStockIn = 1,
        [Description("待出库")]
        WaitStockOut = 2,
        [Description("已完成")]
        Finished = 3

    }

    // 采购单：  初始， 待收货，待入库，完成
    // 采购退单： 待出库，完成
}
