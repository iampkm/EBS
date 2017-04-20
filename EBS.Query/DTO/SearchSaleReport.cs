using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchSaleReport
    {
        public string ProductCodeOrBarCode { get; set; }
        /// <summary>
        /// 门店Id，多个逗号分隔
        /// </summary>
        public string StoreId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CategoryId { get; set; }

        public int BrandId { get; set; }

       
        /// <summary>
        /// 分组条件
        /// </summary>
        public GroupByMethod GroupBy { get; set; }
        /// <summary>
        /// 按品类分组时，分组级别
        /// </summary>
        public int CategoryLevel { get; set; }

        public int OrderLevel { get; set; }

        public int OrderType { get; set; }

        public int PaymentWay { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public int Creator { get; set; }
    }

   public enum GroupByMethod {
       Store = 1, Product=2,Category =3, Supplier=4, Creator=5, Day=6
   }
}
