using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query
{
   public interface IVipProductQuery
    {
       IEnumerable<DTO.VipProductDto> GetPageList(DTO.Pager page, DTO.SearchVipProduct condition);
    }
}
