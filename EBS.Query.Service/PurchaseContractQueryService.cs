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
   public class PurchaseContractQueryService:IPurchaseContractQuery
    {
        IQuery _query;
        public PurchaseContractQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<PurchaseContractDto> GetPageList(Pager page, string code, string name, int supplierId)
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
                where += "and t0.Code=@Code ";
                param.Code = code;
            }           
            if (supplierId > 0)
            {
                where += "and t0.SupplierId=@SupplierId ";
                param.SupplierId = supplierId;
            }
            string sql = @"select t0.Id,t0.Name,t0.Code,t0.SupplierId,t0.Contact,t0.Cooperate,t0.StartDate,t0.EndDate,t0.Status,t1.Name as SupplierName  
from PurchaseContract t0 inner join supplier t1 on t0.SupplierId = t1.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<PurchaseContractDto>(sql, param);
            page.Total = this._query.Count<PurchaseContract>(where, param);

            return rows;
        }
    }
}
