using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 调整售价明细
    /// </summary>
   public class AdjustSalePriceItem:BaseEntity
    {
        // 显示 商品编码，品名，类别，库存，进价（平均价），售价，毛利率，毛利额
        public int AdjustSalePriceId { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 合同价
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 调整价
        /// </summary>
        public decimal AdjustPrice { get; set; }
    }
}
