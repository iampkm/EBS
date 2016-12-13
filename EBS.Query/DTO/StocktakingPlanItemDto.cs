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
               return base.SalesPrice * Quantity;
           }
       }
       public decimal SaleCountAmount
       {
           get
           {
               return base.SalesPrice * CountQuantity;
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
