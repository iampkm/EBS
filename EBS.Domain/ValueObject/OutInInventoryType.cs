using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
    public enum OutInInventoryType
    {        
        [Description("出库")]
        Out = -1,
        [Description("入库")]
        In = 1,
    }
}
