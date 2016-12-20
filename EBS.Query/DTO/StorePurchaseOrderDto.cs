using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class StorePurchaseOrderDto
    {
        public StorePurchaseOrderDto() {
            this.Items = new List<StorePurchaseOrderItemDto>();         
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }

        public int SupplierId { get; set; }

        public string SupplierBill { get; set; }

        public string SupplierName { get; set; }

        public string Supplier
        {
            get
            {
                return string.Format("[{0}]{1}", SupplierCode, SupplierName);
            }
        }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public PurchaseOrderStatus Status { get; set; }

        public string PurchaseOrderStatus
        {
            get
            {
                return Status.Description();
            }
        }
        public string CreatedOn { get; set; }

        public string CreatedByName { get; set; }
        public string ReceivedOn { get; set; }

        public string ReceivedByName { get; set; }
        public string StoragedOn { get; set; }
        public string StoragedByName { get; set; }

        public bool IsGift { get; set; }

        public List<StorePurchaseOrderItemDto> Items { get; set; }

        public OrderType OrderType { get; set; }
    }
}
