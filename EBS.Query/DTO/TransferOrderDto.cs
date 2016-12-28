using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class TransferOrderDto
    {
        public int Id { get; set; }
        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public string Code { get; set; }

        public TransferOrderStatus Status { get; set; }
        public string StatusName { get {
                return Status.Description();
          } }

        public DateTime CreatedOn { get; set; }

        public string CreatedTime {
            get {
                return CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalAmount { get; set; }

        public List<TransaferOrderItemDto> Items { get; set; }
    }
}
