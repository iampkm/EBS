using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class OutInOrderDto
    {
       public int Id { get; set; }
        public string Code { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public string SupplierId { get; set; }
        public string SupplierName { get; set; }

        public OutInOrderStatus Status { get; set; }
        public string StatusName
        {
            get
            {
                return Status.Description();
            }
        }

        public DateTime CreatedOn { get; set; }

        public string CreatedTime
        {
            get
            {
                return CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }

        public List<OutInOrderItemDto> Items { get; set; }
    }
}
