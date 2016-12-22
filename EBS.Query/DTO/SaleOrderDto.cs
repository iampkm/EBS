using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
namespace EBS.Query.DTO
{
   public class SaleOrderDto
    {
         public string Code { get; set; }

         public string StoreName { get; set; }
        public int PosId { get; set; }
        public OrderType OrderType { get; set; }

        public SaleOrderStatus Status { get; set; }

        public decimal OrderAmount { get; set; }

        public decimal PayAmount { get; set; }
        public decimal OnlinePayAmount { get; set; }

        public PaymentWay PaymentWay { get; set; }

        public DateTime PaidDate { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public int UpdateByName { get; set; }
    }
}
