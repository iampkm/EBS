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


        //public void ValidateCDKey(string clientCDKey)
        //{ 
        //     MD5 md5Prider = MD5.Create();
        //    string clientCDKEY = string.Format("{0}-{1}-{2}",cli)
        //    if (!md5Prider.VerifyMd5Hash(password, this.CDKey))
        //}
    }
}
