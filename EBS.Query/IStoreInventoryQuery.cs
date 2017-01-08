using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IStoreInventoryQuery
    {
        IEnumerable<StoreInventoryQueryDto> GetPageList(Pager page, SearchStoreInventory condition);
        IEnumerable<StoreInventoryHistoryQueryDto> GetPageList(Pager page, SearchStoreInventoryHistory condition);
        IEnumerable<StoreInventoryBatchQueryDto> GetPageList(Pager page, SearchStoreInventoryBatch condition);

        IEnumerable<ProductQueryDto> QueryProduct(string productCodeOrBarCode);
    }
}
