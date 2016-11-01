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
        [Description("创建")]
        Create = 1,
        [Description("待入库")]
        WaitingStockIn = 2,
        [Description("已入库")]
        HadStockIn = 3,
        [Description("作废")]
        Cancel = 4
    }
}
