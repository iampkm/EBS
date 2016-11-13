using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Domain.Entity;
namespace EBS.Query
{
   public interface IAdjustSalePriceQuery
    {
        IEnumerable<AdjustSalePriceDto> GetPageList(Pager page, SearchAdjustSalePrice condition);

        IEnumerable<AdjustSalePriceItemDto> GetAdjustSalePriceItems(int AdjustSalePriceId);
        IEnumerable<AdjustSalePriceItemDto> GetItems(int AdjustSalePriceId);
        AdjustSalePriceItemDto GetAdjustSalePriceItem(string productCodeOrBarCode);

        IEnumerable<AdjustSalePriceItemDto> GetAdjustSalePriceList(string inputProducts);

        Dictionary<int, string> GetAdjustSalePriceStatus();
    }
}
