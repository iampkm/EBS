using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
namespace EBS.Domain.Entity
{
   public class AccessToken:BaseEntity
    {
        public int StoreId { get; set; } 
        public int PosId { get; set; }       
        public string CDKey { get; set; }

       /// <summary>
       /// 服务端凭证，采用 门店id，+ posid+ 客户端md5 密码 加密
       /// </summary>
       /// <param name="clientCDKey"></param>
        public void GenderateServerCDKey(string clientCDKey)
        {
            if (string.IsNullOrEmpty(clientCDKey))
            {
                throw new Exception("客户端Key不能为空");
            }
            MD5 md5Prider = MD5.Create();
            string serverKey = string.Format("{0}{1}{2}", this.StoreId, this.PosId, clientCDKey);
            //加密    
            this.CDKey = md5Prider.GetMd5Hash(serverKey);
        }
        
       /// <summary>
       /// 客户单凭证采用 原始密码 md5加密
       /// </summary>
       /// <param name="posPassword"></param>
       /// <returns></returns>
        public string GenerateClientCDKey(string posPassword)
        {
            if (string.IsNullOrEmpty(posPassword))
            {
                throw new Exception("客户端密码不能为空");
            }
            MD5 md5Prider = MD5.Create();
            //加密    
            string md5cdkey = md5Prider.GetMd5Hash(posPassword);
            return md5cdkey;
        }
    }
}
