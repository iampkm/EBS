using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class ProductCodeSequence:BaseEntity
    {
       public ProductCodeSequence()
       {
           this.GuidCode = Guid.NewGuid().ToString().Replace("-", "");
       }
       public string GuidCode { get; set; }

    }
}
