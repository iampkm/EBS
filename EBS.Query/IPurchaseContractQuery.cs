using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Domain.Entity;
namespace EBS.Query
{
   public interface IPurchaseContractQuery
    {
       IEnumerable<PurchaseContractDto> GetPageList(Pager page, SearchSupplierContract condition);

       IEnumerable<PurchaseContractItemDto> GetPurchaseContractItems(string productCodePriceInput);
       IEnumerable<PurchaseContractItemDto> GetPurchaseContractItems(int purchaseContractId);

       Dictionary<int, string> GetPurchaseContractStatus();

        PurchaseContractCreateDto QueryContractInfo(int supplierId);
    }
}
