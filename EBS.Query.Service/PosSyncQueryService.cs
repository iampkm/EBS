using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Query.SyncObject;
namespace EBS.Query.Service
{
   public class PosSyncQueryService:IPosSyncQuery
    {


        public IEnumerable<AccountSync> QueryAccountSync(Pager page)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountSync> QueryAccountSync(int[] Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StoreSync> QueryStoreSync(Pager page)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StoreSync> QueryStoreSync(int[] Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VipCardSync> QueryVipCardSync(Pager page)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VipCardSync> QueryVipCardSync(int[] Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VipProductSync> QueryVipProductSync(Pager page)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VipProductSync> QueryVipProductSync(int[] Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductSync> QueryProductSync(Pager page, int storeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductSync> QueryProductSync(int[] Ids, int storeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChangeDataSync> QueryChangeData(DateTime lastQueryTime)
        {
            throw new NotImplementedException();
        }
    }
}
