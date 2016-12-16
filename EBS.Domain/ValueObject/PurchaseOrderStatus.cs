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
        [Description("初始")]
        Create = 1,
        [Description("待入库")]
        WaitStockIn = 2,
        [Description("待出库")]
        WaitStockOut = 3,
        [Description("已完成")]
        Finished = 4,
        [Description("财务已审")]
        FinanceAuditd =5


    }

    // 采购单：  初始， 待收货，待入库，完成
    // 采购退单： 待出库，完成
}
