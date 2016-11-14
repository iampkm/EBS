using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
namespace EBS.Domain.Orders
{
   public class SaleOrder:BaseEntity
    {
        List<SaleOrderItem> _items;
        public SaleOrder()
        {
            this._items = new List<SaleOrderItem>();
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Status = SaleOrderStatus.Create;
        }
        /// <summary>
        /// 订单编号 ： 单据类型 2 + 账号4位+ （year-2016） 3位+ 下单的秒数（86400，5位） ：12000100086400
        /// </summary>
        public string Code { get; set; }

        public int StoreId { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PaidStatus PaidStatus { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentWay PaymentWay { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PaidDate { get; set; }
        /// <summary>
        /// 下单时段
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public SaleOrderStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public virtual IEnumerable<SaleOrderItem> Items
        {
            get
            {
                return this._items;
            }
        }

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
