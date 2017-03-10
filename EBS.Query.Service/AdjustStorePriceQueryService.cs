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
    public class AdjustStorePriceQueryService : IAdjustStorePriceQuery
    {
        IQuery _query;
        public AdjustStorePriceQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<AdjustStorePriceDto> GetPageList(Pager page, SearchAdjustStorePrice condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += @"and t0.Id in (
select i.adjuststorepriceId from adjuststorepriceitem i left join product p on p.Id = i.productId
where (p.`Code`=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode))";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and t0.Code=@Code ";
                param.Code = condition.Code;
            }
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray();
            }
            if (condition.Status != 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            if (condition.CreatedOn.HasValue)
            {
                where += "and DATE(t0.CreatedOn) =@CreatedOn ";
                param.CreatedOn = condition.CreatedOn.Value;
            }
            string sql = @"select t0.Id,t0.Code,t0.Status,t0.UpdatedOn,t0.CreatedOn,t0.Remark,t1.name as StoreName,t3.NickName as CreatedByName  
from AdjustStorePrice t0 
left join store t1 on t1.Id = t0.StoreId
left join account t3 on t0.createdBy = t3.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            string sqlCount = @"select count(*) from AdjustStorePrice t0 
left join store t1 on t1.Id = t0.StoreId
left join account t3 on t0.updatedBy = t3.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sqlCount = string.Format(sqlCount, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<AdjustStorePriceDto>(sql, param);
            page.Total = this._query.FindScalar<int>(sqlCount, param);

            return rows;
        }

        public IEnumerable<AdjustStorePriceListDto> QueryFinish(Pager page, SearchAdjustStorePrice condition)
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
            if (!string.IsNullOrEmpty(condition.StoreId)&&condition.StoreId!="0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray();
            }
            if (condition.Status != 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            if (condition.CreatedOn.HasValue)
            {
                where += "and DATE(t0.CreatedOn) =@CreatedOn ";
                param.CreatedOn = condition.CreatedOn.Value;
            }
            string sql = @"select t0.Id,t0.`Code`,t0.Status,t0.Remark,t1.ProductId,t2.`Name` as ProductName,t2.`Code` as ProductCode,t2.BarCode,t2.Specification,
t2.Unit,t1.AdjustPrice,t1.StoreSalePrice,t0.updatedOn,t3.NickName as UpdatedByName,t4.`Name` as StoreName 
from AdjustStorePrice t0 
inner join AdjustStorePriceItem t1 on t0.Id = t1.AdjustStorePriceId
left join product t2 on t1.productid = t2.id
left join account t3 on t0.updatedBy = t3.Id
left join Store t4 on t4.Id = t0.StoreId
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            string sqlCount = @"select count(*) from AdjustStorePrice t0 
inner join AdjustStorePriceItem t1 on t0.Id = t1.AdjustStorePriceId
left join product t2 on t1.productid = t2.id
left join account t3 on t0.updatedBy = t3.Id
left join Store t4 on t4.Id = t0.StoreId
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sqlCount = string.Format(sqlCount, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<AdjustStorePriceListDto>(sql, param);
            page.Total = this._query.FindScalar<int>(sqlCount, param);

            return rows;
        }


        public IEnumerable<AdjustStorePriceItemDto> GetAdjustStorePriceItems(int AdjustStorePriceId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,ai.AdjustPrice,i.LastCostPrice,i.StoreSalePrice 
from AdjustStorePriceitem ai 
inner join AdjustStorePrice a on ai.AdjustStorePriceId = a.Id
left join product p on  ai.ProductId = p.Id
left join storeinventory i on i.productId = p.Id and i.StoreId = a.StoreId
where ai.AdjustStorePriceId = @AdjustStorePriceId ";
            var productItems = _query.FindAll<AdjustStorePriceItemDto>(sql, new { AdjustStorePriceId = AdjustStorePriceId });
            return productItems;
        }

        public IEnumerable<AdjustStorePriceItemDto> GetItems(int AdjustStorePriceId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,ai.AdjustPrice,i.LastCostPrice,i.StoreSalePrice 
from AdjustStorePriceitem ai 
inner join AdjustStorePrice a on ai.AdjustStorePriceId = a.Id
left join product p on  ai.ProductId = p.Id
left join storeinventory i on i.productId = p.Id and i.StoreId = a.StoreId
where ai.AdjustStorePriceId = @AdjustStorePriceId ";
            var result = _query.FindAll<AdjustStorePriceItemDto>(sql, new { AdjustStorePriceId = AdjustStorePriceId });
            return result;
        }


        public AdjustStorePriceItemDto GetAdjustStorePriceItem(int storeId, string productCodeOrBarCode)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,s.LastCostPrice ,s.StoreSalePrice
from storeinventory s left join product  p on p.Id = s.ProductId
where (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode ) and s.StoreId =@StoreId   LIMIT 1";
            var item = _query.Find<AdjustStorePriceItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });
            //设置当前件规            
            if (item == null) { throw new Exception("查无商品"); }
            return item;

        }
        public IEnumerable<AdjustStorePriceItemDto> GetAdjustStorePriceList(int storeId, string inputProducts)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            // var dic = GetProductDic(inputProducts);
            var dic = inputProducts.ToDecimalDic();
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,s.LastCostPrice ,s.StoreSalePrice
from storeinventory s left join product  p on p.Id = s.ProductId
where p.`BarCode` in @BarCode and s.StoreId =@StoreId ";
            var productItems = _query.FindAll<AdjustStorePriceItemDto>(sql, new { BarCode = dic.Keys.ToArray(), StoreId = storeId });
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.BarCode))
                {
                    product.AdjustPrice = dic[product.BarCode];
                }
            }
            return productItems;
        }

        public Dictionary<int, string> GetAdjustStorePriceStatus()
        {
            var dic = typeof(AdjustStorePriceStatus).GetValueToDescription();
            return dic;
        }


        public AdjustStorePriceDto GetById(int id)
        {
            string sql = @"select t0.*,t2.NickName as CreatedByName,t1.name as StoreName,t3.NickName as UpdatedByName  from AdjustStorePrice t0 
left join store t1 on t1.Id = t0.StoreId
left join account t2 on t0.CreatedBy = t2.Id 
left join account t3 on t0.updatedBy = t3.Id where t0.Id=@Id";
            var model = _query.Find<AdjustStorePriceDto>(sql, new { Id = id });
           if (model == null) {
               throw new Exception("调价单不存在");
           }
           model.Items = GetAdjustStorePriceItems(id);
           return model;
        }
    }
}
