using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Domain.Entity;
namespace EBS.Query
{
   public interface IStoreQuery
    {
       IEnumerable<StoreDto> GetPageList(Pager page, string name, string code,string canViewStores);
        IEnumerable<StoreTreeNode> LoadStore();
    }
}
