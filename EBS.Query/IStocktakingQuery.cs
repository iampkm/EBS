using EBS.Query.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query
{
   public interface IStocktakingQuery
    {
       IEnumerable<StocktakingPlanDto> GetPageList(Pager page, SearchStocktakingPlan condition);

       IEnumerable<StocktakingPlanDto> GetSummaryData(Pager page, SearchStocktakingPlan condition);
       Dictionary<int, string> GetStocktakingPlanStatus();

       
    }
}
