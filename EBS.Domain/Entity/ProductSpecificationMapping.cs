using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class ProductSpecificationMapping:BaseEntity
    {
       public int ProductSpecificationId { get; set; }
       public int ProductId { get; set; }
       public int ProductSpecificationOptionId { get; set; }
    }
}
