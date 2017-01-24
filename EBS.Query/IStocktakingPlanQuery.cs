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

        IEnumerable<StocktakingSummaryDto> GetSummaryData(Pager page, SearchStocktakingPlanSummary condition);

        IEnumerable<StocktakingPlanItemDto> GetDetails(Pager page, int planId, int? from, int? to, bool showDifference,string productCodeOrBarCode);

        Dictionary<int, string> GetStocktakingPlanStatus();
    }
}
