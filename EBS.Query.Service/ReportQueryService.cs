using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.Service
{
   public class ReportQueryService:IReportQuery
    {
        IQuery _query;
        public ReportQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<PurchaseSaleInventoryDto> QueryPurchaseSaleInventorySummary(Pager page, PurchaseSaleInventorySearch condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.StoreId))
            {
                where += "and t.StoreId in("+condition.StoreId+")";
               // param.StoreId = condition.StoreId;
            }
            
            param.YearMonth = int.Parse(string.Format("{0}{1}",condition.Year,condition.Month.ToString().PadLeft(2,'0')));

            // 此处多显示起止时间，主要是为了让前端表格框架，连接明细时能传递时间参数
            string sql = @"select * from purchasesaleinventory t
where YearMonth=@YearMonth {0} LIMIT {1},{2}";
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<PurchaseSaleInventoryDto>(sql, param);
            string sqlCount = @"select count(*) from purchasesaleinventory t
where YearMonth=@YearMonth {0} ";
            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount,param);
            //求和
            string sqlSum = @"select sum(PreInventoryQuantity) as PreInventoryQuantity,sum(PreInventoryAmount) as PreInventoryAmount,sum(PurchaseQuantity) as PurchaseQuantity,sum(PurchaseAmount) as PurchaseAmount,sum(SaleQuantity) as SaleQuantity,sum(SaleCostAmount) as SaleCostAmount,sum(SaleAmount) as SaleAmount,sum(EndInventoryQuantity) as EndInventoryQuantity,sum(EndInventoryAmount) as EndInventoryAmount from purchasesaleinventory t where YearMonth=@YearMonth {0}";
            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory = this._query.Find<SumPurchaseSaleInventory>(sqlSum, param) as SumPurchaseSaleInventory;
            page.SumColumns.Add(new SumColumn("PreInventoryQuantity", sumStoreInventory.PreInventoryQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("PreInventoryAmount", sumStoreInventory.PreInventoryAmount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("PurchaseQuantity", sumStoreInventory.PurchaseQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("PurchaseAmount", sumStoreInventory.PurchaseAmount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("SaleQuantity", sumStoreInventory.SaleQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("SaleCostAmount", sumStoreInventory.SaleCostAmount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("SaleAmount", sumStoreInventory.SaleAmount.ToString("F2")));
            page.SumColumns.Add(new SumColumn("EndInventoryQuantity", sumStoreInventory.EndInventoryQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("EndInventoryAmount", sumStoreInventory.EndInventoryAmount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("ProfitAmount", sumStoreInventory.ProfitAmount.ToString()));
            page.SumColumns.Add(new SumColumn("ProfitPercent", sumStoreInventory.ProfitPercent.ToString("F2")+"%"));
            return rows;
        }


        public IEnumerable<PurchaseSaleInventoryDetailDto> QueryPurchaseSaleInventoryDetail(Pager page, PurchaseSaleInventoryDetailSearch condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
           
            where += "and t.StoreId = @StoreId ";
            param.StoreId = condition.StoreId;
            param.YearMonth = int.Parse(string.Format("{0}{1}", condition.Year, condition.Month.ToString().PadLeft(2, '0')));

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t.`ProductCode` = @ProductCodeOrBarCode or t.BarCode =@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }

            string sql = @"select * from purchasesaleinventoryDetail t
where YearMonth=@YearMonth {0} LIMIT {1},{2}";
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<PurchaseSaleInventoryDetailDto>(sql, param);
            string sqlCount = @"select count(*) from purchasesaleinventoryDetail t
where YearMonth=@YearMonth {0}";

            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
           
            return rows;
        }

        public IList<int> GetYears()
        {
            string sql = "select DISTINCT year(createdon) from storeinventoryhistory ";
            var rows = _query.Context.Query<int>(sql, null).ToList();
            //去掉月份
            if (rows.Count == 0)
            {
                rows.Add(DateTime.Now.Year);
            }
            return rows;
        }
    }
}
