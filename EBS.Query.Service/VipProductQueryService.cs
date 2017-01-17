using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Infrastructure.Extension;
namespace EBS.Query.Service
{
   public class VipProductQueryService:IVipProductQuery
    {
       IQuery _query;
       public VipProductQueryService(IQuery query)
        {
            this._query = query;
        }
       public IEnumerable<DTO.VipProductDto> GetPageList(DTO.Pager page, DTO.SearchVipProduct condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (p.`Code`=@ProductCodeOrBarCode or p.BarCode = @ProductCodeOrBarCode)";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }

            if (!string.IsNullOrEmpty(condition.Name))
            {
                where += "and p.`Name` like @ProductName";
                param.ProductName = string.Format("%{0}%",condition.Name);
            }

            string sql = @"select v.Id, p.Id as ProductId, p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit,p.SalePrice,v.SalePrice as VipSalePrice from  VipProduct v left join product p on v.ProductId = p.Id 
where 1=1 {0} ORDER BY v.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<VipProductDto>(sql, param);
            string sqlCount = @"select count(*) from  VipProduct v left join product p on v.ProductId = p.Id
where 1=1 {0} ";
            sql = string.Format(sql, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
            return rows;
        }
    }
}
