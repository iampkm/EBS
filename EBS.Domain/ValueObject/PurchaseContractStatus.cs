﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
    public enum PurchaseContractStatus
    {
        [Description("作废")]
        Cancel = -1,
        [Description("初始")]
        Create = 1,
        [Description("待审")]
        WaitingAudit = 2,
        [Description("已审")]
        Audited = 3
      
    }
}
