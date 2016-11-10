using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Domain.Entity;
namespace EBS.Query
{
   public interface IAdjustContractPriceQuery
    {
       IEnumerable<AdjustContractPriceDto> GetPageList(Pager page, SearchAdjustContractPrice condition);
 
        IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceItems(int AdjustContractPriceId);
        IEnumerable<AdjustContractPriceItemDto> GetItems(int AdjustContractPriceId, int supplierId, int storeId);
        AdjustContractPriceItemDto GetAdjustContractPriceItem(string productCodeOrBarCode, int supplierId, int storeId);

        IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceList(string inputProducts, int supplierId, int storeId);

    }
}
