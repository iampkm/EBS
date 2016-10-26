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
   public class SupplierQueryService:ISupplierQuery
    {
       IQuery _query;
       public SupplierQueryService(IQuery query)
        {
            this._query = query;
        }
       public IEnumerable<Supplier> GetPageList(Pager page, string name)
        {
            IEnumerable<Supplier> rows;
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (page.IsPaging)
            {
                rows = this._query.FindPage<Supplier>(page.PageIndex, page.PageSize).Where<Supplier>(where, param);
                page.Total = this._query.Count<Supplier>(where, param);
            }
            else
            {
                rows = this._query.FindAll<Supplier>();
                page.Total = this._query.Count<Supplier>();
            }
            return rows;
        }
    }
}
