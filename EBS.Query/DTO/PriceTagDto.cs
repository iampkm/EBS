using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PriceTagDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 商品名称，长度限制20个字符内
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Grade { get; set; }

        public string Unit { get; set; }
        /// <summary>
        /// 产地
        /// </summary>
        public string MadeIn { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }


    }
}
