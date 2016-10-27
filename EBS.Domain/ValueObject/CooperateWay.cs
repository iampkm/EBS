using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
    /// <summary>
    /// 合作方式
    /// </summary>
    public enum CooperateWay
    {
         [Description("经销")]
        /// <summary>
        /// 经销
        /// </summary>
        SellBySelf = 1,
         [Description("代销")]
        /// <summary>
        /// 代销
        /// </summary>
        SellByProxy = 2,
         [Description("联营")]
        /// <summary>
        /// 联营
        /// </summary>
        SellJoin = 3
    }
}
