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
        /// 批次库存数量
        /// </summary>
        public int BatchQuantity { get; set; }
        
        /// <summary>
        /// 成本均价
        /// </summary>
        public decimal AvgCostPrice { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 售价
        /// </summary>

        public decimal SalePrice { get; set; }

        /// <summary>
        /// 门店售价
        /// </summary>

        public decimal StoreSalePrice { get; set; }
        /// <summary>
        /// 会员售价
        /// </summary>

        public decimal VipSalePrice { get; set; }
        
       /// <summary>
       /// 毛利
       /// </summary>
        public string ProfitAmount { get {           
            var realPrice = StoreSalePrice == 0 ? SalePrice : StoreSalePrice;
            if (realPrice == 0) return "0.00";
            return (realPrice - Price).ToString("F2");
        } }

        /// <summary>
        /// 毛利率 
        /// </summary>
        public string ProfitPercent { get {
            var realPrice = StoreSalePrice == 0 ? SalePrice : StoreSalePrice;
            if (realPrice == 0) return "0.00";
            decimal result = Math.Round((realPrice - Price) / realPrice * 100, 2);
            return result.ToString("F2")+"%";
        } }
    }
}
