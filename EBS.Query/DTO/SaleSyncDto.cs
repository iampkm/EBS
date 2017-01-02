using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SaleSyncDto
    {
        public string StoreName { get; set; }
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
        public string ClientUpdatedOn { get; set; }

        /// <summary>
        /// 服务端订单笔数
        /// </summary>
        public int ServerOrderCount { get; set; }
        /// <summary>
        /// 服务端销售总金额
        /// </summary>
        public decimal ServerOrderTotalAmount { get; set; }
    }
}
