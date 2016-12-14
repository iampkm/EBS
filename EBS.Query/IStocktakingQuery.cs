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

       IEnumerable<StocktakingDto> GetAuditList(Pager page, SearchStocktaking condition);

       IEnumerable<StocktakingItemDto> QueryShelfProduct(int storeId, string shelfCode);

       StocktakingItemDto QueryShelfProduct(int planId, int storeId, string productCodeOrBarCode);

       StocktakingItemDto QueryStocktaingItem(int planId, string productCodeOrBarCode);
       
    }
}
