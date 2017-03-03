using EBS.Query.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query
{
   public interface IAccessTokenQuery
    {
       IEnumerable<AccessTokenDto> GetPageList(Pager page, int storeId);
    }
}
