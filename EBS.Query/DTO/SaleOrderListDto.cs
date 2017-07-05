using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class SaleOrderListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string StoreName { get; set; }
        public int PosId { get; set; }
        public OrderType OrderType { get; set; }

        public string OrderTypeName
        {
            get
            {
                return OrderType.Description();
            }
        }

        public SaleOrderStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                return Status.Description();
            }
        }

        public string OrderLevelName
        {
            get
            {
                return OrderLevel.Description();
            }
        }

        public SaleOrderLevel OrderLevel { get; set; }

        public decimal OrderAmount { get; set; }

        public decimal PayAmount { get; set; }
        public decimal OnlinePayAmount { get; set; }

        public PaymentWay PaymentWay { get; set; }

        public string PaymentWayName
        {
            get
            {
                return PaymentWay.Description();
            }
        }

        public string UpdatedOn { get; set; }

        public string PaidDate { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string NickName { get; set; }

        //明细项
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

        public decimal RealPrice { get; set; }

        public decimal AvgCostPrice { get; set; }
        
        /// <summary>
        /// 销售金额 RealPrice * Quantity
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 均价成本金额：AvgCostPrice * Quantity
        /// </summary>
        public decimal CostAmount { get; set; }
    }
}
