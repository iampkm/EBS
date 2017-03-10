using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class AdjustStorePriceListDto
    {
       public int Id { get; set; }
        public string Code { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public string UpdatedOn { get; set; }

        public string UpdatedByName { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnString
        {
            get
            {
                return this.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public AdjustStorePriceStatus Status { get; set; }

        public string Remark { get; set; }

        public string AdjustStorePriceStatus
        {
            get
            {
                return Status.Description();
            }
        }

        public int ProductId { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        public string BarCode { get; set; }

        public string Specification { get; set; }

        public string Unit { get; set; }

        /// <summary>
        /// 总部指导价
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 门店售价
        /// </summary>
        public decimal StoreSalePrice { get; set; }
        /// <summary>
        /// 调整价
        /// </summary>
        public decimal AdjustPrice { get; set; }
    }
}
