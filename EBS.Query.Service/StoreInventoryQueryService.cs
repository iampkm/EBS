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
   public class StoreInventoryQueryService:IStoreInventoryQuery
    {
        IQuery _query;
        public StoreInventoryQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<DTO.StoreInventoryQueryDto> GetPageList(DTO.Pager page, DTO.SearchStoreInventory condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";           
            
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); ;
            }
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t1.Code=@ProductCodeOrBarCode or t1.BarCode=@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }

            if (!string.IsNullOrEmpty(condition.ProductName))
            {
                where += "and t1.Name like @ProductName ";
                param.ProductName = string.Format("%{0}%", condition.ProductName); 
            }
            if (!string.IsNullOrEmpty(condition.CategoryId))
            {
                where += "and t3.Id like @CategoryId ";
                param.CategoryId = string.Format("{0}%", condition.CategoryId);
            }
            if (!string.IsNullOrEmpty(condition.Operate))
            {
                where +=string.Format("and t0.Quantity {0} @Quantity ",condition.Operate);
                param.Quantity = condition.Quantity; 
            }            

            string sql = @"select t0.*,t1.`Code` as ProductCode ,t1.`Name` as ProductName,t1.BarCode,t1.Specification,t1.SalePrice,t2.`name` as StoreName,t3.FullName as CategoryName 
from storeinventory t0 left join product t1 on t0.productId = t1.Id
left join store t2 on t2.Id = t0.StoreId
left join category t3 on t1.CategoryId = t3.Id
where 1=1 {0} ORDER BY t0.Id desc ";

            if (!page.toExcel)
            {
              //  sql += string.Format(" LIMIT {0},{1}", (page.PageIndex - 1) * page.PageSize, page.PageSize);
                sql += string.Format(" LIMIT {0},{1}", 0, 100);
            }

            sql = string.Format(sql, where);
            var rows = this._query.FindAll<StoreInventoryQueryDto>(sql, param);

            // 查询统计列数据
            string sqlSum = @"select count(*) as TotalCount, sum(t0.Quantity) as Quantity,sum(t0.LastCostPrice*t0.Quantity) as Amount,sum(t1.SalePrice*t0.Quantity) as SaleAmount
