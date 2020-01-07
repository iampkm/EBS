using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security;
using EBS.Infrastructure.Extension;
using System.Security.Cryptography;
using EBS.Infrastructure;

namespace EBS.Admin.PayServices
{
    public class SignatureUtil
    {
        /** 默认编码字符集 */
       // private static string DEFAULT_CHARSET = "utf-8";

        private static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        /// <summary>
        ///  构建签名
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public static string BuildMD5Sign(Dictionary<string, string> parameters, string appKey)
        {
            // 1 组装键值对串
            var signContent = GetSignContent(parameters);
            //2 追加appkey
            var raw = signContent + appKey;
            // MD5加密  
            MD5 md5Prider = MD5.Create();
            string sign = md5Prider.GetMd5Hash(raw);
            return sign;
        }

        public static void CheckMD5Sign(string sourceSign, string targetSign)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 != comparer.Compare(sourceSign, targetSign)) {            
                throw new FriendlyException("签名不正确");
            }           
        }
    }
}