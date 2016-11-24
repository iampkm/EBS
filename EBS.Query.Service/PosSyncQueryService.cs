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

        public IEnumerable<AccountSync> QueryAccountSync(int[] Ids)
        {
            string sql = @"Select Id,UserName,Password,NickName,RoleId,StoreId,Status from Account where Id in @Id";
            var rows = this._query.FindAll<AccountSync>(sql, new { Id = Ids});
            return rows;
        }

        public IEnumerable<StoreSync> QueryStoreSync(Pager page)
        {
            string sql = @"Select Id,Code,Name from Store LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreSync>(sql, null);
            page.Total = this._query.Count<Store>();
            return rows;
        }

        public IEnumerable<StoreSync> QueryStoreSync(int[] Ids)
        {
            string sql = @"Select Id,Code,Name from Store where Id in @Id";
            var rows = this._query.FindAll<StoreSync>(sql, new { Id = Ids });
            return rows;
        }

        public IEnumerable<VipCardSync> QueryVipCardSync(Pager page)
        {
            string sql = @"SELECT Id,Code,Discount FROM VipCard; LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<VipCardSync>(sql, null);
            page.Total = this._query.Count<VipCard>();
            return rows;
        }

        public IEnumerable<VipCardSync> QueryVipCardSync(int[] Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VipProductSync> QueryVipProductSync(Pager page)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VipProductSync> QueryVipProductSync(int[] Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductSync> QueryProductSync(Pager page)
        {
//            string sql = @"SELECT p.Id,p.`Code`,p.`Name`,p.BarCode,p.Specification,p.SalePrice
//FROM Product p inner join storeInventory i on p.Id = i.ProductId 
//where i.storeId=@StoreId LIMIT {0},{1}";
//            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
//            var rows = this._query.FindAll<ProductSync>(sql, new { StoreId = storeId});
//            page.Total = this._query.Count<StoreInventory>();
//            return rows;

            string sql = @"SELECT p.Id,p.`Code`,p.`Name`,p.BarCode,p.Specification,p.SalePrice
FROM Product p LIMIT {0},{1}";
            sql = string.Format(sql, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<ProductSync>(sql, null);
            page.Total = this._query.Count<Product>();
            return rows;
        }

        public IEnumerable<ProductSync> QueryProductSync(int[] Ids, int storeId)
        {
            string sql = @"SELECT p.Id,p.`Code`,p.`Name`,p.BarCode,p.Specification,p.SalePrice
FROM Product p inner join storeInventory i on p.Id = i.ProductId 
where i.storeId=@StoreId and p.Id in @Id and ";
            var rows = this._query.FindAll<ProductSync>(sql, new { Id = Ids, StoreId = storeId });
            return rows;
        }

        public IEnumerable<ChangeDataSync> QueryChangeData(DateTime lastQueryTime)
        {
            throw new NotImplementedException();
        }
    }
}
