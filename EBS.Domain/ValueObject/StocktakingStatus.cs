using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public enum StocktakingStatus
    {
        [Description("作废")]
       Cancel = -1,
        /// <summary>
        /// 待审
        /// </summary>
        [Description("待审")]
        WaitAuditing = 1,
        /// <summary>
        /// 已审
        /// </summary>
        [Description("已审")]
        Audited = 2,
    }
}
