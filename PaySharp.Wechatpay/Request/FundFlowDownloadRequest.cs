using PaySharp.Wechatpay.Domain;
using PaySharp.Wechatpay.Response;

namespace PaySharp.Wechatpay.Request
{
    /// <summary>
    /// 商户可以通过该接口下载自2017年6月1日起 的历史资金流水账单。
    /// </summary>
    public class FundFlowDownloadRequest : BaseRequest<FundFlowDownloadModel, FundFlowDownloadResponse>
    {
        public FundFlowDownloadRequest()
        {
            RequestUrl = "/pay/downloadfundflow";
            IsUseCert = true;
        }

        internal override void Execute(Merchant merchant)
        {
            GatewayData.Remove("notify_url");
            GatewayData.Add("sign_type", "HMAC-SHA256");
        }
    }
}
