using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
namespace EBS.Query.DTO
{
   public class StocktakingPlanItemDto:StocktakingPlanItem
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

       public int DifferentQuantity { get{
           return base.GetDifferenceQuantity();
       }}

       public decimal CostAmount
       {
           get
           {
               return base.CostPrice* Quantity;
           }
       }
       public decimal CostCountAmount
       {
           get
           {
               return base.CostPrice * CountQuantity;
           }
       }
       public decimal CostAmountDifferent
       {
           get
           {
               return CostAmount - CostCountAmount;
           }
       }

       public decimal SaleAmout
       {
           get
           {
               return base.SalePrice * Quantity;
           }
       }
       public decimal SaleCountAmount
       {
           get
           {
               return base.SalePrice * CountQuantity;
           }
       }
       public decimal SaleAmoutDifferent
       {
           get
           {
               return SaleAmout - SaleCountAmount;
           }
       }
    }
}
