using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
  public interface IStocktakingPlanQuery
    {
        IEnumerable<StocktakingPlanDto> GetPageList(Pager page, SearchStocktakingPlan condition);

        IEnumerable<StocktakingSummaryDto> GetSummaryData(Pager page, SearchStocktakingPlan condition);

        IEnumerable<StocktakingPlanItemDto> GetDetails(int planId, int? from, int? to, bool showDifference);

        Dictionary<int, string> GetStocktakingPlanStatus();
    }
}
