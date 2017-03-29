using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchSaleAnalysis
    {
        public string ProductCodeOrBarCode { get; set; }
        /// <summary>
        /// 门店Id，多个逗号分隔
        /// </summary>
        public string StoreId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 品类ID
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string ProductName { get; set; }
    }
}
