using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class StocktakingItem:BaseEntity
    {
        
        /// <summary>
        /// 盘点表编号
        /// </summary>
        public int StocktakingId { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal SalesPrice { get; set; }
        /// <summary>
        /// 库存数
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 盘点数
        /// </summary>
        public int CountQuantity { get; set; }
        /// <summary>
        /// 调整数
        /// </summary>
        public int CorectQuantity { get; set; }
        /// <summary>
        /// 差错原因
        /// </summary>
        public string CorectReason { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
    }
}
