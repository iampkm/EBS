using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Query.SyncObject;
using Dapper.DBContext;
using EBS.Domain.Entity;
namespace EBS.Query.Service
{
   public class PosSyncQueryService:IPosSyncQuery
    {
       IQuery _query;
       public PosSyncQueryService(IQuery query)
       {
           _query = query;
       }
       public IEnumerable<AccountSync> QueryAccountSync(AccessTokenDto token)
        {
            string sql = @"Select Id,UserName,Password,NickName,RoleId,StoreId,Status from Account";
            var rows = this._query.FindAll<AccountSync>(sql, null);
           return rows;
        }
       public IEnumerable<StoreSync> QueryStoreSync(AccessTokenDto token)
        {
            string sql = @"Select Id,Code,Name,LicenseCode from Store ";
            var rows = this._query.FindAll<StoreSync>(sql, null);
            return rows;
        }
        public IEnumerable<VipCardSync> QueryVipCardSync(AccessTokenDto token)
        {
            string sql = @"SELECT Id,Code,Discount FROM VipCard ";
            var rows = this._query.FindAll<VipCardSync>(sql, null);
            return rows;
        }

        public IEnumerable<VipProductSync> QueryVipProductSync(AccessTokenDto token)
        {
            string sql = @"SELECT Id,ProductId,SalePrice FROM VipProduct ";
            var rows = this._query.FindAll<VipProductSync>(sql, null);
            return rows;
        }

        IEnumerable<ProductStorePriceSync> IPosSyncQuery.QueryProductStorePriceSync(AccessTokenDto token)
        {
            string sql = @"SELECT Id,StoreId,ProductId,SalePrice FROM ProductStorePrice where StoreId=@StoreId";
            var rows = this._query.FindAll<ProductStorePriceSync>(sql, new { StoreId=token.StoreId});
            return rows;
        }

        IEnumerable<ProductAreaPriceSync> IPosSyncQuery.QueryProductAreaPriceSync(AccessTokenDto token)
        {
            string sql = @"SELECT p.Id,p.AreaId,p.ProductId,p.SalePrice FROM ProductAreaPrice p
left join Store s on p.AreaId = s.AreaId
where s.Id=@StoreId";
            var rows = this._query.FindAll<ProductAreaPriceSync>(sql, new { StoreId = token.StoreId });
            return rows;
        }

        public IEnumerable<ProductSync> QueryProductSync(AccessTokenDto token)
        {
            string sql = @"SELECT p.Id,p.`Code`,p.`Name`,p.BarCode,p.Specification,p.Unit,p.SalePrice
FROM Product p inner join storeInventory i on p.Id = i.ProductId  
where i.storeId=@StoreId ";

            var rows = this._query.FindAll<ProductSync>(sql, new { StoreId = token.StoreId });
            return rows;

        }       

        
    }
}
