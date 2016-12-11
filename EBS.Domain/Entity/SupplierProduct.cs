using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    public class SupplierProduct : BaseEntity
    {
        public SupplierProduct()
        {
            this.Status = SupplierProductStatus.WaitCompare;
            this.CompareStatus = ComparePriceStatus.WaitCompare;
            this.UpdatedOn = DateTime.Now;
        }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public SupplierProductStatus Status { get; set; }

        public ComparePriceStatus CompareStatus { get; set; }

        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        
    }
}
