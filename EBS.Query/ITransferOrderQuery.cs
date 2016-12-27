using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface ITransferOrderQuery
    {
       IEnumerable<TransferOrderDto> GetPageList(Pager page, SearchTransferOrder condition);
    }
}
