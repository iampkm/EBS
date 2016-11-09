using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
namespace EBS.Query.Service
{
   public class StoreQueryService:IStoreQuery
    {
       IQuery _query;
       public StoreQueryService(IQuery query)
        {
            this._query = query;
        }
       public IEnumerable<StoreDto> GetPageList(Pager page, string name,string code)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (!string.IsNullOrEmpty(code))
            {
                where += "and t0.Code like @Code ";
                param.Code = string.Format("{0}%", code);
            }
            string sql = "select t0.*,t1.FullName from Store t0 left join Area t1 on t0.AreaId=t1.Id where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreDto>(sql, param);
            page.Total = this._query.Count<Store>(where, param);
            
            return rows;
        }
    }
}
