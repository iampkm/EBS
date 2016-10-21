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
   public class ProductQueryService:IProductQuery
    {
        IQuery _query;
        public ProductQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<Product> GetPageList(Pager page, string name)
        {
            IEnumerable<Product> rows;
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (page.IsPaging)
            {
                rows = this._query.FindPage<Product>(page.PageIndex, page.PageSize).Where<Product>(where, param);
                page.Total = this._query.Count<Product>(where, param);
            }
            else
            {
                rows = this._query.FindAll<Product>();
                page.Total = this._query.Count<Product>();
            }
            return rows;
        }
    }
}
