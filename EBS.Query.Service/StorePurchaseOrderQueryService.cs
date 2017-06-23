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
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); ;
            }
            if (!string.IsNullOrEmpty(condition.Status))
            {
                where += "and t0.Status in ("+condition.Status+") ";
               // param.Status = condition.Status;
            }
            string pwhere="";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                //where += "and t3.ProductId in (select Id from Product where Code=@ProductCodeOrBarCode or BarCode=@ProductCodeOrBarCode) ";
                //param.Code = condition.Code;
                pwhere = string.Format("left join product p on p.Id = i.productid  where p.Code='{0}' or p.BarCode='{0}' ", condition.ProductCodeOrBarCode);
            }
            if (condition.OrderType > 0)
            {
                where += " and t0.OrderType=@OrderType ";
                param.OrderType = condition.OrderType;
            }

            if (condition.StartDate.HasValue)
            {
                where += "and t0.CreatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and t0.CreatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }
            if (condition.StoragedBegin.HasValue)
            {
                where += "and t0.StoragedOn >=@StoragedBegin ";
                param.StoragedBegin = condition.StoragedBegin.Value;
            }
            if (condition.StoragedEnd.HasValue)
            {
                where += "and t0.StoragedOn < @StoragedEnd ";
                param.StoragedEnd = condition.StoragedEnd.Value.AddDays(1);
            }

            string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.CreatedOn,t0.CreatedByName,t0.Status,t0.SupplierBill,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,t3.Quantity,t3.ActualQuantity,t3.Amount,t0.StoragedOn  
from  (select i.StorePurchaseOrderId,SUM(i.Quantity) as Quantity,SUM(i.ActualQuantity) as ActualQuantity,SUM(i.Price* i.ActualQuantity ) as Amount 
from  storepurchaseorderitem i {3} GROUP BY i.StorePurchaseOrderId) t3 left join 
 storepurchaseorder t0 on t0.Id = t3.StorePurchaseOrderId left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize,pwhere);
            var rows = this._query.FindAll<StorePurchaseOrderQueryDto>(sql, param);
            string sqlCount = @"select count(*) from  (select i.StorePurchaseOrderId,SUM(i.Quantity) as Quantity,SUM(i.ActualQuantity) as ActualQuantity,SUM(i.Price* i.ActualQuantity ) as Amount from  storepurchaseorderitem i {1} GROUP BY i.StorePurchaseOrderId) t3 left join 
 storepurchaseorder t0 on t0.Id = t3.StorePurchaseOrderId left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id where 1=1 {0} ";
            sqlCount = string.Format(sqlCount, where, pwhere);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
            // 统计列
            string sqlSum = @"select sum(t3.Quantity) as Quantity ,sum(t3.ActualQuantity) as ActualQuantity ,sum(t3.Amount) as Amount  
from  (select i.StorePurchaseOrderId,SUM(i.Quantity) as Quantity,SUM(i.ActualQuantity) as ActualQuantity,SUM(i.Price* i.ActualQuantity ) as Amount 
from  storepurchaseorderitem i {1} GROUP BY i.StorePurchaseOrderId) t3 left join 
 storepurchaseorder t0 on t0.Id = t3.StorePurchaseOrderId left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id where 1=1 {0} ";
            sqlSum = string.Format(sqlSum, where, pwhere);
            var sumStoreInventory = this._query.Find<SumStorePurchaseOrder>(sqlSum, param) as SumStorePurchaseOrder;
            page.SumColumns.Add(new SumColumn("Quantity", sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("ActualQuantity", sumStoreInventory.ActualQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
            return rows;
        }


        public Dictionary<int, string> GetStorePurchaseOrderStatus()
        {
            var dic = typeof(PurchaseOrderStatus).GetValueToDescription();
            return dic;
        }


        public StorePurchaseOrderItemDto GetPurchaseOrderItem(string productCodeOrBarCode, int storeId, int supplierId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,c.SupplierId,s.`Name` as SupplierName
 from purchasecontract c inner join purchasecontractitem i on c.Id= i.PurchaseContractId
inner join product p on p.Id = i.ProductId
left join supplier s on c.SupplierId = s.Id
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode) and c.EndDate>@Today and c.`Status` = 3 and c.SupplierId=@SupplierId 
and FIND_IN_SET(@StoreId,c.StoreIds) order by c.Id desc  LIMIT 1";
            //var supplierWhere = "";
            //if (supplierId > 0)            {
            //    supplierWhere = "and c.supplierId="+supplierId;               
            //}
            //sql = string.Format(sql, supplierWhere);
            var item = _query.Find<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId,SupplierId=supplierId, Today = DateTime.Now });
            //设置当前件规            
            if (item == null) { throw new Exception("查无商品，请检查供应商合同"); }
            // 查询是否有调整价格

            item.SetSpecificationQuantity();
            return item;

        }
        public IEnumerable<StorePurchaseOrderItemDto> GetPurchaseOrderItemList(string inputProducts, int storeId, int supplierId)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            var dic = inputProducts.ToIntDic();
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,c.SupplierId,s.`Name` as SupplierName
 from purchasecontract c inner join purchasecontractitem i on c.Id= i.PurchaseContractId
