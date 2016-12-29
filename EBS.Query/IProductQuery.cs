using EBS.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IProductQuery
    {
       IEnumerable<ProductDto> GetPageList(Pager page, string name, string codeOrBarCode, string categoryId, int brandId);

        IEnumerable<PriceTagDto> QueryProductPriceTagList(string ids);

        PriceTagDto QueryPriceTag(string productCodeOrBarCode);

        string GenerateBarCode();
    }
}
