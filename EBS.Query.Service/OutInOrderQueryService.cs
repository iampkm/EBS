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
                // param.Status = condition.Status;
            }
            string pwhere = "";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {                
                pwhere = string.Format("left join product p on p.Id = i.productid  where p.Code='{0}' or p.BarCode='{0}' ", condition.ProductCodeOrBarCode);
            }
            if (condition.OutInOrderTypeId != 0)
            {
                where += " and t0.OutInOrderTypeId=@OutInOrderTypeId ";
                param.OutInOrderTypeId = condition.OutInOrderTypeId;
            }
            if (condition.OutInInventory != 0)
            {
                where += " and t.OutInInventory=@OutInInventory ";
                param.OutInInventory = condition.OutInInventory;
            }

            if (condition.StartDate.HasValue)
            {
                where += "and t0.UpdatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and t0.UpdatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }
            
            if (!string.IsNullOrEmpty(condition.AuditName))
            {
                where += "and h.CreatedByName  like @AuditName ";
                param.AuditName = string.Format("%{0}%", condition.AuditName);
            }
            string formType = condition.OutInInventory == 1 ? "OtherInOrder" : "OtherOutOrder";
            string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.CreatedOn,t0.CreatedByName,t0.Status,t0.remark,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,t3.Quantity,t3.Amount,t0.UpdatedOn,h.CreatedByName as AuditName,t.TypeName   
from  (select i.OutInOrderId,SUM(i.Quantity) as Quantity,SUM(i.CostPrice* i.Quantity ) as Amount  
from  OutInOrderitem i {3} GROUP BY i.OutInOrderId) t3 left join 
 OutInOrder t0 on t0.Id = t3.OutInOrderId left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id 
left join processhistory h on  t0.Id = h.formId and FormType='{4}' and h.`Status` =4  
left join OutInOrderType t on t.Id = t0.OutInOrderTypeId 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";

            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize, pwhere, formType);
            var rows = this._query.FindAll<OutInOrderDto>(sql, param);

            // 统计列
            string sqlSum = @"select count(*) as TotalCount, sum(t3.Quantity) as Quantity ,sum(t3.Amount) as Amount  
from  (select i.OutInOrderId,SUM(i.Quantity) as Quantity,SUM(i.CostPrice* i.Quantity ) as Amount 
from  OutInOrderitem i {1} GROUP BY i.OutInOrderId) t3 left join 
 OutInOrder t0 on t0.Id = t3.OutInOrderId left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id 
left join processhistory h on  t0.Id = h.formId and FormType='{2}' and h.`Status` =4 
left join OutInOrderType t on t.Id = t0.OutInOrderTypeId 
where 1=1 {0} ";
            sqlSum = string.Format(sqlSum, where, pwhere, formType);
            var sumStoreInventory = this._query.Find<SumOutInOrderSummary>(sqlSum, param) as SumOutInOrderSummary;
            page.Total = sumStoreInventory.TotalCount;
            page.SumColumns.Add(new SumColumn("Quantity", sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
            return rows;
        }

        public OutInOrderItemDto QueryProduct(string productCodeOrBarCode, int storeId, int supplierId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new FriendlyException("请输入商品编码或条码"); }
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,i.ContractPrice as LastCostPrice, 
 i.ContractPrice as CostPrice,c.SupplierId,s.`Name` as SupplierName
 from purchasecontract c inner join purchasecontractitem i on c.Id= i.PurchaseContractId
inner join product p on p.Id = i.ProductId
left join supplier s on c.SupplierId = s.Id
where (p.`Code`=@productCodeOrBarCode or p.BarCode=@productCodeOrBarCode) and c.EndDate>@Today and c.`Status` = 3 and c.SupplierId=@SupplierId 
and FIND_IN_SET(@StoreId,c.StoreIds) order by c.Id desc  LIMIT 1";
            var item = _query.Find<OutInOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId, SupplierId = supplierId, Today = DateTime.Now });
            //设置当前件规            
            if (item == null) { throw new FriendlyException("查无商品，请检查供应商合同"); }
           
            return item;
        }

        public IEnumerable<OutInOrderItemDto> QueryProductList(string inputBarCodes, int storeId, int supplierId)
        {
            if (string.IsNullOrEmpty(inputBarCodes)) throw new FriendlyException("请粘贴导入的商品条码");
            var dic = inputBarCodes.ToIntDic();
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,i.ContractPrice as LastCostPrice, 
 i.ContractPrice as CostPrice,c.SupplierId,s.`Name` as SupplierName
 from purchasecontract c inner join purchasecontractitem i on c.Id= i.PurchaseContractId
inner join product p on p.Id = i.ProductId
left join supplier s on c.SupplierId = s.Id
where p.BarCode in @ProductCode  and c.EndDate>@Today and c.`Status` = 3 and c.SupplierId=@SupplierId
and FIND_IN_SET(@StoreId,c.StoreIds) order by c.Id desc ";
            var productItems = _query.FindAll<OutInOrderItemDto>(sql, new { ProductCode = dic.Keys.ToArray(), StoreId = storeId, SupplierId = supplierId, Today = DateTime.Now });
            if (!productItems.Any()) { throw new FriendlyException("查无商品，请检查供应商合同"); }
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.BarCode))
                {
                    product.Quantity = dic[product.BarCode];
                }              
            }
            return productItems;
        }

        public OutInOrderItemDto QueryRefundProduct(string productCodeOrBarCode, int storeId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new FriendlyException("请输入商品编码或条码"); }

            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,
