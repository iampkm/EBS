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
        [Description("待收货")]
        WaitReceivedGoods = 2,
        [Description("待入库")]
        WaitingStockIn = 3,
        [Description("已入库")]
        HadStockIn = 4

    }
}
