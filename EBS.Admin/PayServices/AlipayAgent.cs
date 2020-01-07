using Dapper.DBContext;
using PaySharp.Alipay;
using PaySharp.Alipay.Domain;
using PaySharp.Alipay.Request;
using PaySharp.Alipay.Response;
using PaySharp.Core;
using PaySharp.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EBS.Infrastructure;
using Newtonsoft.Json;
using EBS.Domain.Entity;

namespace EBS.Admin.PayServices
{
    public class AlipayAgent : IPayAgent
    {
        IGateways _gateways;
        IGateway _gateway;
        IDBContext _db;
        public AlipayAgent(IGateways geteways, IDBContext db)
        {
            this._gateways = geteways;
            this._db = db;
        }

        public AlipayAgent()
        {
            this._gateways = AppContext.Current.Resolve<IGateways>();
            this._db = AppContext.Current.Resolve<IDBContext>();
        }

        #region 条码支付

        [PayRoute("alipay.trade.barcode.pay")]
        public BarcodePayResponse BarcodePay(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<AlipayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent, new { body = "", total_amount = 0, out_trade_no = "", auth_code = "", subject = "" });

            // 记录支付日志
            var content = JsonConvert.SerializeObject(payRequest);
            var payHistory = new PaymentHistory();
            payHistory.LogAlipayBarcodeRequestPay(queryModel.out_trade_no, queryModel.total_amount.ToString(), "saleorder", content);
            _db.Insert(payHistory);
            _db.SaveChange();

            var request = new BarcodePayRequest();
            request.AddGatewayData(new BarcodePayModel()
            {
                Body = queryModel.body,
                TotalAmount = queryModel.total_amount,
                Subject = queryModel.subject,
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
            payHistory.LogAlipayBarcodeResponsePay(result.OutTradeNo, (result.TotalAmount * 100).ToString(), "saleorder", result.Raw, success);
            _db.Insert(payHistory);
            _db.SaveChange();
        }


        [PayRoute("alipay.trade.query")]
        public QueryResponse Query(PayRequest payRequest)
        {
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
              new { out_trade_no = "", trade_no = "" });
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

        [PayRoute("alipay.trade.refund")]
        public RefundResponse Refund(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<AlipayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
                new { out_trade_no = "", trade_no = "", refund_amount = 0, refund_reason = "", out_refund_no = "" });
            // 记录支付日志
            //var content = JsonConvert.SerializeObject(payRequest);
            //var payHistory = new PaymentHistory();
            //payHistory.LogAlipayBarcodeRequestPay(queryModel.out_trade_no, queryModel.total_amount.ToString(), "saleorder", content);
            //_db.Insert(payHistory);
            //_db.SaveChange();

            var request = new RefundRequest();
            request.AddGatewayData(new RefundModel()
            {
                OutTradeNo = queryModel.out_trade_no,
                TradeNo = queryModel.trade_no,
                RefundAmount = queryModel.refund_amount,
                RefundReason = queryModel.refund_reason,
                OutRefundNo = queryModel.out_refund_no
            });

            var response = _gateway.Execute(request);
            return response;
        }

        [PayRoute("alipay.trade.refund.query")]
        public RefundQueryResponse RefundQuery(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<AlipayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
                new { trade_no = "", out_trade_no = "", out_refund_no = "" });
            var request = new RefundQueryRequest();
            request.AddGatewayData(new RefundQueryModel()
            {
                TradeNo = queryModel.trade_no,
                OutTradeNo = queryModel.out_trade_no,
                OutRefundNo = queryModel.out_refund_no
            });

            var response = _gateway.Execute(request);
            return response;
        }

        [PayRoute("alipay.trade.cancel")]
        public CancelResponse Cancel(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<AlipayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
                new { trade_no = "", out_trade_no = "" });
            var request = new CancelRequest();
            request.AddGatewayData(new CancelModel()
            {
                TradeNo = queryModel.trade_no,
                OutTradeNo = queryModel.out_trade_no
            });

            var response = _gateway.Execute(request);
            return response;
        }

        [PayRoute("alipay.trade.close")]
        public CloseResponse Close(PayRequest payRequest)
        {
            _gateway = _gateways.GetByStoreId<AlipayGateway>(payRequest.GetStoreId());
            var queryModel = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payRequest.BizContent,
                new { trade_no = "", out_trade_no = "" });
            var request = new CloseRequest();
            request.AddGatewayData(new CloseModel()
            {
                TradeNo = queryModel.trade_no,
                OutTradeNo = queryModel.out_trade_no
            });

            var response = _gateway.Execute(request);
            return response;
        }
    }
}