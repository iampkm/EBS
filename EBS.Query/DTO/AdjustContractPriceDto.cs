using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
    public class AdjustContractPriceDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
       // public string Name { get; set; }
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

        public string NickName { get; set; }

        public string CreatedOn { get; set; }

        public string Remark { get; set; }

        public AdjustContractPriceStatus Status { get; set; }

        public string StatusName { get {
                return Status.Description();
            } }

    }
}
