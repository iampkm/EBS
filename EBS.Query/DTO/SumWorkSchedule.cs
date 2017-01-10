using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumWorkSchedule
    {
        /// <summary>
        /// 缴款汇总
        /// </summary>
         public decimal CashAmount { get; set; }

        /// <summary>
        /// 销售金额汇总
        /// </summary>
         public decimal TotalAmount { get; set; }

         public decimal TotalOnlineAmount { get; set; }
    }
}
