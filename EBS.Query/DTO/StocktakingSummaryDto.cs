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
        public DateTime StocktakingDate { get; set; }

        public string StocktakingDateString { get {
            return StocktakingDate.ToString("yyyy-MM-dd");
        } }
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
            return TotalCountQuantity - this.TotalInventoryQuantity;
        } }


        /// <summary>
        /// 成本金额
        /// </summary>
        public decimal CostAmount { get; set; }
        /// <summary>
        /// 盘点成本金额
        /// </summary>
        public decimal CostCountAmount { get; set; }
        /// <summary>
        /// 成本金额差异
        /// </summary>
        public decimal CostAmountDifferent
        {
            get
            {
                return CostCountAmount - this.CostAmount;
            }
        }
        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal SaleAmout { get; set; }
        /// <summary>
        /// 盘点销售金额
        /// </summary>
        public decimal SaleCountAmount { get; set; }
        /// <summary>
        /// 总差异数
        /// </summary>
        public decimal SaleAmoutDifferent
        {
            get
            {
                return  SaleCountAmount-this.SaleAmout ;
            }
        }

    }
}
