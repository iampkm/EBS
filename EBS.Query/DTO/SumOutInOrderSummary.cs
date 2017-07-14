using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumOutInOrderSummary
    {
        /// <summary>
        /// 数量合计
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///金额合计
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
