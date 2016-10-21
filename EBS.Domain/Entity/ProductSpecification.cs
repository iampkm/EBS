using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 商品规格： 会影响商品价格或者库存
    /// </summary>
   public class ProductSpecification:BaseEntity
    {
       /// <summary>
       /// 规格名称
       /// </summary>
       public string Name { get; set; }

       /// <summary>
       /// 所属分类
       /// </summary>
       public string CategoryId { get; set; }
    }
}
