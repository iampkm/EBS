using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    /// <summary>
    /// 商品查询DTO
    /// </summary>
   public class ProductQueryDto
    {
        /// <summary>
        /// 商品SKUID
        /// </summary>
        public int ProductId { get; set; }
        

        public string ProductCode { get; set; }
        public string BarCode { get; set; }

        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 供应商名 
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 合同进价
        /// </summary>
        public decimal ContractPrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>

        public decimal SalePrice { get; set; }
    }
}
