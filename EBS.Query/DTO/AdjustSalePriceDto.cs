using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class AdjustSalePriceDto
    {
        public int Id { get; set; }
        public string Code { get; set; } 

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }
        public string BarCode { get; set; }
        public string Specification { get; set; }

        public string Unit { get; set; }

        public decimal SalePrice { get; set; }
        public decimal AdjustPrice { get; set; }

        public string UpdatedOn { get; set; }

        public string UpdatedByName { get; set; }

        public AdjustSalePriceStatus Status { get; set; }

        public string AdjustSalePriceStatus
        {
            get
            {
                return Status.Description();
            }
        }
    }
}
