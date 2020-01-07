using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace EBS.Admin.PayServices
{
    /// <summary>
    ///  支付接口公共请求
    /// </summary>
    public class PayRequest
    {
        /// <summary>
        ///  分配的可连接支付APP
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        ///  链接支付门店ID
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 接口名： wechat.trade.barcode.pay
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        ///  版本 1.0
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///  数据格式：json
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 字符集：默认 UTF-8 
        /// </summary>
        public string charset { get; set; }
        /// <summary>
        /// 加密方式：默认 MD5
        /// </summary>
        public string SignType { get; set; }

        [Required(ErrorMessage = "签名不能为空")]
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }


        [Required(ErrorMessage = "时间戳必填")]
        /// <summary>
        ///  发送请求的时间，格式"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string Timestamp { get; set; }
        [Required(ErrorMessage = "业务请求内容参数必填")]
        /// <summary>
        ///  json业务请求参数
        /// </summary>
        public string BizContent { get; set; }


        public Dictionary<string, string> toDic()
        {
            var type = this.GetType();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var prop in type.GetProperties())
            {
                if (!dic.ContainsKey(prop.Name) && prop.Name.ToLower()!="sign")
                {
                    dic.Add(prop.Name, prop.GetValue(this).ToString());
                }
            }
            return dic;
        }

        public int GetStoreId()
        {
            return string.IsNullOrEmpty(this.StoreId) ? 0 : Convert.ToInt32(this.StoreId);
        }

    }
}