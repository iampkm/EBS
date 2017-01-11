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
namespace EBS.Application.Facade
{
   public class AccessTokenFacade:IAccessTokenFacade
    {
        IDBContext _db;
        AccessTokenService _accessTokenService;
        public AccessTokenFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _accessTokenService = new AccessTokenService(_db);
        }
        public void ValidateCDKey(AccessTokenModel token)
        {
            _accessTokenService.ValidateCDkey(token.StoreId,token.PosId,token.CDKey);
        }
    }
}
