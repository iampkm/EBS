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
        IEnumerable<AdjustContractPriceDto> GetPageList(Pager page, SearchSupplierContract condition);

        IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceItems(string productCodePriceInput);
        IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceItems(int AdjustContractPriceId);

        Dictionary<int, string> GetAdjustContractPriceStatus();
    }
}
