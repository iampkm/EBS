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
    public class StorePurchaseOrderQueryService : IStorePurchaseOrderQuery
    {
        IQuery _query;
        public StorePurchaseOrderQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<DTO.StorePurchaseOrderQueryDto> GetPageList(DTO.Pager page, DTO.SearchStorePurchaseOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and t0.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.SupplierId > 0)
            {
                where += "and t0.SupplierId=@SupplierId ";
                param.SupplierId = condition.SupplierId;
            }
            if (condition.StoreId > 0)
            {
                where += "and t0.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            if (condition.Status != 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and t3.ProductId in (select Id from Product where Code=@ProductCodeOrBarCode or BarCode=@ProductCodeOrBarCode) ";
                param.Code = condition.Code;
            }
            if (condition.OrderType > 0)
            {
                where += " and t0.OrderType=@OrderType";
                param.OrderType = condition.OrderType;
            }          
            string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.CreatedOn,t0.CreatedByName,t0.Status,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,t3.Quantity,t3.ActualQuantity,t3.Amount  
from storepurchaseorder t0 inner join supplier t1 on t0.SupplierId = t1.Id inner join store t2 on t0.StoreId = t2.Id
left join (select i.StorePurchaseOrderId,SUM(i.Quantity) as Quantity,SUM(i.ActualQuantity) as ActualQuantity,SUM(i.Price* i.ActualQuantity ) as Amount 
from  storepurchaseorderitem i GROUP BY i.StorePurchaseOrderId) t3 on t0.Id = t3.StorePurchaseOrderId 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StorePurchaseOrderQueryDto>(sql, param);
            page.Total = this._query.Count<StorePurchaseOrder>(where, param);

            return rows;
        }


        public Dictionary<int, string> GetStorePurchaseOrderStatus()
        {
            var dic = typeof(PurchaseOrderStatus).GetValueToDescription();
            return dic;
        }


        public StorePurchaseOrderItemDto GetPurchaseOrderItem(string productCodeOrBarCode, int storeId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,c.SupplierId,s.`Name` as SupplierName
 from purchasecontract c inner join purchasecontractitem i on c.Id= i.PurchaseContractId
inner join product p on p.Id = i.ProductId
left join supplier s on c.SupplierId = s.Id
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode) and c.EndDate>@Today and c.`Status` = 3
and FIND_IN_SET(@StoreId,c.StoreIds)  LIMIT 1";            
            var item = _query.Find<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId, Today = DateTime.Now });
            //设置当前件规            
            if (item == null) { throw new Exception("查无商品，请检查供应商合同"); }
            // 查询是否有调整价格

            item.SetSpecificationQuantity();
            return item;

        }
        public IEnumerable<StorePurchaseOrderItemDto> GetPurchaseOrderItemList(string inputProducts, int storeId)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            var dic = inputProducts.ToIntDic();
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,c.SupplierId,s.`Name` as SupplierName
 from purchasecontract c inner join purchasecontractitem i on c.Id= i.PurchaseContractId
inner join product p on p.Id = i.ProductId
left join supplier s on c.SupplierId = s.Id
where p.`Code` in @ProductCode  and c.EndDate>@Today and c.`Status` = 3
and FIND_IN_SET(@StoreId,c.StoreIds)";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sql, new { ProductCode = dic.Keys.ToArray(), StoreId = storeId, Today = DateTime.Now });
            if (!productItems.Any()) { throw new Exception("查无商品，请检查供应商合同"); }
            foreach (var product in productItems)
            {
                product.Quantity = dic[product.ProductCode];
                product.SetSpecificationQuantity();
            }
            return productItems;
        }
       
        public StorePurchaseOrderDto GetById(int id)
        {
           string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.StoreId,t0.CreatedOn,t0.CreatedByName,t0.ReceivedOn,t0.ReceivedByName,t0.StoragedOn,t0.StoragedByName,t0.OrderType,
t0.IsGift,t0.Status,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,t0.SupplierBill
from storepurchaseorder t0 inner
join supplier t1 on t0.SupplierId = t1.Id inner
join store t2 on t0.StoreId = t2.Id
where t0.Id= @Id  LIMIT 1";
            var model = _query.Find<StorePurchaseOrderDto>(sql, new { Id = id});
            string sqlitem = @"select i.*,p.Name as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity 
 from storepurchaseorderitem i left join product p on i.productId = p.Id
where i.storepurchaseorderid= @Id";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sqlitem, new { Id = id }).ToList();
            foreach (var product in productItems)
            {
                product.SetSpecificationQuantity();
            }
            model.Items = productItems;
            
            return model;
        }


        //退单按照先进先出原则从库存查询
        public StorePurchaseOrderItemDto GetRefundOrderItem(string productCodeOrBarCode, int storeId,long batchNo = 0)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,i.Price,i.SupplierId,s.`Name` as SupplierName,i.ProductionDate,i.ShelfLife,i.BatchNo,si.Quantity as inventoryQuantity
from storeinventorybatch i inner join product p on p.Id = i.ProductId
left join supplier s on s.Id = i.SupplierId
left join storeinventory si on si.ProductId = p.Id
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode) and i.Quantity>0 and i.StoreId=@StoreId {0} LIMIT 1";
            string whereBatch = "";
            if (batchNo>0)
            {
                whereBatch = string.Format(" and i.BatchNo={0} ", batchNo);
            }
            sql = string.Format(sql, whereBatch);
            var item = _query.Find<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });
            //设置当前件规            
            if (item == null) { throw new Exception("查无商品，请检查供应商合同"); }
            // 查询是否有调整价格

            item.SetSpecificationQuantity();
            return item;
        }

        public IEnumerable<StorePurchaseOrderItemDto> GetRefundOrderItemList(string inputProducts, int storeId)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            var dic = inputProducts.ToIntDic();
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,i.Price,i.SupplierId,s.`Name` as SupplierName,i.ProductionDate,i.ShelfLife,i.BatchNo
from storeinventorybatch i inner join product p on p.Id = i.ProductId
left join supplier s on s.Id = i.SupplierId
where p.`Code` in @ProductCode   and i.Quantity>0 and StoreId=@StoreId ";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sql, new { ProductCode = dic.Keys.ToArray(), StoreId = storeId});
            if (!productItems.Any()) { throw new Exception("查无商品，请检查供应商合同"); }
            foreach (var product in productItems)
            {
                product.Quantity = dic[product.ProductCode];
                product.SetSpecificationQuantity();
            }
            return productItems;
        }



        public IEnumerable<StorePurchaseOrderItemDto> GetProductBatchs(string productCodeOrBarCode, int storeId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)||storeId==0) {
                return new List<StorePurchaseOrderItemDto>();   //查批次必须输入条件，有一个条件为空都返回空记录
            }
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,i.Price,i.SupplierId,s.`Name` as SupplierName,i.ProductionDate,i.ShelfLife,i.BatchNo,i.Quantity 
from storeinventorybatch i inner join product p on p.Id = i.ProductId
left join supplier s on s.Id = i.SupplierId
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode) and i.Quantity>0 and i.StoreId=@StoreId ";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });
            //设置当前件规            

            //foreach (var product in productItems)
            //{
            //    product.SetSpecificationQuantity();
            //}
            return productItems;
        }
    }
}
