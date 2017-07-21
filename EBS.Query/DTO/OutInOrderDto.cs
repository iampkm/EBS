using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class OutInOrderDto
    {
       public OutInOrderDto() {
           this.Items = new List<OutInOrderItemDto>();
       }
       public int Id { get; set; }
        public string Code { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public string SupplierId { get; set; }
        public string SupplierName { get; set; }

        public OutInOrderStatus Status { get; set; }
        public string OrderStatus
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

        /// <summary>
        /// 业务类别名称
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        public string UpdatedOnTime
        {
            get
            {
                return UpdatedOn.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        /// <summary>
        /// 财务复核
        /// </summary>
        public string AuditName { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public string Remark { get; set; }

        public int OutInOrderTypeId { get; set; }


        public List<OutInOrderItemDto> Items { get; set; }
    }
}
