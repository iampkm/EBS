using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class StoreInventoryQueryDto
    {
        /// <summary>
        /// 商品SKUID
        /// </summary>
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        [Description("门店")]
        public string StoreName { get; set; }
        [Description("商品编码")]
        public string ProductCode { get; set; }
        [Description("条码")]
        public string BarCode { get; set; }
       
        [Description("品名")]
        public string ProductName { get; set; }
        [Description("规格")]
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        [Description("品类")]
        public string CategoryName { get; set; }
        [Description("库存")]
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Quantity { get; set; }
        [Description("最新进价")]
        public decimal LastCostPrice { get; set; }
        [Description("成本金额")]
        public decimal Amount
        {
            get
            {
                return LastCostPrice * Quantity;
            }
        }
        [Description("售价")]
        public decimal SalePrice { get; set; }
        [Description("销售金额")]
        public decimal SaleAmount
        {
            get
            {
                return SalePrice * Quantity;
            }
        }
        /// <summary>
        /// 可售数量
        /// </summary>
        public int SaleQuantity { get; set; }
        /// <summary>
        /// 预订数量
        /// </summary>
        public int OrderQuantity { get; set; }
        

        /// <summary>
        /// 预警数量
        /// </summary>
        public int WarnQuantity { get; set; }
        /// <summary>
        /// 是否退市（例如不再进货销售）
        /// </summary>
        public bool IsQuit { get; set; }        

        public string SupplierName { get; set; }
        
    }
}
