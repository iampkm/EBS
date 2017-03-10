using EBS.Query.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query
{
   public interface IAdjustStorePriceQuery
    {
        IEnumerable<AdjustStorePriceDto> GetPageList(Pager page, SearchAdjustStorePrice condition);
        IEnumerable<AdjustStorePriceListDto> QueryFinish(Pager page, SearchAdjustStorePrice condition);
        IEnumerable<AdjustStorePriceItemDto> GetAdjustStorePriceItems(int AdjustStorePriceId);
        IEnumerable<AdjustStorePriceItemDto> GetItems(int AdjustStorePriceId);
        AdjustStorePriceItemDto GetAdjustStorePriceItem(int storeId,string productCodeOrBarCode);

        IEnumerable<AdjustStorePriceItemDto> GetAdjustStorePriceList(int storeId, string inputProducts);

        AdjustStorePriceDto GetById(int id);
        Dictionary<int, string> GetAdjustStorePriceStatus();
    }
}
