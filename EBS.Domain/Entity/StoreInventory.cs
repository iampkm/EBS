using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    public class StoreInventory : BaseEntity
    {
        /// <summary>
        /// 商品SKUID
        /// </summary>
        public int ProductSkuId { get; set; }
        public int StoreId { get; set; }
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
        /// <summary>
        /// 预警数量
        /// </summary>
        public int WarnQuantity { get; set; }
        /// <summary>
        /// 是否退市（例如不再进货销售）
        /// </summary>
        public bool IsQuit { get; set; }
    }
}