inner join product p on p.Id = i.ProductId
left join supplier s on c.SupplierId = s.Id
where p.BarCode in @ProductCode  and c.EndDate>@Today and c.`Status` = 3 and c.SupplierId=@SupplierId
and FIND_IN_SET(@StoreId,c.StoreIds) order by c.Id desc ";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sql, new { ProductCode = dic.Keys.ToArray(), StoreId = storeId, SupplierId = supplierId, Today = DateTime.Now });
            if (!productItems.Any()) { throw new Exception("查无商品，请检查供应商合同"); }
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
        public StorePurchaseOrderItemDto GetRefundOrderItem(string productCodeOrBarCode, int storeId,int supplierId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }

            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity,  
i.LastCostPrice as ContractPrice,i.LastCostPrice as Price,i.Quantity as inventoryQuantity
from storeinventory i 
left join product p on p.id = i.ProductId
left join 
(
select b.StoreId,b.SupplierId,b.ProductId,sum(b.Quantity)
 from storeinventorybatch b group by b.StoreId,b.SupplierId,b.ProductId) t
on t.StoreId = i.StoreId and t.ProductId = i.ProductId
where (p.`Code`=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode) and i.StoreId=@StoreId and t.SupplierId=@SupplierId LIMIT 1";

            var item = _query.Find<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId, SupplierId = supplierId });
            //设置当前件规            
            if (item == null) { throw new Exception("查无商品，请检查供应商合同"); }           
            item.SetSpecificationQuantity();
            return item;
        }

        public IEnumerable<StorePurchaseOrderItemDto> GetRefundOrderItemList(string inputProducts, int storeId, int supplierId)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            var dic = inputProducts.ToIntDic();
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity,  
i.LastCostPrice as ContractPrice,i.LastCostPrice as Price,i.Quantity as inventoryQuantity
from storeinventory i 
left join product p on p.id = i.ProductId
left join 
(
select b.StoreId,b.SupplierId,b.ProductId,sum(b.Quantity)
 from storeinventorybatch b group by b.StoreId,b.SupplierId,b.ProductId) t
on t.StoreId = i.StoreId and t.ProductId = i.ProductId
where p.BarCode in @BarCode  and i.StoreId=@StoreId and t.SupplierId=@SupplierId ";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sql, new { BarCode = dic.Keys.ToArray(), StoreId = storeId, SupplierId = supplierId });
            if (!productItems.Any()) { throw new Exception("查无商品，请检查供应商合同"); }
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



        public IEnumerable<StorePurchaseOrderItemDto> GetProductBatchs(string productCodeOrBarCode, int storeId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)||storeId==0) {
                return new List<StorePurchaseOrderItemDto>();   //查批次必须输入条件，有一个条件为空都返回空记录
            }
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,p.SpecificationQuantity as ProductSpecificationQuantity, 
 i.ContractPrice,i.Price,i.SupplierId,s.`Name` as SupplierName,i.ProductionDate,i.ShelfLife,i.BatchNo,i.Quantity 
