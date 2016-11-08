using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    public class SearchStoreInventoryHistory
    {
       public string BillCode { get; set; }

        public int BillType { get; set; }

        public int StoreId { get; set; }

        public string ProductCodeOrBarCode { get; set; }

        public string BatchNo { get; set; }
    }
}
