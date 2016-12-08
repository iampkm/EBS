using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public enum StocktakingPlanMethod
    {
       /// <summary>
        /// 大盘
        /// </summary>
        [Description("大盘")]
        Market = 1,
        /// <summary>
        /// 小盘
        /// </summary>
        [Description("小盘")]
        SmallCap = 2
    }
}
