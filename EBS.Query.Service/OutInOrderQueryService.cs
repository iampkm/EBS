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
using EBS.Infrastructure;
namespace EBS.Query.Service
{
    public class OutInOrderQueryService : IOutInOrderQuery
    {
        IQuery _query;
        public OutInOrderQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<DTO.OutInOrderDto> GetPageList(DTO.Pager page, DTO.SearchOutInOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and o.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.Status != 0)
            {
                where += "and o.Status=@Status ";
                param.Status = condition.Status;
            }
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and  o.StoreId in @StoreId  ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray();                
            }

            if (condition.StartDate.HasValue)
            {
                where += "and o.CreatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and o.CreatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += @"and o.Id in (select d.OutInOrderId from OutInOrderitem d left join product p on p.id = d.productid  where p.code=@ProductCodeOrBarCode or p.barcode =@ProductCodeOrBarCode  ) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }

            string sql = @"select o.Id,o.Code,o.FromStoreName,o.ToStoreName,o.Status,o.CreatedByName,o.UpdatedByName,o.CreatedOn, t.TotalQuantity,t.TotalAmount
from OutInOrder o left join 
(select i.OutInOrderId,sum(i.Quantity) as TotalQuantity ,sum(i.price* i.Quantity) as TotalAmount 
from OutInOrderitem i GROUP BY i.OutInOrderId ) t on o.Id = t.OutInOrderId
where 1=1 {0} ORDER BY o.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<OutInOrderDto>(sql, param);
            string sqlCount = @"select count(*) from OutInOrder o left join 
(select i.OutInOrderId,sum(i.Quantity) as TotalQuantity ,sum(i.price* i.Quantity) as TotalAmount 
from OutInOrderitem i GROUP BY i.OutInOrderId ) t on o.Id = t.OutInOrderId
where 1=1 {0} ORDER BY o.Id desc ";
            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);

            return rows;
        }


        public List<OutInOrderItemDto> QueryProductBatch(string productCodeOrBarCode, int storeId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 b.ContractPrice,b.Price,b.SupplierId,b.BatchNo ,s.`Name` as SupplierName,b.Quantity AS BatchQuantity
from storeinventorybatch b left join  product p on p.Id = b.ProductId
left join supplier s on b.SupplierId = s.Id
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode)  and  b.Quantity>0   and b.StoreId = @StoreId
ORDER BY b.Id ";
            var rows = this._query.FindAll<OutInOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId }).ToList();
          
            return rows;
        }

        public OutInOrderItemDto QueryProduct(string productCodeOrBarCode, int storeId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 b.ContractPrice,b.Price,b.SupplierId,b.BatchNo ,s.`Name` as SupplierName,i.Quantity AS InventoryQuantity
from storeinventorybatch b left join  product p on p.Id = b.ProductId
left join supplier s on b.SupplierId = s.Id
left join (select productId,Quantity from storeinventory where storeid = @StoreId ) i on b.productId = i.productId
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode)  and  b.Quantity>0   and b.StoreId = @StoreId
ORDER BY b.Id Limit 1";
            var model = this._query.Find<OutInOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });
            if (model == null)
            {
                throw new FriendlyException("商品不存在");
            }
         
            return model;
        }

        public IEnumerable<OutInOrderItemDto> ImportProducts(int storeId, string inputBarCodes)
        {
            if (string.IsNullOrEmpty(inputBarCodes)) throw new FriendlyException("商品明细为空");
            // var dic = GetProductDic(inputProducts);
            var dic = inputBarCodes.ToIntDic();
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,s.LastCostPrice ,s.StoreSalePrice
from storeinventory s left join product  p on p.Id = s.ProductId
where p.`BarCode` in @BarCode and s.StoreId =@StoreId ";
            var productItems = _query.FindAll<OutInOrderItemDto>(sql, new { BarCode = dic.Keys.ToArray(), StoreId = storeId });
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.BarCode))
                {
                    product.Quantity = dic[product.BarCode];
                }

            }
            return productItems;
        }

        public OutInOrderDto GetById(int id)
        {
            string sql = "select * from OutInOrder where Id=@Id";
            var model = _query.Find<OutInOrderDto>(sql, new { Id = id });
            if (model == null)
            {
                throw new FriendlyException("单据不存在");
            }
            string sqlItem = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, i.SupplierId,i.ContractPrice,i.Price,i.Quantity,i.BatchNo,s.Quantity as InventoryQuantity 
from OutInOrderitem i left join  product p on p.Id = i.ProductId 
left join storeinventory s on s.productid = i.productid and s.storeId =@FromStoreId  
where i.OutInOrderId=@OutInOrderId";
            var items = _query.FindAll<OutInOrderItemDto>(sqlItem, new { OutInOrderId = model.Id, StoreId = model.StoreId }).ToList();
            model.Items = items;           
            return model;

        }





        public IEnumerable<OutInOrderListDto> GetFinishList(Pager page, SearchOutInOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and o.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.Status != 0)
            {
                where += "and o.Status=@Status ";
                param.Status = condition.Status;
            }
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                
            where += "and  o.StoreId in @StoreId  ";
            param.StoreId = condition.StoreId.Split(',').ToIntArray();
                              
            }

            if (condition.StartDate.HasValue)
            {
                where += "and o.UpdatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and o.UpdatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += @"and ( p.code=@ProductCodeOrBarCode or p.barcode =@ProductCodeOrBarCode  ) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.ProductName))
            {
                where += "and p.Name like @ProductName ";
                param.ProductName = string.Format("%{0}%", condition.ProductName);
            }

            string sql = @"select o.Id,o.Code,o.FromStoreName,o.ToStoreName,o.Status,o.CreatedByName,o.CreatedOn, o.UpdatedOn,p.code as ProductCode,p.BarCode,p.`Name` as ProductName,p.Specification,