from storeinventory t0 left join product t1 on t0.productId = t1.Id
left join store t2 on t2.Id = t0.StoreId
left join category t3 on t1.CategoryId = t3.Id
where 1=1 {0}";
            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory= this._query.Find<SumStoreInventory>(sqlSum, param) as SumStoreInventory;
            page.Total = sumStoreInventory.TotalCount;
            page.SumColumns.Add(new SumColumn("Quantity",sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("SaleAmount", sumStoreInventory.SaleAmount.ToString("F2")));
            return rows;
        }

        public IEnumerable<StoreInventoryHistoryQueryDto> GetPageList(Pager page, SearchStoreInventoryHistory condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId!="0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); ;
            }
            if (!string.IsNullOrEmpty(condition.BillCode))
            {
                where += "and t0.BillCode=@BillCode ";
                param.BillCode = condition.BillCode;
            }
            if (!string.IsNullOrEmpty(condition.BatchNo))
            {
                where += "and t0.BatchNo=@BatchNo ";
                param.BatchNo = condition.BatchNo;
            }
            if (!string.IsNullOrEmpty(condition.BillType))
            {
                where += "and t0.BillType in @BillType ";
                param.BillType = condition.BillType.Split(',').ToIntArray();
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

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t1.Code=@ProductCodeOrBarCode or t1.BarCode=@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            string sql = @"select t0.CreatedOn,t2.`name` as StoreName,t1.`Code` as ProductCode ,t1.`Name` as ProductName,t1.BarCode,t1.Specification,t0.BillType,t0.BillCode,t0.BatchNo, t0.Quantity,t0.Price,
IFNULL(case when t0.ChangeQuantity<0 then t0.ChangeQuantity end ,0) as OutQuantity,
IFNULL(case when t0.ChangeQuantity<0 then t0.ChangeQuantity*t0.Price end ,0) as OutAmount,
IFNULL(case when t0.ChangeQuantity>=0 then t0.ChangeQuantity  end,0) as InQuantity,
IFNULL(case when t0.ChangeQuantity>=0 then t0.ChangeQuantity*t0.Price end ,0) as InAmount
from storeinventoryhistory t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";          
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreInventoryHistoryQueryDto>(sql, param);

            string sqlCount = @"select count(*) as rowCount ,
sum(IFNULL(case when t0.ChangeQuantity<0 then t0.ChangeQuantity end ,0)) as OutQuantity,
sum(IFNULL(case when t0.ChangeQuantity<0 then t0.ChangeQuantity*t0.Price end ,0)) as OutAmount,
sum(IFNULL(case when t0.ChangeQuantity>=0 then t0.ChangeQuantity  end,0)) as InQuantity,
sum(IFNULL(case when t0.ChangeQuantity>=0 then t0.ChangeQuantity*t0.Price end ,0)) as InAmount
from storeinventoryhistory t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId  
where 1=1 {0} ";
            sqlCount = string.Format(sqlCount, where);
            var sumModel = this._query.Find<SumStoreInventoryHistory>(sqlCount, param) as SumStoreInventoryHistory;
            page.Total = sumModel.RowCount;
            page.SumColumns.Add(new SumColumn("InQuantity", sumModel.InQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("OutAmount", sumModel.OutAmount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("OutQuantity", sumModel.OutQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("InAmount", sumModel.InAmount.ToString("F4")));
            //page.SumColumns.Add(new SumColumn("CurrentQuantity", sumModel.CurrentQuantity.ToString()));
            return rows;
        }

        public IEnumerable<StoreInventoryBatchQueryDto> GetPageList(Pager page, SearchStoreInventoryBatch condition)
        {


            dynamic param = new ExpandoObject();
            string where = "";
            if (condition.SupplierId > 0)
            {
                where += "and t0.SupplierId=@SupplierId ";
                param.SupplierId = condition.SupplierId;
            }
            if (!string.IsNullOrEmpty(condition.BatchNo))
            {
                where += "and t0.BatchNo=@BatchNo ";
                param.BatchNo = condition.BatchNo;
            }            
            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and t0.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); ;
            }

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t1.Code=@ProductCodeOrBarCode or t1.BarCode=@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.CategoryId))
            {
                where += "and t4.Id like @CategoryId ";
                param.CategoryId = string.Format("{0}%", condition.CategoryId);
            }
            if (!string.IsNullOrEmpty(condition.Operate))
            {
                where += string.Format("and t0.Quantity {0} @Quantity ", condition.Operate);
                param.Quantity = condition.Quantity;
            }
            string sql = @"select t0.*,t1.`Code` as ProductCode ,t1.`Name` as ProductName,t1.BarCode,t1.Specification,t2.`name` as StoreName,t3.`Name` as SupplierName,t4.FullName as CategoryName 
from storeinventorybatch t0 left join product t1 on t0.productId = t1.Id 
left join store t2 on t2.Id = t0.StoreId 
left join supplier t3 on t3.Id = t0.SupplierId  
left join category t4 on t4.Id = t1.CategoryId  
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreInventoryBatchQueryDto>(sql, param);
            // page.Total = this._query.Count<StoreInventory>(where, param);
            page.Total = this._query.Count<StoreInventoryBatch>();

            return rows;
        }

        public IEnumerable<ProductQueryDto> QueryProduct(SearchStoreInventory condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode )";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.ProductName))
            {
                where += "and p.Name like @ProductName ";
                param.ProductName = string.Format("%{0}%", condition.ProductName);
            }
            //if (condition.StoreId > 0)
            //{
            //    where += "and t.Id=@StoreId ";
            //    param.StoreId = condition.StoreId;
            //}
            string sql = @"select b.ProductId, p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit,p.SalePrice,
b.Quantity as BatchQuantity,s.Name as supplierName,t.`Name` as StoreName,b.Price,sp.StoreSalePrice,v.SalePrice as VipSalePrice
from ( select i.storeid,i.supplierId,i.productId,i.Price,sum(i.quantity) as Quantity from storeinventorybatch i group by  i.storeid,i.supplierId,i.productId,i.Price ) b 
left join product p on b.ProductId = p.Id
left join supplier s on s.Id = b.SupplierId
left join store t on t.Id = b.StoreId
left join storeinventory sp on sp.ProductId = p.Id and sp.StoreId = b.storeid
left join vipproduct v on v.ProductId = p.Id
where 1=1 and b.Quantity>0 {0} order by b.StoreId";  //b.Quantity>0
            if (string.IsNullOrEmpty(where)) return new List<ProductQueryDto>();
            sql = string.Format(sql, where);
            var rows = _query.FindAll<ProductQueryDto>(sql, param);
            //设置当前件规
            return rows;
        }
    }
}
