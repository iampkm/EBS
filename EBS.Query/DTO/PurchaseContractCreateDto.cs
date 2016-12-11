using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class PurchaseContractCreateDto
    {
        public PurchaseContractCreateDto()
        {
            Items = new List<PurchaseContractItemDto>();
        }

        public int SupplierId { get; set; }

        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }

        public List<PurchaseContractItemDto> Items { get; set; }
    }
}
