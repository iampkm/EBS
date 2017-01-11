using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.SyncObject;
using EBS.Query.DTO;
namespace EBS.Query
{
    /// <summary>
    /// Pos 数据同步查询
    /// </summary>
   public interface IPosSyncQuery
    {
       IEnumerable<AccountSync> QueryAccountSync();
       IEnumerable<StoreSync> QueryStoreSync();
       IEnumerable<VipCardSync> QueryVipCardSync();
       IEnumerable<VipProductSync> QueryVipProductSync();

       IEnumerable<ProductStorePriceSync> QueryProductStorePriceSync(int storeId);

       IEnumerable<ProductAreaPriceSync> QueryProductAreaPriceSync(int storeId);
       /// <summary>
       /// 库存商品数据
       /// </summary>
       /// <param name="page"></param>
       /// <param name="storeId">门店</param>
       /// <returns></returns>
       IEnumerable<ProductSync> QueryProductSync(int storeId,string productCodeOrBarCode); 
        
    }

  
}
