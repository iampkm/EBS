using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class SaleCheckDto
    {
        /// <summary>
        /// 班次ID
        /// </summary>
        public int Id { get; set; }
        public string CreatedByName { get; set; }
        public string StoreName { get; set; }
        public int PosId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal OrderCount { get; set; }
        
        public OrderType OrderType { get; set; }

        public string OrderTypeName { get {
                return OrderType.Description();
            } }

        public SaleOrderStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                return Status.Description();
            }
        }
    }
}
