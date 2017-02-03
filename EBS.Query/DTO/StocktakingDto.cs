using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class StocktakingDto:Stocktaking
    {
       public DateTime StocktakingDate { get; set; }

       public string StocktakingDateString
       {
           get
           {
               return StocktakingDate.ToString("yyyy-MM-dd");
           }
       }

       public string CreatedOnString { get {
           return CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
       } }

       public string StatusString
       {
           get
           {
               return Status.Description();
           }
       }

       public string StocktakingTypeString
       {
           get
           {
               return StocktakingType.Description();
           }
       }

       public string StoreName
       {
           get;
           set;
       }
    }
}
