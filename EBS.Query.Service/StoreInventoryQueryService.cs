﻿using System;
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
            //if (condition.SupplierId > 0)
            //{
            //    where += "and t3.SupplierId=@SupplierId ";
            //    param.SupplierId = condition.SupplierId;
            //}
            if (condition.StoreId > 0)
            {
                where += "and t0.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
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

            string sql = @"select t0.*,t1.`Code` as ProductCode ,t1.`Name` as ProductName,t1.BarCode,t1.Specification,t1.SalePrice,t2.`name` as StoreName
from storeinventory t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreInventoryQueryDto>(sql, param);
            string sqlCount = @"select count(*) from storeinventory t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId
where 1=1 {0} ";
            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);

            // 查询统计列数据
            string sqlSum = @"select sum(t0.Quantity) as Quantity,sum(t0.AvgCostPrice*t0.Quantity) as Amount,sum(t1.SalePrice*t0.Quantity) as SaleAmount
from storeinventory t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId
where 1=1 {0}";
            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory= this._query.Find<SumStoreInventory>(sqlSum, param) as SumStoreInventory;             
            page.SumColumns.Add(new SumColumn("Quantity",sumStoreInventory.Quantity.ToString()));
            page.SumColumns.Add(new SumColumn("Amount", sumStoreInventory.Amount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("SaleAmount", sumStoreInventory.SaleAmount.ToString("F2")));
            return rows;
        }

        public IEnumerable<StoreInventoryHistoryQueryDto> GetPageList(Pager page, SearchStoreInventoryHistory condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";           
            if (condition.StoreId > 0)
            {
                where += "and t0.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
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
            if (condition.BillType > 0)
            {
                where += "and t0.BillType=@BillType ";
                param.BillType = condition.BillType;
            }

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t1.Code=@ProductCodeOrBarCode or t1.BarCode=@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            string sql = @"select t0.*,t1.`Code` as ProductCode ,t1.`Name` as ProductName,t1.BarCode,t1.Specification,t2.`name` as StoreName
from storeinventoryhistory t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";          
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreInventoryHistoryQueryDto>(sql, param);

            string sqlCount = @"select count(*) from storeinventoryhistory t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId 
where 1=1 {0} ";
            sqlCount = string.Format(sqlCount, where);
            int rowCount= this._query.Context.ExecuteScalar<int>(sqlCount, param);
            page.Total = rowCount;

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
            if (condition.StoreId > 0)
            {
                where += "and t0.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
            }

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t1.Code=@ProductCodeOrBarCode or t1.BarCode=@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            string sql = @"select t0.*,t1.`Code` as ProductCode ,t1.`Name` as ProductName,t1.BarCode,t1.Specification,t2.`name` as StoreName,t3.`Name` as SupplierName
from storeinventorybatch t0 left join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId
left join supplier t3 on t3.Id = t0.SupplierId 
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
            if (condition.StoreId > 0)
            {
                where += "and t.Id=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            string sql = @"select b.ProductId, p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit,p.SalePrice,
b.Quantity as BatchQuantity,s.Name as supplierName,t.`Name` as StoreName,b.Price,sp.SalePrice as StoreSalePrice,v.SalePrice as VipSalePrice
from ( select i.storeid,i.supplierId,i.productId,i.Price,sum(i.quantity) as Quantity from storeinventorybatch i group by  i.storeid,i.supplierId,i.productId,i.Price ) b 
left join product p on b.ProductId = p.Id
left join supplier s on s.Id = b.SupplierId
left join store t on t.Id = b.StoreId
left join productstoreprice sp on sp.ProductId = p.Id
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
