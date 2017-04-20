using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchAdjustStorePrice
    {
        public string Code { get; set; }
        public string ProductCodeOrBarCode { get; set; }

        public int Status { get; set; }
        /// <summary>
        /// 查询门店，多个门店逗号分隔
        /// </summary>
        public string StoreId { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
