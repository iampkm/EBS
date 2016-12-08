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
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;

namespace EBS.Query.Service
{
   public class SupplierQueryService:ISupplierQuery
    {
       IQuery _query;
       public SupplierQueryService(IQuery query)
        {
            this._query = query;
        }
       public IEnumerable<SupplierDto> GetPageList(Pager page, string name,string code)
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
                param.Code = code+"%";
            }           
            string sql = @"select t0.Id,t0.Name,t0.Code,t0.Contact,t0.QQ,t0.Phone,t0.Type,t1.FullName as AreaName  
from supplier t0 inner join Area t1 on t0.AreaId = t1.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<SupplierDto>(sql, param);
            page.Total = this._query.Count<Supplier>(where, param);

            return rows;
        }

        public IEnumerable<SupplierProductDto> GetSupplierProducts(string productCodePriceInput)
        {
            if (string.IsNullOrEmpty(productCodePriceInput)) throw new Exception("商品明细为空");
            var dic = productCodePriceInput.ToDecimalDic();
            string sql = "select p.Id as ProductId,p.Code,p.`Name`,p.Specification,c.FullName as CategoryName from Product p inner join category c on p.categoryId = c.Id where p.code in @Codes";
            var productItems = _query.FindAll<SupplierProductDto>(sql, new { Codes = dic.Keys.ToArray() });
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.Code))
                {
                    product.Price = dic[product.Code];
                }
               
            }
            return productItems;
        }

        public IDictionary<int, string> GetSupplierType()
        {
            return typeof(SupplierType).GetValueToDescription();
        }
    }
}
