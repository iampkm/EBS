﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public enum AdjustContractPriceStatus
    {
        [Description("作废")]
        Cancel = -1,
        [Description("待审")]
        WaitingAudit = 1,
        [Description("已审")]
        Audited = 2
    }
}
