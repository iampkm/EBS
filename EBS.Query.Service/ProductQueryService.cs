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
        public IEnumerable<ProductDto> GetPageList(Pager page, string name,string codeOrBarCode,string categoryId,int brandId)
        {            
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (!string.IsNullOrEmpty(codeOrBarCode))
            {
                where += "and (t0.Code=@CodeOrBarCode or t0.BarCode=@CodeOrBarCode) ";
                param.CodeOrBarCode = codeOrBarCode;
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                where += "and t0.CategoryId like @CategoryId ";
                param.CategoryId = string.Format("{0}%", categoryId);
            }
            if (brandId > 0)
            {
                where += "and t0.BrandId=@BrandId ";
                param.BrandId = brandId;
            }
            string sql = @"select t0.Id,t0.Name,t0.Code,t0.BarCode,t0.Specification,t0.SalePrice,t1.Name as CategoryName,t2.Name as BrandName 
from product t0 inner join category t1 on t0.CategoryId = t1.Id
inner join brand t2 on t0.BrandId = t2.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
             //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<ProductDto>(sql, param);
            page.Total = this._query.Count<Product>(where, param);
           
            return rows;
        }
    }
}
