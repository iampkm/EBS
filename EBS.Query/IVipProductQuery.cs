using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IVipProductQuery
    {
       IEnumerable<DTO.VipProductDto> GetPageList(DTO.Pager page, DTO.SearchVipProduct condition);

       IEnumerable<VipProductDto> QueryProductByBarCode(string inputProducts);

       VipProductDto QueryProduct(string productCodeOrBarCode);
    }
}
