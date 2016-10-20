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
   public class BrandQueryService :IBrandQuery
    {
         IQuery _query;
        public BrandQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<Brand> GetPageList(Pager page, string name)
        {
            IEnumerable<Brand> rows;
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (page.IsPaging)
            {
                rows = this._query.FindPage<Brand>(page.PageIndex, page.PageSize).Where<Brand>(where, param);
                page.Total = this._query.Count<Brand>(where, param);
            }
            else
            {
                rows = this._query.FindAll<Brand>();
                page.Total = this._query.Count<Brand>();
            }
            return rows;
        }
    }
}
