using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain
{
    /// <summary>
    ///  所有配置key 
    /// </summary>
   public class SettingKeys
    {
        public static readonly string System_Domain = "system.domain";

        #region 通用支付配置

        /// <summary>
        ///  分配收银前台访问Appid
        /// </summary>
        public static readonly string Pay_Appid = "pay.appid";
        /// <summary>
        /// 收银前台访问appkey
        /// </summary>
        public static readonly string Pay_AppKey = "pay.appkey";
        public static readonly string Pay_Notify_Url = "pay.notify.url";
        public static readonly string Pay_Return_Url = "pay.return.url";

        #endregion

        #region Alipay
        public static readonly string Pay_Alipay_Appid = "pay.alipay.appid";
        public static readonly string Pay_Alipay_Public_Key = "pay.alipay.public.key";
        public static readonly string Pay_Alipay_Private_Key = "pay.alipay.private.key";

        #endregion

        #region Wechatpay
        public static readonly string Pay_Wechat_Appid = "pay.wechat.appid";
        public static readonly string Pay_Wechat_MchId = "pay.wechat.mchid";
        public static readonly string Pay_Wechat_MchKey = "pay.wechat.mchkey";
        public static readonly string Pay_Wechat_AppSecret = "pay.wechat.appsecret";
        #endregion


    }
}
