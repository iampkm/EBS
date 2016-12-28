using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class TransferOrderDto
    {
        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public string Code { get; set; }

        public string StatusName { get; set; }

        public int EditBy { get; set; }

        public string EditByName { get; set; }

        public List<TransaferOrderItemDto> Items { get; set; }
    }
}
