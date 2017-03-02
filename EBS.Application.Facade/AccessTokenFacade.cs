using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
using EBS.Infrastructure;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Domain.Service;
using EBS.Infrastructure.Caching;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
using EBS.Application.Facade.Mapping;
namespace EBS.Application.Facade
{
    public class AccessTokenFacade : IAccessTokenFacade
    {
        IDBContext _db;
        ICacheManager _cacheManager;

        public AccessTokenFacade(IDBContext dbContext, ICacheManager cacheManager)
        {
            _db = dbContext;
            _cacheManager = cacheManager;
        }
        public void ValidateCDKey(AccessTokenModel token)
        {
            string cacheKey = "EBS.AccessToken.All";
            var accessTokens = _cacheManager.Get(cacheKey, () =>
            {
                return _db.Table.FindAll<AccessToken>();
            });

            MD5 md5Prider = MD5.Create();
            string clientCDKEY = string.Format("{0}{1}{2}", token.StoreId, token.PosId, token.CDKey);
            //加密    
            string clientCDKeyMd5 = md5Prider.GetMd5Hash(clientCDKEY);
            // var entity= _db.Table.Find<AccessToken>(n=>n.CDKey==clientCDKeyMd5);
            var entity = accessTokens.FirstOrDefault(n => n.CDKey == clientCDKeyMd5);
            if (entity == null)
            {
                throw new Exception("cdkey 不存在");
            }
            if (entity.StoreId == token.StoreId && entity.PosId == token.PosId)
            {
                //匹配成功，返回
            }
            else
            {
                throw new Exception("cdkey 错误");
            }
        }


        public void Create(AccessTokenModel model)
        {           
            var entity = _db.Table.Find<AccessToken>(n=>n.StoreId==model.StoreId&&n.PosId ==model.PosId);
            if (entity == null) {
                entity =model.MapTo<AccessToken>();
                model.PosCDKey = entity.GenerateClientCDKey(model.PosPassword);
                entity.GenderateServerCDKey(model.PosCDKey);
                model.CDKey = entity.CDKey;
                _db.Insert(entity);
            }
            else {               
                model.PosCDKey = entity.GenerateClientCDKey(model.PosPassword);
                entity.GenderateServerCDKey(model.PosCDKey);
                model.CDKey = entity.CDKey;
                _db.Update(entity);
            }           
            _db.SaveChange();
        }
      
    }
}
