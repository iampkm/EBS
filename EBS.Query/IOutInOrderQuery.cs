using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IOutInOrderQuery
    {
        IEnumerable<OutInOrderDto> GetPageList(Pager page, SearchOutInOrder condition);

        OutInOrderItemDto QueryProduct(string productCodeOrBarCode, int storeId);

        IEnumerable<OutInOrderItemDto> ImportProducts(int storeId, string inputBarCodes);
        List<OutInOrderItemDto> QueryProductBatch(string productCodeOrBarCode, int storeId);

        OutInOrderDto GetById(int id);

        IEnumerable<OutInOrderListDto> GetFinishList(Pager page, SearchOutInOrder condition);

        IEnumerable<OutInOrderSummaryDto> GetSummaryList(Pager page, SearchOutInOrder condition);
    }
}
