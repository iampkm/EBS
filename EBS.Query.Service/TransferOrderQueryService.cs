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
    public class TransferOrderQueryService : ITransferOrderQuery
    {
        IQuery _query;
        public TransferOrderQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<DTO.TransferOrderDto> GetPageList(DTO.Pager page, DTO.SearchTransferOrder condition)
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
                if (condition.From.HasValue && condition.To.HasValue && condition.From.Value && condition.To.Value || !condition.From.HasValue && !condition.To.HasValue)
                {
                    where += "and ( o.FromStoreId in @StoreId or o.ToStoreId in @StoreId ) ";
                    param.StoreId = condition.StoreId.Split(',').ToIntArray();
                }
                else
                {
                    if (condition.From.HasValue && condition.From.Value)
                    {
                        where += "and o.FromStoreId in @StoreId ";
                        param.StoreId = condition.StoreId.Split(',').ToIntArray();
                    }
                    if (condition.To.HasValue && condition.To.Value)
                    {
                        where += "and o.ToStoreId in @StoreId ";
                        param.StoreId = condition.StoreId.Split(',').ToIntArray();
                    }  
                }
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
                where += @"and o.Id in (select d.transferOrderId from transferorderitem d left join product p on p.id = d.productid  where p.code=@ProductCodeOrBarCode or p.barcode =@ProductCodeOrBarCode  ) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }

            string sql = @"select o.Id,o.Code,o.FromStoreName,o.ToStoreName,o.Status,o.CreatedByName,o.UpdatedByName,o.CreatedOn, t.TotalQuantity,t.TotalAmount
from transferorder o left join 
(select i.TransferOrderId,sum(i.Quantity) as TotalQuantity ,sum(i.price* i.Quantity) as TotalAmount 
from transferorderitem i GROUP BY i.transferorderId ) t on o.Id = t.TransferOrderId
where 1=1 {0} ORDER BY o.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<TransferOrderDto>(sql, param);
            string sqlCount = @"select count(*) from transferorder o left join 
(select i.TransferOrderId,sum(i.Quantity) as TotalQuantity ,sum(i.price* i.Quantity) as TotalAmount 
from transferorderitem i GROUP BY i.transferorderId ) t on o.Id = t.TransferOrderId
where 1=1 {0} ORDER BY o.Id desc ";
            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);

            return rows;
        }


        public List<TransaferOrderItemDto> QueryProductBatch(string productCodeOrBarCode, int storeId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 b.ContractPrice,b.Price,b.SupplierId,b.BatchNo ,s.`Name` as SupplierName,b.Quantity AS BatchQuantity
from storeinventorybatch b left join  product p on p.Id = b.ProductId
left join supplier s on b.SupplierId = s.Id
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode)  and  b.Quantity>0   and b.StoreId = @StoreId
ORDER BY b.Id ";
            var rows = this._query.FindAll<TransaferOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId }).ToList();
            rows.ForEach(item => item.SetSpecificationQuantity());
            return rows;
        }

        public TransaferOrderItemDto QueryProduct(string productCodeOrBarCode, int storeId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 b.ContractPrice,b.Price,b.SupplierId,b.BatchNo ,s.`Name` as SupplierName,i.Quantity AS InventoryQuantity
from storeinventorybatch b left join  product p on p.Id = b.ProductId
left join supplier s on b.SupplierId = s.Id
left join (select productId,Quantity from storeinventory where storeid = @StoreId ) i on b.productId = i.productId
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode)  and  b.Quantity>0   and b.StoreId = @StoreId
ORDER BY b.Id Limit 1";
            var model = this._query.Find<TransaferOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });
            if (model == null)
            {
                throw new Exception("商品不存在");
            }
            model.SetSpecificationQuantity();
            return model;
        }

        public IEnumerable<TransaferOrderItemDto> ImportProducts(int storeId, string inputBarCodes)
        {
            if (string.IsNullOrEmpty(inputBarCodes)) throw new Exception("商品明细为空");
            // var dic = GetProductDic(inputProducts);
            var dic = inputBarCodes.ToIntDic();
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,
p.Unit,p.SalePrice,s.LastCostPrice ,s.StoreSalePrice
from storeinventory s left join product  p on p.Id = s.ProductId
where p.`BarCode` in @BarCode and s.StoreId =@StoreId ";
            var productItems = _query.FindAll<TransaferOrderItemDto>(sql, new { BarCode = dic.Keys.ToArray(), StoreId = storeId });
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.BarCode))
                {
                    product.Quantity = dic[product.BarCode];
                }
                product.SetSpecificationQuantity();
            }
            return productItems;
        }

        public TransferOrderDto GetById(int id)
        {
            string sql = "select * from transferorder where Id=@Id";
            var model = _query.Find<TransferOrderDto>(sql, new { Id = id });
            if (model == null)
            {
                throw new Exception("单据不存在");
            }
            string sqlItem = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, i.SupplierId,i.ContractPrice,i.Price,i.Quantity,i.BatchNo,s.Quantity as InventoryQuantity 
from transferorderitem i left join  product p on p.Id = i.ProductId 
left join storeinventory s on s.productid = i.productid and s.storeId =@FromStoreId  
where i.TransferOrderId=@TransferOrderId";
            var items = _query.FindAll<TransaferOrderItemDto>(sqlItem, new { TransferOrderId = model.Id, FromStoreId = model.FromStoreId }).ToList();
            model.Items = items;
            foreach (var product in model.Items)
            {
                product.SetSpecificationQuantity();
            }
            return model;

        }
    }
}
