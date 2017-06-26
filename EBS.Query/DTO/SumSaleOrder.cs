using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumSaleOrder
    {
        /// <summary>
        /// 销售数合计
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 销售金额合计
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 均价成本汇总
        /// </summary>
        public decimal CostAmount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
