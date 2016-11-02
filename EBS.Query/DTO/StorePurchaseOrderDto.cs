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
        public DateTime UpdateOn { get; set; }

        public string UpdateTime
        {
            get
            {
                return UpdateOn.ToString("yyyy-MM-dd");
            }
        }

        public string UpdatedByName { get; set; }
    }
}
