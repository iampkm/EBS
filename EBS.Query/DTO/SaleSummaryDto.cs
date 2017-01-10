using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
    public class SaleSummaryDto
    {
        public int Id { get; set; }
        public string CreatedByName { get; set; }
        public string StoreName { get; set; }
        public int PosId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        /// <summary>
        /// 店员录入收现金额
        /// </summary>
        public decimal CashAmount { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalOnlineAmount { get; set; }
        public PaymentWay PaymentWay { get; set; }

        public string PaymentWayName
        {
            get
            {
                return PaymentWay.Description();
            }
        }
    }
}
