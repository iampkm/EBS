using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class StocktakingPlanModel
    {
       public int Id {get;set;}

       public int Method {get;set;}

       public int EditedBy {get;set;}
       public string Editor {get;set;}
       public DateTime StocktakingDate { get; set; }

    }
}
