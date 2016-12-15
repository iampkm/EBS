using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class StorePurchaseOrderQueryDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }

        public string Supplier
        {
            get
            {
                return string.Format("[{0}]{1}", SupplierCode, SupplierName);
            }
        }
        public string StoreName { get; set; }
        public PurchaseOrderStatus Status { get; set; }

        public string PurchaseOrderStatus
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
                return CreatedOn.ToString("yyyy-MM-dd");
            }
        }

        public string CreatedByName { get; set; }

        public int Quantity { get; set; }

        public int ActualQuantity { get; set; }

        public decimal Amount { get; set; }


    }
}
