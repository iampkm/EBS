using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PurchaseContractItemDto
    {
         public int Id { get; set; }

         public string Name { get; set; }

         public string Code { get; set; }

         public string CategoryName { get; set; }

        public string Specification { get; set; }

         public decimal Price { get; set; }
    }
}
