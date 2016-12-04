using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
  public enum OrderType
    {
        [Description("订单")]
        /// <summary>
        /// 订单
        /// </summary>
        Order = 1,
        [Description("退单")]
        /// <summary>
        /// 退单
        /// </summary>
        Refund = 2,
    }
}
