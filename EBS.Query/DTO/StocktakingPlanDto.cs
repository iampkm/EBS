﻿using System;
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
       public string StocktakingDate { get; set; }

       public StocktakingPlanStatus Status { get; set; }

       public string StocktakingPlanStatus {
           get { return Status.Description(); }
       }

       public StocktakingPlanMethod Method { get; set; }

       public string StocktakingPlanMethod
       {
           get { return Method.Description(); }
       }
    }

   public class SearchStocktakingPlan
   {
       public string Code { get; set; }

       public int Status { get; set; }

       public DateTime? StocktakingDate { get; set; }

       public int StoreId { get; set; }
   }
}
