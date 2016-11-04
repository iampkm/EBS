using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Domain.Entity;
namespace EBS.Query
{
   public interface IStorePurchaseOrderQuery
    {
       IEnumerable<StorePurchaseOrderDto> GetPageList(Pager page, SearchStorePurchaseOrder condition);
       Dictionary<int, string> GetStorePurchaseOrderStatus();
       StorePurchaseOrderItemDto GetPurchaseOrderItem(string productCodeOrBarCode, int supplierId, int storeId);

       StorePurchaseOrder GetById(int id);
    }
}
