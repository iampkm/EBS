using Dapper.DBContext;
using EBS.Query.DTO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.Service
{
   public class AccessTokenQueryService:IAccessTokenQuery
    {
       IQuery _query;
       public AccessTokenQueryService(IQuery query)
        {
            this._query = query;
        }
       public IEnumerable<AccessTokenDto> GetPageList(Pager page, int storeId)
        {
            IEnumerable<AccessTokenDto> rows;
            dynamic param = new ExpandoObject();
            string where = "";
           
            if (storeId>0)
            {
                where += "and t0.StoreId = @StoreId ";
                param.StoreId = storeId;
            }
           
                string sql = @"Select *,t1.`Name` as StoreName from AccessToken t0 left join Store t1 on t0.StoreId = t1.Id
where 1=1  {0} Order By  t0.Id LIMIT {1},{2}";
                sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
                rows = this._query.FindAll<AccessTokenDto>(sql,param);
                string sqlCount = @"Select count(*) from AccessToken t0 left join Store t1 on t0.StoreId = t1.Id
where 1=1  {0}";
                sqlCount = string.Format(sqlCount, where);
                page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
           
            return rows;
        }  
    }
}
