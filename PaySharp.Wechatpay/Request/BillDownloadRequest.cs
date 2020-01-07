using PaySharp.Wechatpay.Domain;
using PaySharp.Wechatpay.Response;

namespace PaySharp.Wechatpay.Request
{
    /// <summary>
    /// 商户可以通过该接口下载历史交易清单。比如掉单、系统错误等导致商户侧和微信侧数据不一致，通过对账单核对后可校正支付状态。
    /// </summary>
    public class BillDownloadRequest : BaseRequest<BillDownloadModel, BillDownloadResponse>
    {
        public BillDownloadRequest()
        {
            RequestUrl = "/pay/downloadbill";
        }

        internal override void Execute(Merchant merchant)
        {
            GatewayData.Remove("notify_url");
        }
    }
}
