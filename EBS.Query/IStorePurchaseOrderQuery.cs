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
       StorePurchaseOrderItemDto GetPurchaseOrderItem(string productCodeOrBarCode,int storeId, int supplierId);

       IEnumerable<StorePurchaseOrderItemDto> GetPurchaseOrderItemList(string inputProducts, int storeId, int supplierId);

       StorePurchaseOrderItemDto GetRefundOrderItem(string productCodeOrBarCode, int storeId, int supplierId);

       IEnumerable<StorePurchaseOrderItemDto> GetRefundOrderItemList(string inputProducts, int storeId, int supplierId);
        /// <summary>
        /// 查询商品的所有批次记录
        /// </summary>
        /// <param name="productCodeOrBarCode"></param>
        /// <returns></returns>
        IEnumerable<StorePurchaseOrderItemDto> GetProductBatchs(string productCodeOrBarCode,int storeId);
        StorePurchaseOrderDto GetById(int id);

        IEnumerable<StorePurchaseOrderListDto> GetFinishList(Pager page, SearchStorePurchaseOrder condition);

        IEnumerable<StorePurchaseOrderSummaryDto> GetSummaryList(Pager page, SearchStorePurchaseOrder condition);


    }
}
