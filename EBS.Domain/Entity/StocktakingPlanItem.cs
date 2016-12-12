using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class StocktakingPlanItem:BaseEntity
    {
        public int StocktakingPlanId { get; set; }
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
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public string CostPrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public string SalesPrice  { get; set; }
 
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 盘点数量
        /// </summary> 
        public int CountQuantity { get; set; }

        /// <summary>
        /// 差异数量  盘点数-库存数
        /// </summary>
        /// <returns></returns>
        public int GetDifferenceQuantity()
        {
            return this.CountQuantity - this.Quantity;
        }
    }
}
