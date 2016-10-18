using EBS.Query.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EBS.Query
{
   public interface IAccountQuery
    {
       IEnumerable<AccountInfo> GetPageList(Pager page, int? id, string userName, string nickNam);
    }
}
