using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class ProductDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 商品名称，长度限制20个字符内
        /// </summary>
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        /// 是否发布上架
        /// </summary>
        public bool IsPublish { get; set; }

        public string Publish
        {
            get { 
                return IsPublish?"是":"否";
            }
        }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }       
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }
       
        /// <summary>
        /// 移动平均成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        
    }
}
