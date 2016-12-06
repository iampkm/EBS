using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 货架层商品
    /// </summary>
   public class ShelfLayerProduct:BaseEntity
    {
        public int ShelfLayerId { get; set; }
        public int Number { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// 货架码
        /// </summary>
        public string Code { get; set; }
        public int StoreId { get; set; }
    }
}