i.Price,i.Quantity,i.Price* i.Quantity as Amount 
from OutInOrder o 
inner join OutInOrderitem i on o.Id = i.OutInOrderId 
left join product p on p.Id = i.ProductId
where 1=1 {0} ORDER BY o.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<OutInOrderListDto>(sql, param);
            string sqlSum = @"
select count(*) as TotalCount,sum(i.Quantity) as Quantity,sum(i.Price* i.Quantity) as Amount 
from OutInOrder o 
inner join OutInOrderitem i on o.Id = i.OutInOrderId 
left join product p on p.Id = i.ProductId
where 1=1 {0} ";

            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory = this._query.Find<SumOutInOrderSummary>(sqlSum, param) as SumOutInOrderSummary;
            page.Total = sumStoreInventory.TotalCount;
            page.SumColumns.Add(new SumColumn("Quantity", sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
            return rows;
        }


        public IEnumerable<OutInOrderSummaryDto> GetSummaryList(Pager page, SearchOutInOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";

            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and o.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray();
            }

            if (condition.StartDate.HasValue)
            {
                where += "and o.CreatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and o.CreatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }
            //if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            //{
            //    where += @"and ( p.code=@ProductCodeOrBarCode or p.barcode =@ProductCodeOrBarCode  ) ";
            //    param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            //}
            //if (!string.IsNullOrEmpty(condition.ProductName))
            //{
            //    where += "and p.Name like @ProductName ";
            //    param.ProductName = string.Format("%{0}%", condition.ProductName);
            //}

            string sql = @"select s.`Name` as StoreName,t.InQuantity,t.InAmount,t.OutQuantity,t.OutAmount,t.InQuantity+t.OutQuantity as DifferenceQuantity,t.InAmount+t.OutAmount as DifferenceAmount from ( 
select StoreId,
sum(case when ChangeQuantity<0 then ChangeQuantity end) OutQuantity,sum(case when ChangeQuantity<0 then price* ChangeQuantity end) OutAmount,
sum(case when ChangeQuantity>=0 then ChangeQuantity end) InQuantity,sum(case when ChangeQuantity>=0 then price* ChangeQuantity end) InAmount from storeinventoryhistory o 
where BillType=60 {0}  
GROUP BY storeid ORDER BY o.Id desc
) t
left join store s on s.id = t.storeid  LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<OutInOrderSummaryDto>(sql, param);
            string sqlSum = @"
select count(*) as TotalCount,sum(t.InQuantity) as InQuantity,sum(InAmount) as InAmount, sum(t.OutQuantity) as OutQuantity,sum(OutAmount) as OutAmount,sum(t.InQuantity+t.OutQuantity) as DifferenceQuantity,sum(t.InAmount+t.OutAmount) as DifferenceAmount
from ( 
select 
sum(case when ChangeQuantity<0 then ChangeQuantity end) OutQuantity,sum(case when ChangeQuantity<0 then price* ChangeQuantity end) OutAmount,
sum(case when ChangeQuantity>=0 then ChangeQuantity end) InQuantity,sum(case when ChangeQuantity>=0 then price* ChangeQuantity end) InAmount from storeinventoryhistory o 
where BillType=60 {0}  
GROUP BY storeid ORDER BY o.Id desc
) t ";

            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory = this._query.Find<SumOutInOrderSummary>(sqlSum, param) as SumOutInOrderSummary;
            page.Total = sumStoreInventory.TotalCount;
            //page.SumColumns.Add(new SumColumn("InQuantity", sumStoreInventory.InQuantity.ToString()));
            //page.SumColumns.Add(new SumColumn("InAmount", sumStoreInventory.InAmount.ToString("F4")));
            //page.SumColumns.Add(new SumColumn("OutQuantity", sumStoreInventory.OutQuantity.ToString()));
            //page.SumColumns.Add(new SumColumn("OutAmount", sumStoreInventory.OutAmount.ToString("F4")));
            //page.SumColumns.Add(new SumColumn("DifferenceQuantity", sumStoreInventory.DifferenceQuantity.ToString()));
            //page.SumColumns.Add(new SumColumn("DifferenceAmount", sumStoreInventory.DifferenceAmount.ToString("F4")));
            return rows;
        }
    }
}
