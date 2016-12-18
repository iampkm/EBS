using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class StoreInventoryBatchUpdate:BaseEntity
    {
        public StoreInventoryBatchUpdate(int id, int quantity)
        {
            this.Id = id;
            this.Quantity = quantity;
        }
        public int Quantity { get; set; }

       
    }

   
}
