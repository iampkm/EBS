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
   public class TransferOrderQueryService:ITransferOrderQuery
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
                where += "and t0.Code=@Code ";
                param.Code = condition.Code;
            }            
            if (condition.Status != 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }                
            string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.CreatedOn,t0.CreatedByName,t0.Status,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,t3.Quantity,t3.ActualQuantity,t3.Amount  
from storepurchaseorder t0 inner join supplier t1 on t0.SupplierId = t1.Id inner join store t2 on t0.StoreId = t2.Id
left join (select i.StorePurchaseOrderId,SUM(i.Quantity) as Quantity,SUM(i.ActualQuantity) as ActualQuantity,SUM(i.Price* i.ActualQuantity ) as Amount 
from  storepurchaseorderitem i GROUP BY i.StorePurchaseOrderId) t3 on t0.Id = t3.StorePurchaseOrderId 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<TransferOrderDto>(sql, param);
           // page.Total = this._query.Count<TransferOrder>(where, param);

            return rows;
        }


        public List<TransaferOrderItemDto> QueryProductBatch(string productCodeOrBarCode, int storeId)
        {
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 b.ContractPrice,b.Price,b.SupplierId,b.BatchNo ,s.`Name` as SupplierName,b.Quantity AS BatchQuantity,i.Quantity as InventoryQuantity
from storeinventorybatch b left join  product p on p.Id = b.ProductId
left join storeinventory i on i.ProductId = b.ProductId
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
 b.ContractPrice,b.Price,b.SupplierId,b.BatchNo ,s.`Name` as SupplierName,b.Quantity AS BatchQuantity,i.Quantity as InventoryQuantity
from storeinventorybatch b left join  product p on p.Id = b.ProductId
left join storeinventory i on i.ProductId = b.ProductId
left join supplier s on b.SupplierId = s.Id
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
    }
}
