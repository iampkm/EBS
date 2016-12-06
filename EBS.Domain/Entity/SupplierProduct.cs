using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class SupplierProduct:BaseEntity
    {
       public SupplierProduct(int id,int supplierId, int productId, decimal price)
       {
           this.Id = id;
           this.SupplierId = supplierId;
           this.ProductId = productId;
           this.Price = price;
       }
       public int SupplierId { get; set; }
       public int ProductId { get; set; }
       public decimal Price { get; set; }
    }
}
