using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.SyncObject
{
    public class VipProductSync
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public decimal SalePrice { get; set; }
    }
}
