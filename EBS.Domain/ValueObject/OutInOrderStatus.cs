﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace EBS.Domain.ValueObject
{
   public enum OutInOrderStatus
    {
        [Description("作废")]
        Cancel = -1,
        [Description("初始")]
        Create = 1,
        [Description("待审")]
        WaitAudit = 2,
        [Description("已完成")]
        Audited = 3,
        [Description("财务复审")]
        FinanceAudited = 4
    }
}
