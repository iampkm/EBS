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

        public string BillType { get; set; }

        public string StoreId { get; set; }

        public string ProductCodeOrBarCode { get; set; }

        public string BatchNo { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
