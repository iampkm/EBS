using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public enum AdjustSalePriceStatus
    {
        [Description("作废")]
        Cancel = -1,
        [Description("未生效")]
        InValid = 1,
        [Description("已生效")]
        Valid = 2
        //[Description("已审")]
        //Audited = 3
    }
}
