using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{

   public enum TransferOrderStatus
    {
        [Description("作废")]
        Cancel = -1,
        [Description("待审")]
        WaitAudit = 1,
        [Description("已审")]
        Audited = 2
    }
}
