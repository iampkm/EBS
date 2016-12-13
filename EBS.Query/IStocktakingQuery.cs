using EBS.Query.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
namespace EBS.Query
{
   public interface IStocktakingQuery
    {
       IEnumerable<StocktakingListDto> GetPageList(Pager page, SearchStocktaking condition);

       StocktakingPlan GetRunningPlan(int storeId);

       
       
    }
}
