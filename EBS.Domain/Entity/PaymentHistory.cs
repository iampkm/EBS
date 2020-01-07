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
        /// 订单类型   saleorder(销售订单),refundorder(销售退单)
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 支付类型：wechat；alipay，unionpay(银联)
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        ///  交易金额：单位分
        /// </summary>
        public string Amount { get; set; }
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
        public string Content { get; set; }
        public string CreatedOn { get; set; }

        /// <summary>
        /// 发起支付请求
        /// </summary>
        /// <param name="orderCode">订单号</param>
        /// <param name="amount">金额，单位分</param>
        /// <param name="orderType">订单类型：saleorder(销售单),refundorder（销售退单）</param>
        /// <param name="paymentType">支付类型：wechat,alipay,unionpay</param>
        /// <param name="content">请求报文</param>
        public void LogWechatBarcodeRequestPay(string orderCode, string amount,string orderType,string content )
        {
            InitPaymentHistory(orderCode, amount, orderType, "wechat", "request.barcode.pay", content);          
        }
        public void LogWechatBarcodeResponsePay(string orderCode, string amount, string orderType,string content, bool success)
        {
            var action = success? "response.barcode.pay.success" : "response.barcode.pay.failed";
            InitPaymentHistory(orderCode, amount, orderType, "wechat", action, content);
        }
        public void LogAlipayBarcodeRequestPay(string orderCode, string amount, string orderType, string content)
        {
            InitPaymentHistory(orderCode, amount, orderType, "alipay", "request.barcode.pay", content);
        }
        public void LogAlipayBarcodeResponsePay(string orderCode, string amount,string orderType, string content, bool success)
        {
            var action = success ? "response.barcode.pay.success" : "response.barcode.pay.failed";
            InitPaymentHistory(orderCode, amount, orderType, "alipay", action, content);
        }

        private void InitPaymentHistory(string orderCode,string amount,string orderType,string paymentType,string tradeAction,string content)
        {
            this.OrderCode = orderCode;
            this.Amount = amount;         
            this.OrderType = orderType.ToLower();
            this.PaymentType = paymentType.ToLower();
            this.TradeAction = tradeAction;
            this.Content = content;
            this.RequestUrl = "";
            this.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
       

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
