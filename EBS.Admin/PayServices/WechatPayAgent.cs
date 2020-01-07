using PaySharp.Core;
using PaySharp.Core.Response;
using PaySharp.Wechatpay;
using PaySharp.Wechatpay.Domain;
using PaySharp.Wechatpay.Request;
using PaySharp.Wechatpay.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Infrastructure;
using Newtonsoft.Json;

namespace EBS.Admin.PayServices
{
    public class WechatPayAgent : IPayAgent
    {

        IGateways _gateways;
        IGateway _gateway;
        IDBContext _db;
        public WechatPayAgent(IGateways geteways,IDBContext db)
        {
            this._gateways = geteways;
            this._db = db;
        }
        public WechatPayAgent()
        {
            this._gateways = AppContext.Current.Resolve<IGateways>();
            this._db = AppContext.Current.Resolve<IDBContext>();
        }
        #region 条码支付接口

        [PayRoute("wechat.trade.barcode.pay")]
        public BarcodePayResponse BarcodePay(PayRequest payRequest)
        {            
            _gateway = _gateways.GetByStoreId<WechatpayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent, new { body = "", total_amount = 0, out_trade_no = "", auth_code = "" });

            // 记录支付日志
            var content = JsonConvert.SerializeObject(payRequest);
            var payHistory = new PaymentHistory();
            payHistory.LogWechatBarcodeRequestPay(queryModel.out_trade_no, queryModel.total_amount.ToString(),"saleorder", content);
            _db.Insert(payHistory);
            _db.SaveChange();

            var request = new BarcodePayRequest();
            request.AddGatewayData(new BarcodePayModel()
            {
                Body = queryModel.body,
                TotalAmount = queryModel.total_amount,
                OutTradeNo = queryModel.out_trade_no,
                AuthCode = queryModel.auth_code
            });
            
            request.PaySucceed += BarcodePay_PaySucceed;
            request.PayFailed += BarcodePay_PayFaild;

            var response = _gateway.Execute(request);
            return response;
        }

        /// <summary>
        /// 支付成功事件
        /// </summary>
        /// <param name="response">返回结果</param>
        /// <param name="message">提示信息</param>
        private void BarcodePay_PaySucceed(IResponse response, string message)
        {
            SavePayResponseHistory(response, true);
        }        

        /// <summary>
        /// 支付失败事件
        /// </summary>
        /// <param name="response">返回结果,可能是BarcodePayResponse/QueryResponse</param>
        /// <param name="message">提示信息</param>
        private void BarcodePay_PayFaild(IResponse response, string message)
        {
            SavePayResponseHistory(response, false);
        }

        private void SavePayResponseHistory(IResponse response, bool success)
        {
            var result = response as BarcodePayResponse;
            var payHistory = new PaymentHistory();
            payHistory.LogWechatBarcodeResponsePay(result.OutTradeNo, result.TotalAmount.ToString(),"saleorder", result.Raw, success);
            _db.Insert(payHistory);
            _db.SaveChange();
        }

        #endregion


        #region 支付查询

        [PayRoute("wechat.trade.query")]
        public QueryResponse Query(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<WechatpayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent, new { trade_no = "",  out_trade_no = "" });

            var request = new QueryRequest();
            request.AddGatewayData(new QueryModel()
            {
                TradeNo = queryModel.trade_no,
                OutTradeNo = queryModel.out_trade_no
            });

            var response = _gateway.Execute(request);
            return response;
        }

        #endregion
        #region 退款接口

        [PayRoute("wechat.trade.refund")]
        public RefundResponse Refund(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<WechatpayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent, 
                new { trade_no = "", out_trade_no = "", total_amount=0, refund_amount=0, refund_reason = "", out_refund_no =""});
            var request = new RefundRequest();
            request.AddGatewayData(new RefundModel()
            {
                TradeNo = queryModel.trade_no,
                RefundAmount = queryModel.refund_amount,
                RefundDesc = queryModel.refund_reason,
                OutRefundNo = queryModel.out_refund_no,
                TotalAmount = queryModel.total_amount,
                OutTradeNo = queryModel.out_trade_no
            });

            var response = _gateway.Execute(request);
            return response;
        }

        #endregion

        #region 退款查询接口

        [PayRoute("wechat.trade.refund.query")]
        public RefundQueryResponse RefundQuery(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<WechatpayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
                new { trade_no = "", out_trade_no = "",  refund_no = "", out_refund_no = "" });
            var request = new RefundQueryRequest();
            request.AddGatewayData(new RefundQueryModel()
            {
                TradeNo = queryModel.trade_no,
                OutTradeNo = queryModel.out_trade_no,
                OutRefundNo = queryModel.out_refund_no,
                RefundNo = queryModel.refund_no
            });

            var response = _gateway.Execute(request);
            return response;
        }

        #endregion

        [PayRoute("wechat.trade.close")]
        public CloseResponse Close(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<WechatpayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
                new {  out_trade_no = "" });
            var request = new CloseRequest();
            request.AddGatewayData(new CloseModel()
            {
                OutTradeNo = queryModel.out_trade_no
            });

            var response = _gateway.Execute(request);
            return response;
        }

        [PayRoute("wechat.trade.cancel")]
        public CancelResponse Cancel(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<WechatpayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
                new { out_trade_no = "" });
            var request = new CancelRequest();
            request.AddGatewayData(new CancelModel()
            {
                OutTradeNo = queryModel.out_trade_no
            });

            var response = _gateway.Execute(request);
            return response;
        }
    }
}