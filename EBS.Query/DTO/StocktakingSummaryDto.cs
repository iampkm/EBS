using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class StocktakingSummaryDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Code { get; set; }
        public string StocktakingDate { get; set; }
        public StocktakingPlanStatus Status { get; set; }

        public string StocktakingPlanStatus
        {
            get { return Status.Description(); }
        }

        public StocktakingPlanMethod Method { get; set; }

        public string StocktakingPlanMethod
        {
            get { return Method.Description(); }
        }
        /// <summary>
        /// 总库存数
        /// </summary>
        public int TotalInventoryQuantity { get; set; }
       /// <summary>
       /// 总盘点数
       /// </summary>
        public int TotalCountQuantity { get; set; }
        /// <summary>
        /// 总差异数
        /// </summary>
        public int TotalDifferentQuantity { get {
            return this.TotalInventoryQuantity - TotalCountQuantity;
        } }
    }
}
