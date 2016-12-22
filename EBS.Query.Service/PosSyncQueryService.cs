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
        public IEnumerable<AccountSync> QueryAccountSync(Pager page)
        {
            string sql = @"Select Id,UserName,Password,NickName,RoleId,StoreId,Status from Account LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<AccountSync>(sql, null);
            page.Total = this._query.Count<Account>();
           return rows;
        }
        public IEnumerable<StoreSync> QueryStoreSync(Pager page)
        {
            string sql = @"Select Id,Code,Name,LicenseCode from Store LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreSync>(sql, null);
            page.Total = this._query.Count<Store>();
            return rows;
        }
        public IEnumerable<VipCardSync> QueryVipCardSync(Pager page)
        {
            string sql = @"SELECT Id,Code,Discount FROM VipCard LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<VipCardSync>(sql, null);
            page.Total = this._query.Count<VipCard>();
            return rows;
        }

        public IEnumerable<VipProductSync> QueryVipProductSync(Pager page)
        {
            string sql = @"SELECT Id,ProductId,SalePrice FROM VipCard LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<VipProductSync>(sql, null);
            page.Total = this._query.Count<VipProduct>();
            return rows;
        }

        IEnumerable<ProductStorePriceSync> IPosSyncQuery.QueryProductStorePriceSync(Pager page)
        {
            string sql = @"SELECT Id,StoreId,ProductId,SalePrice FROM ProductStorePrice LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<ProductStorePriceSync>(sql, null);
            page.Total = this._query.Count<ProductStorePrice>();
            return rows;
        }

        IEnumerable<ProductAreaPriceSync> IPosSyncQuery.QueryProductAreaPriceSync(Pager page)
        {
            string sql = @"SELECT Id,AreaId,ProductId,SalePrice FROM ProductAreaPrice LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<ProductAreaPriceSync>(sql, null);
            page.Total = this._query.Count<ProductAreaPrice>();
            return rows;
        }

        public IEnumerable<ProductSync> QueryProductSync(Pager page, int storeId)
        {
//            string sql = @"SELECT p.Id,p.`Code`,p.`Name`,p.BarCode,p.Specification,p.SalePrice
//FROM Product p inner join storeInventory i on p.Id = i.ProductId 
//where i.storeId=@StoreId LIMIT {0},{1}";
//            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
//            var rows = this._query.FindAll<ProductSync>(sql, new { StoreId = storeId});
//            page.Total = this._query.Count<StoreInventory>();
//            return rows;

            string sql = @"SELECT p.Id,p.`Code`,p.`Name`,p.BarCode,p.Specification,p.Unit,p.SalePrice
FROM Product p LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<ProductSync>(sql, null);
            page.Total = this._query.Count<Product>();
            return rows;
        }       

        
    }
}
