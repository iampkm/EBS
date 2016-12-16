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
       IEnumerable<StorePurchaseOrderQueryDto> GetPageList(Pager page, SearchStorePurchaseOrder condition);
       Dictionary<int, string> GetStorePurchaseOrderStatus();
       StorePurchaseOrderItemDto GetPurchaseOrderItem(string productCodeOrBarCode,int storeId);

       StorePurchaseOrderItemDto GetRefundOrderItem(string productCodeOrBarCode, int storeId);
        IEnumerable<StorePurchaseOrderItemDto> GetPurchaseOrderItemList(string inputProducts, int storeId);
        IEnumerable<StorePurchaseOrderItemDto> GetRefundOrderItemList(string inputProducts, int storeId);
        StorePurchaseOrderDto GetById(int id);
    }
}