from storeinventorybatch i inner join product p on p.Id = i.ProductId
left join supplier s on s.Id = i.SupplierId
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode) and i.Quantity>0 and i.StoreId=@StoreId ";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });

            return productItems;
        }


        public IEnumerable<StorePurchaseOrderListDto> GetFinishList(Pager page, SearchStorePurchaseOrder condition)
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
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); ;
            }
            if (!string.IsNullOrEmpty(condition.Status))
            {
                where += "and t0.Status in (" + condition.Status + ") ";               
            }
           
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {               
                where += string.Format("and (p.Code=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode) ", condition.ProductCodeOrBarCode);
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (condition.OrderType > 0)
            {
                where += " and t0.OrderType=@OrderType ";
                param.OrderType = condition.OrderType;
            }

            if (condition.StartDate.HasValue)
            {
                where += "and t0.CreatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and t0.CreatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }
            if (condition.StoragedBegin.HasValue)
            {
                where += "and t0.StoragedOn >=@StoragedBegin ";
                param.StoragedBegin = condition.StoragedBegin.Value;
            }
            if (condition.StoragedEnd.HasValue)
            {
                where += "and t0.StoragedOn < @StoragedEnd ";
                param.StoragedEnd = condition.StoragedEnd.Value.AddDays(1);
            }
            if (!string.IsNullOrEmpty(condition.ProductName))
            {
                where += "and p.Name like @ProductName ";
                param.ProductName = string.Format("%{0}%", condition.ProductName);
            }
            string sql = @"select t0.Id,t0.Code,t0.CreatedOn,t0.CreatedByName,t0.Status,t0.SupplierBill,t0.IsGift,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,i.Quantity,i.ActualQuantity,t0.StoragedOn,i.Price,i.ContractPrice,
p.code as ProductCode,p.BarCode,p.`Name` as ProductName,p.Specification,i.Price* i.ActualQuantity as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId
left join supplier t1 on t0.SupplierId = t1.Id 
left join store t2 on t0.StoreId = t2.Id 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";

            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StorePurchaseOrderListDto>(sql, param);

            // 统计列
            string sqlSum = @"select count(*) as TotalCount, sum(i.Quantity) as Quantity ,sum(i.ActualQuantity) as ActualQuantity ,sum(i.Price* i.ActualQuantity) as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId
left join supplier t1 on t0.SupplierId = t1.Id 
left join store t2 on t0.StoreId = t2.Id 
where 1=1 {0} ";
            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory = this._query.Find<SumStorePurchaseOrder>(sqlSum, param) as SumStorePurchaseOrder;
            page.Total = sumStoreInventory.TotalCount;
            page.SumColumns.Add(new SumColumn("Quantity", sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("ActualQuantity", sumStoreInventory.ActualQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
            return rows;
        }


        public IEnumerable<StorePurchaseOrderSummaryDto> GetSummaryList(Pager page, SearchStorePurchaseOrder condition)
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
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); ;
            }
            if (!string.IsNullOrEmpty(condition.Status))
            {
                where += "and t0.Status in (" + condition.Status + ") ";
            }

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += string.Format("and (p.Code=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode) ", condition.ProductCodeOrBarCode);
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (condition.OrderType > 0)
            {
                where += " and t0.OrderType=@OrderType ";
                param.OrderType = condition.OrderType;
            }

            if (condition.StartDate.HasValue)
            {
                where += "and t0.CreatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and t0.CreatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }
            if (condition.StoragedBegin.HasValue)
            {
                where += "and t0.StoragedOn >=@StoragedBegin ";
                param.StoragedBegin = condition.StoragedBegin.Value;
            }
            if (condition.StoragedEnd.HasValue)
            {
                where += "and t0.StoragedOn < @StoragedEnd ";
                param.StoragedEnd = condition.StoragedEnd.Value.AddDays(1);
            }
            if (!string.IsNullOrEmpty(condition.ProductName))
            {
                where += "and p.Name like @ProductName ";
                param.ProductName = string.Format("%{0}%", condition.ProductName);
            }
            if (string.IsNullOrEmpty(condition.GroupBy))
            {
                throw new Exception("请选择分组方式");
            }
            string sql = "";
            string sqlSum = "";
            switch (condition.GroupBy)
            {
                case "Store":
                    sql = @"select t2.Name AS StoreName,t.* from (
select t0.StoreId,
sum(case when t0.OrderType=2 then  -i.Quantity
         when t0.OrderType=1 then i.Quantity end) as Quantity ,
sum(case when t0.OrderType=2 then  -i.ActualQuantity
         when t0.OrderType=1 then i.ActualQuantity end ) as ActualQuantity ,
sum(case when t0.OrderType=2 then  i.Price* -i.ActualQuantity
         when t0.OrderType=1 then i.Price* i.ActualQuantity end 
) as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId 
where 1=1 {0} 
GROUP BY t0.StoreId  ORDER BY t0.Id desc 
 ) t
LEFT JOIN store t2 on t2.Id = t.StoreId order by t2.Id LIMIT {1},{2} ";

                    // 统计列
                    sqlSum = @"select count(*) as TotalCount,sum(t.Quantity) as Quantity,sum(t.ActualQuantity) as ActualQuantity,sum(t.Amount) as Amount from (
select t0.StoreId,
sum(case when t0.OrderType=2 then  -i.Quantity
         when t0.OrderType=1 then i.Quantity end) as Quantity ,
sum(case when t0.OrderType=2 then  -i.ActualQuantity
         when t0.OrderType=1 then i.ActualQuantity end ) as ActualQuantity ,
sum(case when t0.OrderType=2 then  i.Price* -i.ActualQuantity
         when t0.OrderType=1 then i.Price* i.ActualQuantity end 
) as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId
where 1=1 {0} 
GROUP BY t0.StoreId 
 ) t
LEFT JOIN store t2 on t2.Id = t.StoreId ";
                    break;
                case "Supplier":
                    sql = @"select t2.Name AS SupplierName,t.* from (
select t0.SupplierId,
sum(case when t0.OrderType=2 then  -i.Quantity
         when t0.OrderType=1 then i.Quantity end) as Quantity ,
sum(case when t0.OrderType=2 then  -i.ActualQuantity
         when t0.OrderType=1 then i.ActualQuantity end ) as ActualQuantity ,
sum(case when t0.OrderType=2 then  i.Price* -i.ActualQuantity
         when t0.OrderType=1 then i.Price* i.ActualQuantity end 
) as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId 
where 1=1 {0} 
GROUP BY t0.SupplierId  ORDER BY t0.Id desc 
 ) t
LEFT JOIN Supplier t2 on t2.Id = t.SupplierId order by t2.Id LIMIT {1},{2} ";

                    // 统计列
                    sqlSum = @"select count(*) as TotalCount,sum(t.Quantity) as Quantity,sum(t.ActualQuantity) as ActualQuantity,sum(t.Amount) as Amount from (
select t0.SupplierId,
sum(case when t0.OrderType=2 then  -i.Quantity
         when t0.OrderType=1 then i.Quantity end) as Quantity ,
sum(case when t0.OrderType=2 then  -i.ActualQuantity
         when t0.OrderType=1 then i.ActualQuantity end ) as ActualQuantity ,
sum(case when t0.OrderType=2 then  i.Price* -i.ActualQuantity
         when t0.OrderType=1 then i.Price* i.ActualQuantity end 
) as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId
where 1=1 {0} 
GROUP BY t0.SupplierId 
 ) t
