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
   public class RoleQueryService:IRoleQuery
    {
        IQuery _query;
        public RoleQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<Role> GetPageList(Pager page, string name)
        {
            IEnumerable<Role> rows;
            dynamic param = new ExpandoObject();
            string where = " and t0.Id>1"; //不加载系统超管角色
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (page.IsPaging)
            {
                rows = this._query.FindPage<Role>(page.PageIndex, page.PageSize).Where<Role>(where, param);
                page.Total = this._query.Count<Role>(where, param);
            }
            else
            {
                rows = this._query.FindAll<Role>();
                page.Total = this._query.Count<Role>();
            }
            return rows;
        }
    }
}