i.LastCostPrice ,i.LastCostPrice as CostPrice,i.Quantity as inventoryQuantity
from storeinventory i 
left join product p on p.id = i.ProductId 
where (p.`Code`=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode) and i.StoreId=@StoreId  LIMIT 1";

            var item = _query.Find<OutInOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });
            //设置当前件规            
            if (item == null) { throw new FriendlyException("库存无此商品"); }

            return item;
        }

        public IEnumerable<OutInOrderItemDto> QueryRefundProductList(string inputBarCodes, int storeId)
        {
            if (string.IsNullOrEmpty(inputBarCodes)) throw new FriendlyException("请粘贴导入的商品条码");
            var dic = inputBarCodes.ToIntDic();
            // 有调整价，有先使用最新的调整价；无才使用合同价
            string sql = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit,
i.LastCostPrice ,i.LastCostPrice as CostPrice,i.Quantity as inventoryQuantity
from storeinventory i 
left join product p on p.id = i.ProductId 
where p.BarCode in @ProductCode and i.StoreId=@StoreId ";
            var productItems = _query.FindAll<OutInOrderItemDto>(sql, new { ProductCode = dic.Keys.ToArray(), StoreId = storeId});
            if (!productItems.Any()) { throw new FriendlyException("库存无此商品"); }
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
            string sql = @"select o.*,t.TypeName,s.`Name` as StoreName,l.`Name` as SupplierName,t.OutInInventory from OutInOrder o left join outinordertype t on o.OutInOrderTypeId= t.id 
left join store s on o.StoreId = s.Id 
left join supplier l on l.Id = o.SupplierId where o.Id=@Id";
            var model = _query.Find<OutInOrderDto>(sql, new { Id = id });
            if (model == null)
            {
                throw new FriendlyException("单据不存在");
            }
            string sqlItem = @"select p.Id as ProductId,p.`Name` as ProductName,p.`Code` as ProductCode,p.Specification,p.BarCode,p.Unit, i.LastCostPrice,i.CostPrice,i.Quantity,s.Quantity as InventoryQuantity 
from OutInOrderitem i left join  product p on p.Id = i.ProductId 
left join storeinventory s on s.productid = i.productid and s.storeId =@StoreId  
where i.OutInOrderId=@OutInOrderId";
            var items = _query.FindAll<OutInOrderItemDto>(sqlItem, new { OutInOrderId = model.Id, StoreId = model.StoreId }).ToList();
            model.Items = items;
            model.Quantity = model.Items.Sum(n => n.Quantity);
            model.Amount = model.Items.Sum(n => n.Amount);
            return model;

        }





        public IEnumerable<OutInOrderListDto> GetFinishList(Pager page, SearchOutInOrder condition)
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
            if (!string.IsNullOrEmpty(condition.Status) && condition.Status != "0")
            {
                where += "and t0.Status in (" + condition.Status + ") ";
                // param.Status = condition.Status;
            }
          
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where = string.Format("and (p.Code=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode) ", condition.ProductCodeOrBarCode);
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.ProductName))
            {
                where = string.Format("and p.Name like @ProductName ", condition.ProductName);
                param.ProductName = string.Format("%{0}%", condition.ProductName);
            }
            if (condition.OutInOrderTypeId != 0)
            {
                where += " and t0.OutInOrderTypeId=@OutInOrderTypeId ";
                param.OutInOrderTypeId = condition.OutInOrderTypeId;
            }
            if (condition.OutInInventory != 0)
            {
                where += " and t.OutInInventory=@OutInInventory ";
                param.OutInInventory = condition.OutInInventory;
            }

            if (condition.StartDate.HasValue)
            {
                where += "and t0.UpdatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and t0.UpdatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }

            if (!string.IsNullOrEmpty(condition.AuditName))
            {
                where += "and h.CreatedByName  like @AuditName ";
                param.AuditName = string.Format("%{0}%", condition.AuditName);
            }
            string formType = condition.OutInInventory == 1 ? "OtherInOrder" : "OtherOutOrder";

            string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.CreatedOn,t0.CreatedByName,t0.Status,t0.remark,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,
t3.Quantity,t3.CostPrice,t3.LastCostPrice,t0.UpdatedOn,h.CreatedByName as AuditName,t.TypeName,p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit   
from  OutInOrder t0 
inner join OutInOrderitem t3 on t0.Id = t3.OutInOrderId
left join product p on p.id = t3.ProductId
left join OutInOrderType t on t.Id = t0.OutInOrderTypeId 
left join supplier t1 on t0.SupplierId = t1.Id 
left join store t2 on t0.StoreId = t2.Id 
left join processhistory h on t0.Id = h.formId and FormType='{3}' and h.`Status` =4 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";

            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize, formType);
            var rows = this._query.FindAll<OutInOrderListDto>(sql, param);

            // 统计列
            string sqlSum = @"select count(*) as TotalCount, sum(t3.Quantity) as Quantity ,sum(t3.Quantity*t3.CostPrice) as Amount  
