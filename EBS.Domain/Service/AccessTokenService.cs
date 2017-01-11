using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
using EBS.Domain.Entity;
namespace EBS.Domain.Service
{
   public class AccessTokenService
    {
        IDBContext _db;
        public AccessTokenService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public void ValidateCDkey(int storeId,int posId,string cdkey)
        {
                MD5 md5Prider = MD5.Create();
                string clientCDKEY = string.Format("{0}-{1}-{2}",storeId,posId,cdkey);
             //加密    
               string clientCDKeyMd5 = md5Prider.GetMd5Hash(clientCDKEY);
               var entity= _db.Table.Find<AccessToken>(n=>n.CDKey==clientCDKeyMd5);
                if(entity==null)
                {
                    throw new Exception("cdkey 不存在");
                }
                if (entity.StoreId == storeId && entity.PosId == posId)
                {
                    //匹配成功，不处理
                }
                else
                {
                    throw new Exception("cdkey 错误");
                }

        }
    }
}
