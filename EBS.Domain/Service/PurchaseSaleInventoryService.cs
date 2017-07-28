using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Infrastructure;
using EBS.Infrastructure.Log;
using System.Diagnostics;
using EBS.Domain.Entity;

namespace EBS.Domain.Service
{
   public class PurchaseSaleInventoryService
    {
        IDBContext _db;
        ILogger _log;
        public PurchaseSaleInventoryService(IDBContext dbContext,ILogger log)
       {
            this._db = dbContext;
            this._log = log;
        }
       public void Generate(DateTime today)
       {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // var today = DateTime.Now;
            //var today = this._currentDate;
            var yearMonth = int.Parse(today.ToString("yyyyMM"));
            var lastYearMonth = int.Parse(today.AddMonths(-1).ToString("yyyyMM"));
            _log.Info("执行{0}进销存报表自动任务", today);
            string sql = @"REPLACE INTO PurchaseSaleInventory (yearMonth,StoreId,StoreName,PreInventoryQuantity,PreInventoryAmount,PurchaseQuantity,PurchaseAmount
,SaleQuantity,SaleCostAmount,SaleAmount,EndInventoryQuantity,EndInventoryAmount,updatedOn)  
select @YearMonth,s.Id as StoreId,s.Name as StoreName,
IFNULL(d.EndInventoryQuantity,0) as PreInventoryQuantity,IFNULL(d.EndInventoryAmount,0)  as PreInventoryAmount,
IFNULL(c.PurchaseQuantity,0) as PurchaseQuantity,IFNULL(c.PurchaseAmount,0) as PurchaseAmount,IFNULL(c.SaleQuantity,0) as SaleQuantity,IFNULL(c.SaleCostAmount,0) as SaleCostAmount,IFNULL(c.SaleAmount,0) as SaleAmount,
(IFNULL(d.EndInventoryQuantity,0)+ IFNULL(c.PurchaseQuantity,0)- IFNULL(c.SaleQuantity,0)) as EndInventoryQuantity ,
(IFNULL(d.EndInventoryAmount,0) + IFNULL(c.PurchaseAmount,0)-IFNULL(c.SaleCostAmount,0)) as EndInventoryAmount ,@UpdatedOn
from store s 
LEFT JOIN purchasesaleinventory d on s.Id = d.StoreId and d.YearMonth = @LastYearMonth 
left join (
	select h.storeid,
IFNULL(sum(case when h.BillType in (51,52,53,60,61,62)  then h.ChangeQuantity	end),0)  as PurchaseQuantity, 
IFNULL(sum(case when  h.BillType in (51,52,53,60,61,62)  then h.ChangeQuantity*h.Price end),0)  as PurchaseAmount, 
IFNULL(abs(sum(case when h.BillType in (1,2)  then h.ChangeQuantity end)),0)  as SaleQuantity, 
IFNULL(abs(sum(case when  h.BillType in (1,2)  then h.ChangeQuantity*h.Price end)),0)  as SaleCostAmount ,
IFNULL(abs(sum(case when  h.BillType in (1,2)  then h.ChangeQuantity*h.SalePrice end)),0)  as SaleAmount  
from StoreInventoryHistory h 
where h.CreatedOn BETWEEN @StartDate and @EndDate
group by h.storeid
) c on c.storeid = s.Id 
order by s.Id ";
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = startDate.AddMonths(1); // 统计当月所有数据
            try
            {
                _db.Command.Execute(sql, new { YearMonth = yearMonth,LastYearMonth= lastYearMonth, StartDate = startDate, EndDate = endDate, UpdatedOn = DateTime.Now }, 180);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            watch.Stop();
            _log.Info("进销存报表自动任务结束,耗时{0}毫秒", watch.Elapsed);
            _log.Info("----------------------------------");
        }


       public void GenerateDetail(DateTime today)
       {
            Stopwatch watch = new Stopwatch();         
            watch.Start();
            var yearMonth = int.Parse(today.ToString("yyyyMM"));
            var lastYearMonth = int.Parse(today.AddMonths(-1).ToString("yyyyMM"));
            _log.Info("执行{0}进销存明细报表自动任务", today);
            string sql = @"REPLACE INTO PurchaseSaleInventoryDetail 
(yearMonth,StoreId,ProductId,PreInventoryQuantity,PreInventoryAmount,PurchaseQuantity,PurchaseAmount
,SaleQuantity,SaleCostAmount,SaleAmount,EndInventoryQuantity,EndInventoryAmount,avgCostPrice,updatedOn)  
select @YearMonth,s.StoreId,s.ProductId, 
IFNULL(d.EndInventoryQuantity,0) as PreInventoryQuantity,IFNULL(d.EndInventoryAmount,0)  as PreInventoryAmount,
IFNULL(c.PurchaseQuantity,0) as PurchaseQuantity,IFNULL(c.PurchaseAmount,0) as PurchaseAmount,IFNULL(c.SaleQuantity,0) as SaleQuantity,IFNULL(c.SaleCostAmount,0) as SaleCostAmount,IFNULL(c.SaleAmount,0) as SaleAmount,
(IFNULL(d.EndInventoryQuantity,0)+ IFNULL(c.PurchaseQuantity,0)- IFNULL(c.SaleQuantity,0)) as EndInventoryQuantity ,
(IFNULL(d.EndInventoryAmount,0) + IFNULL(c.PurchaseAmount,0)-IFNULL(c.SaleCostAmount,0)) as EndInventoryAmount ,IFNULL(c.avgCostPrice ,0) as avgCostPrice ,@UpdatedOn 
from storeinventory s 
LEFT JOIN purchasesaleinventorydetail d on s.StoreId = d.StoreId and s.ProductId = d.ProductId and d.YearMonth = @LastYearMonth 
left join (
	select h.storeid,h.productid,
IFNULL(sum(case when h.BillType in (51,52,53,60,61,62)  then h.ChangeQuantity end),0)  as PurchaseQuantity, 
IFNULL(sum(case when  h.BillType in (51,52,53,60,61,62)  then h.ChangeQuantity*h.Price end),0)  as PurchaseAmount, 
IFNULL(abs(sum(case when h.BillType in (1,2)  then h.ChangeQuantity end)),0)  as SaleQuantity,
IFNULL(abs(sum(case when  h.BillType in (1,2)  then h.ChangeQuantity*h.Price end)),0)  as SaleCostAmount ,
IFNULL(abs(sum(case when  h.BillType in (1,2)  then h.ChangeQuantity*h.SalePrice end)),0)  as SaleAmount,
avg(IfNull(h.price,0)) as avgCostPrice 
from StoreInventoryHistory h 
where h.CreatedOn BETWEEN @StartDate and @EndDate
group by h.storeid,h.productid 
) c on c.storeid = s.storeid and c.productid = s.productid
order by s.storeid,s.productid ";
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = startDate.AddMonths(1); // 统计当月所有数据
            try
            {
                _log.Info("进销存明细报表生成开始");

                _db.Command.Execute(sql, new { YearMonth = yearMonth, StartDate = startDate, EndDate = endDate, UpdatedOn = DateTime.Now, LastYearMonth = lastYearMonth }, 600);
                _log.Info("进销存明细报表生成成功！");
            }
            catch (Exception ex)
            {
                _log.Info("进销存明细报表生成失败！");
                _log.Error(ex);               
            }
            finally {
                watch.Stop();
                _log.Info("进销存明细报表生成结束,耗时{0}", watch.Elapsed);
            }
            _log.Info("----------------------------------");
        }
    }
}
