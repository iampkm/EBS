using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext.Schema;
namespace EBS.Domain.Entity
{
    [Table("payment_history")]
    public class PaymentHistory : BaseEntity
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 订单类型   Order.SaleOrder   Order.RefundOrder
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 支付类型：    WechatPay；Alipay，UnionPay
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        ///  交易金额：单位分
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 退款单号
        /// </summary>
        public string RefundCode { get; set; }
        /// <summary>
        /// 支付企业交易号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        ///  交易动作  request.pay;   response.pay.notify;   
        /// </summary>
        public string TradeAction { get; set; }
        public string RequestUrl { get; set; }
        /// <summary>
        /// 原始报文数据
        /// </summary>
        public string RawData { get; set; }
        public string CreatedOn { get; set; }


        public string RequestPay() {
            return "request.pay";
        }

        public string ResponsePayNotify()
        {
            return "response.pay.notify";
        }

        public string WechatPay(){
            return "Wechatpay";
        }
        public string AliPay()
        {
            return "Alipay";
        }
        public string UnionPay()
        {
            return "Unionpay";
        }
    }
}
