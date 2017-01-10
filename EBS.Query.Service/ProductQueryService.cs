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
using EBS.Infrastructure.Extension;
namespace EBS.Query.Service
{
    public class ProductQueryService : IProductQuery
    {
        IQuery _query;
        public ProductQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<ProductDto> GetPageList(Pager page, string name, string codeOrBarCode, string categoryId, int brandId)
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

        public PriceTagDto QueryPriceTag(string productCodeOrBarCode)
        {
            string sql = "select Id,Name,Code,BarCode,Specification,Unit,/*Grade,MadeIn,*/SalePrice from product where Code = @ProductCodeOrBarCode or BarCode =@ProductCodeOrBarCode";
            var model = _query.Find<PriceTagDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode });
            return model;
        }

        public IEnumerable<PriceTagDto> QueryProductPriceTagList(string ids)
        {
            var idArray = ids.Split(',').ToIntArray();
            string sql = "select Id,Name,Code,BarCode,Specification,Unit,Grade,MadeIn,SalePrice from product where Id in @Ids";
            var rows = _query.FindAll<PriceTagDto>(sql, new { Ids = idArray });
            return rows;
        }


        public string GenerateBarCode()
        {
            string sql = "select barCode from Product where BarCode like '88%' and length(barCode) between 6 and 8 order by BarCode desc limit 1";
            var lastBarCode = _query.Find<string>(sql, null);
            var barCode = "";
            if (string.IsNullOrEmpty(lastBarCode))
            {
                barCode = "880001";
            }
            else
            {
                var number= Convert.ToInt64(lastBarCode.Substring(2)) + 1;
                if (number > 999999) throw new Exception("自建条码已达上限999999");
                var numberCode = number > 9999 ? number.ToString() : number.ToString().PadLeft(4, '0');
                barCode = "88" + numberCode.ToString();
            }
            return barCode;
        }

        public IEnumerable<ProductCheckDto> QueryContractProductNoSalePrice(string productCodeOrBarCode)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            string sql = @"select p.Id,p.`Code` as ProductCode,p.BarCode,p.`Name` as ProductName,p.Specification,p.Unit,p.SalePrice from purchasecontract c 
inner join purchasecontractitem i on c.Id = i.PurchaseContractId
left join product p on i.ProductId= p.Id
where c.`Status` =3 and p.SalePrice<=0 ";
          
            if (string.IsNullOrEmpty(productCodeOrBarCode))
            {
                where += "and (p.Code=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = productCodeOrBarCode;
            }
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<ProductCheckDto>(sql, param);
            return rows;

        }
    }
}
