using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
namespace EBS.Query.DTO
{
   public class ProductAttrbuteValue
    {
         public ProductSpecification Ppecification { get; set; }

         public List<ProductSpecificationOption> Options { get; set; }
    }
}
