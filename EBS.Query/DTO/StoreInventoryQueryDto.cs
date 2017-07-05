using System;
using System.Collections.Generic;
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
        public string StoreName { get; set; }
        /// <summary>
        /// 可售数量
        /// </summary>
        public int SaleQuantity { get; set; }
        /// <summary>
        /// 预订数量
        /// </summary>
        public int OrderQuantity { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Quantity { get; set; }

        public decimal AvgCostPrice { get; set; }

        public decimal Amount {
            get {
                return AvgCostPrice * Quantity;
            }
        }

        public decimal SalePrice { get; set; }
        public decimal SaleAmount
        {
            get
            {
                return SalePrice * Quantity;
            }
        }

        /// <summary>
        /// 预警数量
        /// </summary>
        public int WarnQuantity { get; set; }
        /// <summary>
        /// 是否退市（例如不再进货销售）
        /// </summary>
        public bool IsQuit { get; set; }

        public string BarCode { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        public string SupplierName { get; set; }

        public string CategoryName { get; set; }
    }
}
