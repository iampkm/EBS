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
   public class AdjustSalePriceQueryService:IAdjustSalePriceQuery
    {
        IQuery _query;
        public AdjustSalePriceQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<AdjustSalePriceDto> GetPageList(Pager page, SearchAdjustSalePrice condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t2.`Code`=@ProductCodeOrBarCode or t2.BarCode=@ProductCodeOrBarCode)";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and t0.Code=@Code ";
                param.Code = condition.Code;
            }
           
            if (condition.Status != 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            string sql = @"select t0.Id,t0.`Code`,t0.Status,t1.ProductId,t2.`Name` as ProductName,t2.`Code` as ProductCode,t2.BarCode,t2.Specification,
t2.Unit,t1.AdjustPrice,t1.SalePrice,t0.updatedOn,t3.NickName as UpdatedByName 
from AdjustSalePrice t0 
inner join AdjustSalePriceItem t1 on t0.Id = t1.AdjustSalePriceId
inner join product t2 on t1.productid = t2.id
left join account t3 on t0.updatedBy = t3.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            string sqlCount = @"select count(*) from AdjustSalePrice t0 
inner join AdjustSalePriceItem t1 on t0.Id = t1.AdjustSalePriceId
inner join product t2 on t1.productid = t2.id
left join account t3 on t0.updatedBy = t3.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sqlCount = string.Format(sqlCount, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<AdjustSalePriceDto>(sql, param);
            page.Total = this._query.FindScalar<int>(sqlCount, param);

            return rows;
        }


        public IEnumerable<AdjustSalePriceItemDto> GetAdjustSalePriceItems(int AdjustSalePriceId)
        {
            string sql = @"select pc.ProductId,p.Code as ProductCode,p.`Name` as ProductName,p.BarCode,p.Specification,c.FullName as CategoryName,pc.AdjustPrice,pc.ContractPrice from AdjustSalePriceItem pc inner join  Product p on pc.ProductId=p.Id inner join category c on p.categoryId = c.Id 
            where pc.AdjustSalePriceId = @AdjustSalePriceId";
            var productItems = _query.FindAll<AdjustSalePriceItemDto>(sql, new { AdjustSalePriceId = AdjustSalePriceId });
            return productItems;
        }

        public IEnumerable<AdjustSalePriceItemDto> GetItems(int AdjustSalePriceId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,ai.SalePrice,ai.AdjustPrice,i.contractPrice 
from adjustsalepriceitem ai 
left join product p on  ai.ProductId = p.Id
left join purchasecontractitem i on i.productId = p.Id
where ai.AdjustSalePriceId = @AdjustSalePriceId ";
            var result = _query.FindAll<AdjustSalePriceItemDto>(sql, new { AdjustSalePriceId = AdjustSalePriceId });
            return result;
        }


        public AdjustSalePriceItemDto GetAdjustSalePriceItem(string productCodeOrBarCode)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,i.contractPrice 
from product  p left join purchasecontractitem i  on p.Id = i.ProductId
where (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode ) LIMIT 1";
            var item = _query.Find<AdjustSalePriceItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode });
            //设置当前件规            
            if (item == null) { throw new Exception("查无商品"); }
            return item;

        }
        public IEnumerable<AdjustSalePriceItemDto> GetAdjustSalePriceList(string inputProducts)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
           // var dic = GetProductDic(inputProducts);
            var dic = inputProducts.ToDecimalDic();
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,i.contractPrice 
from product  p left join purchasecontractitem i  on p.Id = i.ProductId
where p.`BarCode` in @BarCode order by i.Id DESC";
            var productItems = _query.FindAll<AdjustSalePriceItemDto>(sql, new { BarCode = dic.Keys.ToArray() });
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.BarCode))
                {
                    product.AdjustPrice = dic[product.BarCode]; 
                }               
            }
            return productItems;
        }        

        public Dictionary<int, string> GetAdjustSalePriceStatus()
        {
            var dic = typeof(AdjustSalePriceStatus).GetValueToDescription();
            return dic;
        }

        public IEnumerable<AdjustSalePriceItemDto> GetProductBySource(string source)
        {
            if (source.ToLower() != "pricecheck")
            { 
                 throw new Exception("商品来源source有错");
            }

            string sql = @"select p.Id as ProductId,p.`Code` as ProductCode,p.BarCode,p.`Name` as ProductName,p.Specification,p.Unit,p.SalePrice,i.contractPrice from purchasecontract c 
inner join purchasecontractitem i on c.Id = i.PurchaseContractId
left join product p on i.ProductId= p.Id
where c.`Status` =3 and p.SalePrice<=0  limit 1000";

            var rows = this._query.FindAll<AdjustSalePriceItemDto>(sql, null);
            return rows;
        }
    }
}
