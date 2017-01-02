using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 客户端销售同步
    /// </summary>
   public class SaleSync:BaseEntity
    {
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string SaleDate { get; set; }
        public int StoreId { get; set; }
        public int PosId { get; set; }
        /// <summary>
        /// 订单笔数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 销售总金额
        /// </summary>
        public decimal OrderTotalAmount { get; set; }
        /// <summary>
        /// 客户端更新时间
        /// </summary>
        public DateTime ClientUpdatedOn { get; set; }
    }
}
