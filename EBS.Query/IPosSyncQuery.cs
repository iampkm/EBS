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
       IEnumerable<AccountSync> QueryAccountSync(AccessTokenDto token);
       IEnumerable<StoreSync> QueryStoreSync(AccessTokenDto token);
       IEnumerable<VipCardSync> QueryVipCardSync(AccessTokenDto token);
       IEnumerable<VipProductSync> QueryVipProductSync(AccessTokenDto token);

       IEnumerable<ProductStorePriceSync> QueryProductStorePriceSync(AccessTokenDto token);

       IEnumerable<ProductAreaPriceSync> QueryProductAreaPriceSync(AccessTokenDto token);
       /// <summary>
       /// 库存商品数据
       /// </summary>
       /// <param name="page"></param>
       /// <param name="storeId">门店</param>
       /// <returns></returns>
       IEnumerable<ProductSync> QueryProductSync(AccessTokenDto token); 
        
    }

  
}
