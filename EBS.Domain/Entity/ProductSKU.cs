using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class ProductSku:BaseEntity
    {
        public ProductSku()
        {
            this.CreatedOn = DateTime.Now;
        }
       /// <summary>
       /// 商品编码
       /// </summary>
       public int ProductId { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
       public string Code { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
       public string BarCode { get; set; }
        /// <summary>
       /// 规格列表，格式：规格ID：规格选项值；规格ID：规格选项值；
        /// </summary>
       public string SpecificationList { get; set; }
        /// <summary>
       /// 规格
        /// </summary>
       public string Specification { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
       public decimal MarketPrice { get; set; }
       /// <summary>
       /// 销售价
       /// </summary>
       public decimal SalePrice { get; set; }
        /// <summary>
        /// 批发价
        /// </summary>
       public decimal WholeSalePrice { get; set; }
        /// <summary>
        /// 移动平均成本价
        /// </summary>
       public decimal CostPrice { get; set; }       
       /// <summary>
       /// 子SKU编码 ：  用于整件拆零，例如一条烟 20包 ，这里配置一包烟的 SKU 编码
       /// </summary>
       public string SubSkuCode { get; set; }
       /// <summary>
       /// 子SKU 可拆数量  ： 用于整件拆零，例如一条烟 20包 ，这里配置 20 包
       /// </summary>
       public int SubSkuQuantity { get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>
       public DateTime CreatedOn { get; set; }

    }
}
