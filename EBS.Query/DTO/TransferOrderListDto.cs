using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class TransferOrderListDto
    {
        public int Id { get; set; }
        public string FromStoreName { get; set; }
        public string ToStoreName { get; set; }
        public string Code { get; set; }

        public TransferOrderStatus Status { get; set; }
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
                return CreatedOn.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedTime
        {
            get
            {
                return UpdatedOn.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string CreatedByName { get; set; }

        // 明细数据
        public string ProductCode { get; set; }

        public string ProductCodeAndBarCode
        {
            get
            {
                var result = string.Format("{0} | {1}", this.ProductCode, this.BarCode);
                return result;
            }
        }
        public string BarCode { get; set; }

        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}
