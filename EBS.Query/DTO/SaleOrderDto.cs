using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class SaleOrderDto
    {
         public string Code { get; set; }

         public string StoreName { get; set; }
        public int PosId { get; set; }
        public OrderType OrderType { get; set; }

        public string OrderTypeName { get {
            return OrderType.Description();
        } }

        public SaleOrderStatus Status { get; set; }

        public string StatusName {
            get {
                return Status.Description();
            }
        }

        public decimal OrderAmount { get; set; }

        public decimal PayAmount { get; set; }
        public decimal OnlinePayAmount { get; set; }

        public PaymentWay PaymentWay { get; set; }

        public string PaymentWayName {
            get {
                return PaymentWay.Description();
            }
        }

        public string UpdatedOn { get; set; }

        public string PaidDate { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string NickName { get; set; }
    }
}
