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
            string sql = @"select t0.*,t1.`Code` as ProductCode ,t1.`Name` as ProductName,t1.BarCode,t1.Specification,t2.`name` as StoreName
from storeinventory t0 inner join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreInventoryQueryDto>(sql, param);
            // page.Total = this._query.Count<StoreInventory>(where, param);
            page.Total = this._query.Count<StoreInventory>();

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
from storeinventoryhistory t0 inner join product t1 on t0.productId = t1.Id
inner join store t2 on t2.Id = t0.StoreId 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreInventoryHistoryQueryDto>(sql, param);
            // page.Total = this._query.Count<StoreInventory>(where, param);
            page.Total = this._query.Count<StoreInventoryHistory>();

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
from storeinventorybatch t0 inner join product t1 on t0.productId = t1.Id
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

}
}
