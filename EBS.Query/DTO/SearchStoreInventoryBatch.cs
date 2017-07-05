using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchStoreInventoryBatch
    {
        public int SupplierId { get; set; }

        public string StoreId { get; set; }

        public string ProductCodeOrBarCode { get; set; }

        public string BatchNo { get; set; }

        public string CategoryId { get; set; }
        /// <summary>
        /// 库存数
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string Operate { get; set; }
    }
}
