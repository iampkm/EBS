using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class ProductCheckDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 商品名称，长度限制20个字符内
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        public string Unit { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }

        //public string AreaName { get; set; }

        //public decimal AreaSalePrice { get; set; }

        //public string StoreName { get; set; }
        //public Decimal StorePrice { get; set; }

        //public decimal VipSalePrice { get; set; }
    }
}
