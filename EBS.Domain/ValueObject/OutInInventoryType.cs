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
        [Description("入库")]
        /// <summary>
        /// 入库
        /// </summary>
        In = 1,
        [Description("出库")]
        /// <summary>
        /// 出库
        /// </summary>
        Out = 2,
    }
}
