using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
namespace EBS.Domain.Entity
{
   public class SaleOrder:BaseEntity
    {

        public SaleOrder()
        {
            this.Items = new List<SaleOrderItem>();
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Status = SaleOrderStatus.Create;
            this.OrderLevel = SaleOrderLevel.General;
        }
        /// <summary>
        /// 订单编号 ： 单据类型 2 + 账号4位+ （日期20160101） 3位+ 下单的秒数（86400，5位）+ 2位随机
        /// </summary>
        public string Code { get; set; }
        public int StoreId { get; set; }
        /// <summary>
        /// Pos 机ID
        /// </summary>
        public int PosId { get; set; }
        /// <summary>
        /// 订单类型：销售单1，销售退单2
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentWay PaymentWay { get; set; }
        /// <summary>
        /// 退款账户
        /// </summary>
        public string RefundAccount { get; set; }
        /// <summary>
        /// 支付日期
        /// </summary>
        public DateTime? PaidDate { get; set; }

        public int Hour { get; set; }
        
        /// <summary>
        /// 订单金额 = 实际价格RealAmount * 数量
        /// </summary> 
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 现金支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 刷卡支付，微信支付，阿里支付等在线支付金额
        /// </summary>
        public decimal OnlinePayAmount { get; set; }
        /// <summary>
        /// 销售单状态
        /// </summary>
        public SaleOrderStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        /// <summary>
        /// 班次代码
        /// </summary>
        public string WorkScheduleCode { get; set; }

        public SaleOrderLevel OrderLevel { get; set; }

        /// <summary>
        /// 原销售单单号，只有ordertype=2 时有值
        /// </summary>
        public string SourceSaleOrderCode { get; set; }

        public virtual List<SaleOrderItem> Items   { get; set; }


        public string GenderateNewCode()
        {
           
            string createdBy = this.CreatedBy > 9999 ?this.CreatedBy.ToString().Substring(0,4): this.CreatedBy.ToString().PadLeft(4, '0'); // 4位
            int orderYear = this.CreatedOn.Year - 2016;  // 3位
            var date = this.CreatedOn;
            var ts = date - Convert.ToDateTime(date.ToShortDateString());
            var seconds = Math.Truncate(ts.TotalSeconds).ToString().PadLeft(6, '0');  // 5位
            return string.Format("{0}{1}{2}{3}", (int)BillIdentity.SaleOrder, createdBy, orderYear.ToString().PadLeft(3,'0'), seconds);
        }



    }
}
