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
       IEnumerable<AccountSync> QueryAccountSync(Pager page);
       IEnumerable<StoreSync> QueryStoreSync(Pager page);
       IEnumerable<VipCardSync> QueryVipCardSync(Pager page);
       IEnumerable<VipProductSync> QueryVipProductSync(Pager page);

       IEnumerable<ProductStorePriceSync> QueryProductStorePriceSync(Pager page);

       IEnumerable<ProductAreaPriceSync> QueryProductAreaPriceSync(Pager page);
       /// <summary>
       /// 库存商品数据
       /// </summary>
       /// <param name="page"></param>
       /// <param name="storeId">门店</param>
       /// <returns></returns>
       IEnumerable<ProductSync> QueryProductSync(Pager page,int storeId); 
        
    }

  
}
