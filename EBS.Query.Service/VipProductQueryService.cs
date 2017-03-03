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
            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
            return rows;
        }


       public IEnumerable<VipProductDto> QueryProductByBarCode(string inputProducts)
       {
           if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
           var dic = inputProducts.ToDecimalDic();
           string sql = @"select p.Id as ProductId, p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit,p.SalePrice,i.price  from (
select s.productid,s.price from storeinventorybatch s right join (
SELECT b.productid,MAX(b.id) as bid from storeinventorybatch b 
group by b.productid ) t on s.id = bid ) i
left join product p  on p.id = i.productid
where p.BarCode in @BarCodes  ";
           var productItems= this._query.FindAll<VipProductDto>(sql, new { BarCodes = dic.Keys.ToArray() });
           foreach (var product in productItems)
           {
               if (dic.ContainsKey(product.BarCode))
               {
                   product.VipSalePrice = dic[product.BarCode];
               }
           }
           return productItems;
       }


       public VipProductDto QueryProduct(string productCodeOrBarCode)
       {
           if (string.IsNullOrEmpty(productCodeOrBarCode)) throw new Exception("商品条码为空");
           string sql = @"select v.Id, p.Id as ProductId, p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit,p.SalePrice,v.SalePrice as VipSalePrice,i.price  from product p left join VipProduct v  on v.ProductId = p.Id 
             left join storeinventorybatch i on i.productId = p.Id 
where p.BarCode = @ProductCodeOrBarCode or p.Code =@ProductCodeOrBarCode 
order by i.Id desc LIMIT 1";
           var model = this._query.Find<VipProductDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode });
           if (model == null) throw new Exception("商品不存在");
           return model;
       }
    }
}
