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
       IEnumerable<PurchaseContractDto> GetPageList(Pager page, string code, string name, int supplierId);

       IEnumerable<PurchaseContractItemDto> GetPurchaseContractItems(string productCodePriceInput);

       IDictionary<int, string> GetCooperateWay();
    }
}
