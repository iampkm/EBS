using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Task;
using Dapper.DBContext;
using EBS.Infrastructure;
using EBS.Infrastructure.Log;
using System.Diagnostics;
namespace EBS.Domain.Service
{
    public class PurchaseSaleInventoryTask : ITask
    {
       IDBContext _db;
       ILogger _log;
       DateTime _currentDate;
       public PurchaseSaleInventoryTask()
       {
           _db = AppContext.Current.Resolve<IDBContext>();
           _log = AppContext.Current.Resolve<ILogger>();
           _currentDate = DateTime.Now;
       }

        /// <summary>
        /// 此方法用来手动执行任务
        /// </summary>
        /// <param name="selectDate"></param>
       public PurchaseSaleInventoryTask(DateTime selectDate):this()
       {
           this._currentDate = selectDate;
       }

       public void Execute()
       {
           Stopwatch watch = new Stopwatch();
           watch.Start();
          // var today = DateTime.Now;
           var today = this._currentDate;
           var yearMonth =int.Parse(today.ToString("yyyyMM"));
           _log.Info("执行{0}进销存报表自动任务", today);
           string sql = @"REPLACE INTO PurchaseSaleInventory (yearMonth,StoreId,StoreName,PreInventoryQuantity,PreInventoryAmount,PurchaseQuantity,PurchaseAmount
,SaleQuantity,SaleCostAmount,SaleAmount,EndInventoryQuantity,EndInventoryAmount,updatedOn)  
select @YearMonth,t.Id as StoreId ,t.name as StoreName,ifnull(t1.qty,0) as PreInventoryQuantity,ifnull(t1.amount,0) as PreInventoryAmount,
ifnull(t2.qty,0) as PurchaseQuantity,ifnull(t2.amount,0) as PurchaseAmount,
ifnull(t4.qty,0) as SaleQuantity,ifnull(t4.amount,0) as SaleCostAmount,
ifnull(t5.amount,0) as SaleAmount,
ifnull(t3.qty,0) as EndInventoryQuantity ,ifnull(t3.amount,0) as EndInventoryAmount,@UpdatedOn from 
store t LEFT JOIN 
 (
	select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn <@StartDate
group by storeid
) t1 on t.Id = t1.storeid left join 
(
		select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn BETWEEN @StartDate and @EndDate   and BillType in (51,52,53,60)
group by storeid
) t2 on t.Id = t2.storeid
left  join 
(
select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn <@EndDate
group by storeid
) t3 on t.Id = t3.storeid
left join 
(
	select storeid,abs(SUM(changequantity)) as qty,abs(sum(changequantity*price)) as amount from storeinventoryhistory 
where createdOn BETWEEN @StartDate and @EndDate  and BillType in (1,2)
group by storeid
) t4 on t.Id = t4.StoreId
left join 
(
	select s.storeid,sum(i.Quantity) qty,sum(i.realprice*i.Quantity) amount from saleorder s inner join saleorderitem i 
on s.id= i.saleorderid
where s.`Status` = 3 and s.UpdatedOn BETWEEN @StartDate and @EndDate 
group by s.storeid
) t5 on t.Id = t5.StoreId";
           var startDate = new DateTime(today.Year, today.Month, 1);
           var endDate = startDate.AddMonths(1); // 统计当月所有数据
            try
            {
                _db.Command.Execute(sql, new { YearMonth = yearMonth, StartDate = startDate, EndDate = endDate, UpdatedOn = DateTime.Now },180);
            }
            catch (Exception ex)
            {
                _log.Error(ex);               
            }           
            watch.Stop();
            _log.Info("进销存报表自动任务结束,耗时{0}毫秒", watch.ElapsedMilliseconds);
            _log.Info("----------------------------------");
       }
    }
}
