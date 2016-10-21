using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 商品规格选项
    /// </summary>
   public class ProductSpecificationOption:BaseEntity
    {
       public int ProductSpecificationId { get; set; }

       public string Value { get; set; }
    }
}
