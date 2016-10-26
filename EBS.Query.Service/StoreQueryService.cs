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
       public IEnumerable<StoreDto> GetPageList(Pager page, string name)
        {
            IEnumerable<Store> rows;
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (page.IsPaging)
            {
                rows = this._query.FindPage<Store>(page.PageIndex, page.PageSize).Where<Store>(where, param);
                page.Total = this._query.Count<Store>(where, param);
            }
            else
            {
                rows = this._query.FindAll<Store>();
                page.Total = this._query.Count<Store>();
            }
            // 转换日期字段 : 临时解决办法
            var result = rows.Select(n => new StoreDto()
             {
                 Id = n.Id,
                 Name = n.Name,
                 Contact = n.Contact,
                 Address = n.Address,
                 CreatedOn = n.CreatedOn.ToString(),
                 Phone = n.Phone
             });
           return result;
        }
    }
}
