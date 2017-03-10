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
       public IEnumerable<AccountSync> QueryAccountSync()
        {
            string sql = @"Select Id,UserName,Password,NickName,RoleId,StoreId,Status from Account";
            var rows = this._query.FindAll<AccountSync>(sql, null);
           return rows;
        }
       public IEnumerable<StoreSync> QueryStoreSync()
        {
            string sql = @"Select Id,Code,Name,LicenseCode from Store ";
            var rows = this._query.FindAll<StoreSync>(sql, null);
            return rows;
        }
        public IEnumerable<VipCardSync> QueryVipCardSync()
        {
            string sql = @"SELECT Id,Code,Discount FROM VipCard ";
            var rows = this._query.FindAll<VipCardSync>(sql, null);
            return rows;
        }

        public IEnumerable<VipProductSync> QueryVipProductSync()
        {
            string sql = @"SELECT Id,ProductId,SalePrice FROM VipProduct ";
            var rows = this._query.FindAll<VipProductSync>(sql, null);
            return rows;
        }

        IEnumerable<ProductStorePriceSync> IPosSyncQuery.QueryProductStorePriceSync(int storeId)
        {
           // string sql = @"SELECT Id,StoreId,ProductId,SalePrice FROM ProductStorePrice where StoreId=@StoreId";
            string sql = @"SELECT Id,StoreId,ProductId,StoreSalePrice as SalePrice FROM storeinventory where StoreId=@StoreId and StoreSalePrice>0 ";
            var rows = this._query.FindAll<ProductStorePriceSync>(sql, new { StoreId = storeId });
            return rows;
        }

        IEnumerable<ProductAreaPriceSync> IPosSyncQuery.QueryProductAreaPriceSync(int storeId)
        {
            string sql = @"SELECT p.Id,p.AreaId,p.ProductId,p.SalePrice FROM ProductAreaPrice p
left join Store s on p.AreaId = s.AreaId
where s.Id=@StoreId";
            var rows = this._query.FindAll<ProductAreaPriceSync>(sql, new { StoreId = storeId });
            return rows;
        }

        public IEnumerable<ProductSync> QueryProductSync(int storeId,string productCodeOrBarCode)
        {
            string sql = @"SELECT p.Id,p.`Code`,p.`Name`,p.BarCode,p.Specification,p.Unit,p.SalePrice
FROM Product p inner join storeInventory i on p.Id = i.ProductId  
where i.storeId=@StoreId ";
            if (!string.IsNullOrEmpty(productCodeOrBarCode))
            {
                sql += string.Format("and (p.Code='{0}' or p.BarCode='{0}')", productCodeOrBarCode);
            }
            var rows = this._query.FindAll<ProductSync>(sql, new { StoreId = storeId });
            return rows;

        }       

        
    }
}
