using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Contact { get; set; }
        public string StoreName { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        public string StartTime
        {
            get
            {
                return StartDate.ToString("yyyy-MM-dd");
            }
        }
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

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }
        public string BarCode { get; set; }
        public string Specification { get; set; }

        public string Unit { get; set; }

        public decimal OldContractPrice { get; set; }
        public decimal ContractPrice { get; set; }

    }
}
