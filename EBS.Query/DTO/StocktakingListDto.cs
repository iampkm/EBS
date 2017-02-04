using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class StocktakingListDto
    {
       public int Id { get; set; }
        /// <summary>
        /// 盘点单据号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 盘点类型
        /// </summary>
        public StocktakingType StocktakingType { get; set; }

        public string Type { get {
            return StocktakingType.Description();
        } }

        public DateTime StocktakingDate { get; set; }

        public string StocktakingDateString { get {
            return StocktakingDate.ToString("yyyy-MM-dd");
        } }

        /// <summary>
        /// 货架码
        /// </summary>
        public string ShelfCode { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string StoreName { get; set; }

        public string ProductCode { get; set; }

        public string BarCode { get; set; }
        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string CountQuantity { get; set; }
        /// <summary>
        /// 差错原因
        /// </summary>
        public string CorectReason { get; set; }
       /// <summary>
       /// 备注
       /// </summary>
        public string Note { get; set; }
    }
}
