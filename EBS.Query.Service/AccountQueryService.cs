using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Query.DTO;
namespace EBS.Query.Service
{
   public class AccountQueryService:IAccountQuery 
    {
        IQuery _query;
        public AccountQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<AccountInfo> GetPageList(Pager page, int? id, string userName, string nickName,int storeId)
        {
            IEnumerable<AccountInfo> rows;
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(nickName))
            {
                where += "and t0.NickName like @NickName ";
                param.NickName = string.Format("%{0}%", nickName);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                where += "and t0.UserName = @UserName ";
                param.UserName =userName;
            }
            if (id.HasValue)
            {
                where += "and t0.Id like @Id ";
                param.Id = id.Value;
            }
            if (storeId>=0)
            {
                where += "and t0.StoreId = @StoreId ";
                param.StoreId = storeId;
            }
            if (page.IsPaging)
            {
                string sql = @"Select t0.`Id`,t0.`UserName`,t0.`NickName`,t0.`Status`,t0.`CreatedOn`,t0.`LastUpdateDate`,t0.`LoginErrorCount`,t1.Name as RoleName,t2.Name as StoreName 
from Account t0 inner join Role t1 on t0.RoleId=t1.Id 
left join Store t2 on t0.StoreId = t2.Id
where t0.Id>1 {0} Order By  t0.Id LIMIT {1},{2}";
                sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
                rows = this._query.FindAll<AccountInfo>(sql,param);
                page.Total = this._query.Count<Account>(where, param);
            }
            else
            {
                string sql = "Select t0.`Id`,t0.`UserName`,t0.`NickName`,t0.`Status`,t0.`CreatedOn`,t0.`LastUpdateDate`,t0.`LoginErrorCount`,t1.Name as RoleName  from Account t0 inner join Role t1 on t0.RoleId=t1.Id where t0.Id>1 {0} Order By  t0.Id ";
                sql = string.Format(sql, where);
                rows = this._query.FindAll<AccountInfo>(sql, param);
                page.Total = this._query.Count<Account>();
            }
            return rows;
        }       
    }
}
