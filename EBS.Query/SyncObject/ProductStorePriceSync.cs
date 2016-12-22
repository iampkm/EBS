using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.SyncObject
{
   public class ProductStorePriceSync
    {
        public int Id { get; set; }

        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public decimal SalePrice { get; set; }
    }
}