Left join Supplier t2 on t2.Id = t.SupplierId
";
                    break;
                case "StoreAndSupplier":
                    sql = @"select t2.Name as StoreName,t3.Name AS SupplierName,t.* from (
select t0.StoreId,t0.SupplierId,
sum(case when t0.OrderType=2 then  -i.Quantity
         when t0.OrderType=1 then i.Quantity end) as Quantity ,
sum(case when t0.OrderType=2 then  -i.ActualQuantity
         when t0.OrderType=1 then i.ActualQuantity end ) as ActualQuantity ,
sum(case when t0.OrderType=2 then  i.Price* -i.ActualQuantity
         when t0.OrderType=1 then i.Price* i.ActualQuantity end 
) as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId 
where 1=1 {0} 
GROUP BY t0.StoreId,t0.SupplierId  
 ) t
LEFT JOIN Store t2 on t2.Id = t.StoreId 
Left join Supplier t3 on t3.Id = t.SupplierId order by t2.Id,t3.Id LIMIT {1},{2} ";

                    // 统计列
                    sqlSum = @"select count(*) as TotalCount,sum(t.Quantity) as Quantity,sum(t.ActualQuantity) as ActualQuantity,sum(t.Amount) as Amount from (
select t0.StoreId,t0.SupplierId, 
sum(case when t0.OrderType=2 then  -i.Quantity
         when t0.OrderType=1 then i.Quantity end) as Quantity ,
sum(case when t0.OrderType=2 then  -i.ActualQuantity
         when t0.OrderType=1 then i.ActualQuantity end ) as ActualQuantity ,
sum(case when t0.OrderType=2 then  i.Price* -i.ActualQuantity
         when t0.OrderType=1 then i.Price* i.ActualQuantity end 
) as Amount  
from storepurchaseorder t0 
inner join storepurchaseorderitem i on t0.Id = i.storepurchaseOrderId
left join product p on p.Id = i.ProductId
where 1=1 {0} 
GROUP BY t0.StoreId,t0.SupplierId 
 ) t
LEFT JOIN store t2 on t2.Id = t.StoreId
Left join Supplier t3 on t3.Id = t.SupplierId ";
                    break;
            }

            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StorePurchaseOrderSummaryDto>(sql, param);

            
            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory = this._query.Find<SumStorePurchaseOrder>(sqlSum, param) as SumStorePurchaseOrder;
            page.Total = sumStoreInventory.TotalCount;
            page.SumColumns.Add(new SumColumn("Quantity", sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("ActualQuantity", sumStoreInventory.ActualQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
            return rows;
        }
    }
}
