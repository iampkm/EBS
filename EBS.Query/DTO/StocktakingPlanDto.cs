using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class StocktakingPlanDto
    {
       public int Id { get; set; }
       public string StoreName { get; set; }
       public string Code { get; set; }
       public DateTime StocktakingDate { get; set; }

       public string StocktakingDateString { get {
           return StocktakingDate.ToString("yyyy-MM-dd");
       } }

       public StocktakingPlanStatus Status { get; set; }

       public string StocktakingPlanStatus {
           get { return Status.Description(); }
       }

       public StocktakingPlanMethod Method { get; set; }

       public string StocktakingPlanMethod
       {
           get { return Method.Description(); }
       }

       public string CreatedByName { get; set; }
    }

   public class SearchStocktakingPlan
   {
       public string Code { get; set; }

       public int Status { get; set; }

       public DateTime? StocktakingDate { get; set; }

       public int StoreId { get; set; }
       public DateTime? StartDate { get; set; }

       public DateTime? EndDate { get; set; }
   }

    public class SearchStocktakingPlanSummary
    {
        public string Code { get; set; }
        /// <summary>
        /// 逗号分隔状态值
        /// </summary>
        public string Status { get; set; }

        public DateTime? StocktakingDate { get; set; }

        public string StoreId { get; set; }
        /// <summary>
        /// 盘点方式：大盘，小盘
        /// </summary>
        public int Method { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
