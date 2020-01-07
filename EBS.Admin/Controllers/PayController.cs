using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper.DBContext;
using EBS.Admin.PayServices;
using PaySharp.Core;
using PaySharp.Core.Response;
using PaySharp.Wechatpay;
using PaySharp.Wechatpay.Domain;
using PaySharp.Wechatpay.Request;
using PaySharp.Wechatpay.Response;
using PaySharp.Core.Utils;
using EBS.Infrastructure.Log;
using Newtonsoft.Json;
using EBS.Domain;
using EBS.Infrastructure.Extension;
namespace EBS.Admin.Controllers
{
    public class PayController : Controller
    {
        IPayRoute _route;
        ILogger _log;
        public PayController(IPayRoute route,ILogger log)
        {
            this._route = route;
            this._log = log;

        }

        [HttpPost]
        /// <summary>
        /// 支付网关入口
        /// </summary>
        /// <returns></returns>
        public  JsonResult Geteway(PayRequest payRequest)
        {            
            try
            {
                this._log.Info("pay-----------------------");
                this._log.Info("pay.request:{0}", JsonConvert.SerializeObject(payRequest));
                //1  请求参数校验，
                ValidateUtil.Validate(payRequest, null);
                // 2 client-app签名校验           
                var checkSign = SignatureUtil.BuildMD5Sign(payRequest.toDic(), "CC73A89B-E4A4-4E84-9434-4619FE2B523A");
                SignatureUtil.CheckMD5Sign(payRequest.Sign, checkSign);

                // 执行请求
                var routeData = _route.Find(payRequest.Method);
                var payAgent = Activator.CreateInstance(routeData.AgentType);
                var resultObj = routeData.AgentMethod.Invoke(payAgent, new object[] { payRequest });
                var dic = resultObj.ToKeyValueDic(); 
                var resultString = JsonConvert.SerializeObject(resultObj);
                this._log.Info("pay.response:{0}", resultString);
                return Json(new { success = true, data = dic }); 
            }
            catch (Exception ex)
            {
                this._log.Info("pay.response.failed:{0}", ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
           
        }

#if DEBUG
        /// <summary>
        /// 供测试查签名用
        /// </summary>
        /// <param name="payRequest"></param>
        /// <returns></returns>

        [HttpPost]
        public string Sign(PayRequest payRequest)
        {
            var sign = SignatureUtil.BuildMD5Sign(payRequest.toDic(), "123456");
            return sign;
        }
#endif

    }
}