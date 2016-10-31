using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class PurchaseContractDto
    {
       public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }

        public string Supplier {
            get {
                return string.Format("[{0}]{1}", SupplierCode, SupplierName);
            }
        }
        public string Contact { get; set; }
      
        public string StoreName { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        public string StartTime { get {
            return StartDate.ToString("yyyy-MM-dd");
        } }

        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime EndDate { get; set; }
        public string EndTime
        {
            get
            {
                return EndDate.ToString("yyyy-MM-dd");
            }
        }
        public PurchaseContractStatus Status { get; set; }

        public string PurchaseContractStatus
        {
            get
            {
                return Status.Description();
            }
        }
    }
}