from OutInOrder t0 
inner join OutInOrderitem t3 on t0.Id = t3.OutInOrderId
left join product p on p.id = t3.ProductId
left join OutInOrderType t on t.Id = t0.OutInOrderTypeId 
left join supplier t1 on t0.SupplierId = t1.Id 
left join store t2 on t0.StoreId = t2.Id 
left join processhistory h on t0.Id = h.formId and FormType='{1}' and h.`Status` =4 
where 1=1 {0} ";
            sqlSum = string.Format(sqlSum, where, formType);
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
                where += "and h.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); ;
            }            
           
            if (condition.OutInOrderTypeId != 0)
            {
                where += " and t.Id=@OutInOrderTypeId ";
                param.OutInOrderTypeId = condition.OutInOrderTypeId;
            }
            if (condition.OutInInventory != 0)
            {
                where += " and t.OutInInventory=@OutInInventory ";
                param.OutInInventory = condition.OutInInventory;
            }

            if (condition.StartDate.HasValue)
            {
                where += "and h.CreatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and h.CreatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }

            string sql = @"select s.name as StoreName,b.typeName,a.Quantity,a.Amount from (
select h.StoreId,t.Id outinordertypeId,sum(h.ChangeQuantity) as Quantity,sum( h.ChangeQuantity*h.Price) as amount from storeinventoryhistory h 
inner join outinorder o on o.code = h.BillCode 
inner join outinordertype t on t.Id = o.OutInOrderTypeId
where h.BillType in (61,62) {0}
GROUP BY h.StoreId,t.Id
) a 
left join store s on a.storeid = s.id
left join outinordertype b on b.id = a.outinordertypeId  LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<OutInOrderSummaryDto>(sql, param);
            string sqlSum = @"
select count(*) as TotalCount,sum(Quantity) as Quantity,sum( amount) as amount 
from (
select h.StoreId,t.Id outinordertypeId,count(*) as TotalCount,sum(h.ChangeQuantity) as Quantity,sum( h.ChangeQuantity*h.Price) as amount from storeinventoryhistory h 
inner join outinorder o on o.code = h.BillCode 
inner join outinordertype t on t.Id = o.OutInOrderTypeId
where h.BillType in (61,62) {0} 
GROUP BY h.StoreId,t.Id
) a ";

            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory = this._query.Find<SumOutInOrderSummary>(sqlSum, param) as SumOutInOrderSummary;
            page.Total = sumStoreInventory.TotalCount;

            page.SumColumns.Add(new SumColumn("Quantity", sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
           
            return rows;
        }


        private IDictionary<int, string> GetOtherOutInOrderTypes(OutInInventoryType outInInventory)
        {
            var rows = _query.FindAll<OutInOrderType>(n =>n.OutInInventory == outInInventory);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            rows.ToList().ForEach(n => dic.Add(n.Id, n.TypeName));
            return dic;
        }

        public IDictionary<int, string> GetOtherInOrderTypes()
        {
            return GetOtherOutInOrderTypes(OutInInventoryType.In);
        }

        public IDictionary<int, string> GetOtherOutOrderTypes()
        {
            return GetOtherOutInOrderTypes(OutInInventoryType.Out);
        }

       
    }
}
