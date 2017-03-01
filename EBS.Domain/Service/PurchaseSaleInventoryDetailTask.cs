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
using EBS.Domain.Entity;
namespace EBS.Domain.Service
{
    public class PurchaseSaleInventoryDetailTask : ITask
    {
        IDBContext _db;
        ILogger _log;
        DateTime _currentDate;
        public PurchaseSaleInventoryDetailTask()
        {
            _db = AppContext.Current.Resolve<IDBContext>();
            _log = AppContext.Current.Resolve<ILogger>();
             _currentDate = DateTime.Now;
        }

        public PurchaseSaleInventoryDetailTask(DateTime selectDate)
            : this()
       {
           this._currentDate = selectDate;
       }

//        public void Execute()
//        {
//            Stopwatch watch = new Stopwatch();
//            Stopwatch watchEvery = new Stopwatch();
//            watch.Start();
//            var today = this._currentDate;
//            var yearMonth = int.Parse(today.ToString("yyyyMM"));
////            // REPLACE INTO PurchaseSaleInventoryDetail (yearMonth,StoreId,ProductId,ProductCode,BarCode,ProductName,PreInventoryQuantity,PreInventoryAmount,PurchaseQuantity,PurchaseAmount
////,SaleQuantity,SaleCostAmount,SaleAmount,EndInventoryQuantity,EndInventoryAmount,updatedOn)  
//            _log.Info("执行{0}进销存明细报表自动任务", today);
//            string sql = @"select @YearMonth as YearMonth,@StoreId as StoreId,t.Id as ProductId, t.`code` as productCode,t.BarCode, t.name as ProductName,
//ifnull(t1.qty,0) as PreInventoryQuantity,ifnull(t1.amount,0) as PreInventoryAmount,
//ifnull(t2.qty,0) as PurchaseQuantity,ifnull(t2.amount,0) as PurchaseAmount,
//ifnull(t4.qty,0) as SaleQuantity,ifnull(t4.amount,0) as SaleCostAmount,
//ifnull(t5.amount,0) as SaleAmount,
//ifnull(t3.qty,0) as EndInventoryQuantity ,ifnull(t3.amount,0) as EndInventoryAmount from 
//storeinventory s left join product t on t.Id = s.ProductId
// LEFT JOIN 
// (
//	select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
//where storeid=@StoreId and  createdOn <@StartDate
//group by productid
//) t1 on t.Id = t1.productid left join 
//(
//		select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
//where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate    and BillType in (51,52,53,60)
//group by productid
//) t2 on t.Id = t2.productid
//left  join 
//(
//select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
//where storeid=@StoreId and createdOn <@EndDate
//group by productid
//) t3 on t.Id = t3.productid
//left join 
//(
//	select productid,abs(SUM(changequantity)) as qty,abs(sum(changequantity*price)) as amount from storeinventoryhistory 
//where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate   and BillType in (1,2)
//group by productid
//) t4 on t.Id = t4.productid
//left join 
//(
//	select i.productid,sum(i.Quantity) qty,sum(i.realprice*i.Quantity) amount from saleorder s inner join saleorderitem i 
//on s.id= i.saleorderid
//where s.storeid=@StoreId and s.`Status` =3 and s.UpdatedOn BETWEEN @StartDate and @EndDate 
//group by i.productid
//) t5 on t.Id = t5.productid where s.StoreId = @StoreId";
//            var startDate = new DateTime(today.Year, today.Month, 1);
//            var endDate = startDate.AddMonths(1); // 统计当月所有数据
//            var stores = _db.Table.FindAll<Store>();
//            string sqlExists = "select count(*) from PurchaseSaleInventoryDetail where YearMonth=@YearMonth and StoreId=@StoreId and ProductId=@ProductId";
//            string sqlUpdate = "update PurchaseSaleInventoryDetail set ProductCode=@ProductCode,BarCode=@BarCode,ProductName=@ProductName,PreInventoryQuantity=@PreInventoryQuantity,PreInventoryAmount=@PreInventoryAmount,PurchaseQuantity=@PurchaseQuantity,PurchaseAmount=@PurchaseAmount,SaleQuantity=@SaleQuantity,SaleCostAmount=@SaleCostAmount,SaleAmount=@SaleAmount,EndInventoryQuantity=@EndInventoryQuantity,EndInventoryAmount=@EndInventoryAmount,updatedOn=@updatedOn where YearMonth=@YearMonth and StoreId=@StoreId and ProductId=@ProductId";
//            List<PurchaseSaleInventoryDetail> updateList = new List<PurchaseSaleInventoryDetail>();
//            List<PurchaseSaleInventoryDetail> insertList = new List<PurchaseSaleInventoryDetail>();
//            foreach (var store in stores)
//            {
//                try
//                {
//                    _log.Info("[{0}]进销存明细报表生成开始", store.Name);
//                    watchEvery.Start();
//                    // 查询数据
//                   var rows=  _db.Table.FindAll<PurchaseSaleInventoryDetail>(sql, new { YearMonth = yearMonth, StartDate = startDate, EndDate = endDate, StoreId = store.Id });
//                    // 写入进销售存明细表
//                   foreach (var item in rows)
//                   {
//                      var count= _db.Table.Context.ExecuteScalar<int>(sqlExists, new { YearMonth = yearMonth, StoreId = store.Id,ProductId = item.ProductId });
//                      if (count > 0)
//                      {
//                          updateList.Add(item);
//                      }
//                      else
//                      {
//                          insertList.Add(item);
//                      }
//                   }
//                    //入库
//                   if (updateList.Count > 0)
//                   {
//                       _db.Command.AddExecute(sqlUpdate, updateList.ToArray());
//                   }
//                   else
//                   {
//                       _db.Insert<PurchaseSaleInventoryDetail>(insertList.ToArray());
//                   }
//                   if (rows.Count() > 0)
//                   {
//                       _db.SaveChange();       
//                   }
//                   // _db.Command.Execute(sql, new { YearMonth = yearMonth, StartDate = startDate, EndDate = endDate, UpdatedOn = DateTime.Now, StoreId = store.Id },180);
//                    watchEvery.Stop();
//                    _log.Info("[{0}]进销存明细报表生成结束,耗时{1}毫秒", store.Name, watchEvery.ElapsedMilliseconds);
//                    watchEvery.Reset();
//                }
//                catch (Exception ex)
//                {
//                    _log.Error(ex);
//                    watchEvery.Stop();
//                    _log.Info("[{0}]进销存明细报表生成出错,耗时{1}毫秒", store.Name, watchEvery.ElapsedMilliseconds);
//                    watchEvery.Reset();
//                }
//            }
//            watch.Stop();
//            _log.Info("进销存明细报表自动任务结束,耗时{0}毫秒", watch.ElapsedMilliseconds);
//            _log.Info("----------------------------------");
//        }


        public void Execute()
        {
            Stopwatch watch = new Stopwatch();
            Stopwatch watchEvery = new Stopwatch();
            watch.Start();
            var today = this._currentDate;
            var yearMonth = int.Parse(today.ToString("yyyyMM")); 
            _log.Info("执行{0}进销存明细报表自动任务", today);
            string sql = @"REPLACE INTO PurchaseSaleInventoryDetail (yearMonth,StoreId,ProductId,ProductCode,BarCode,ProductName,PreInventoryQuantity,PreInventoryAmount,PurchaseQuantity,PurchaseAmount
,SaleQuantity,SaleCostAmount,SaleAmount,EndInventoryQuantity,EndInventoryAmount,updatedOn)  
select @YearMonth as YearMonth,@StoreId as StoreId,t.Id as ProductId, t.`code` as productCode,t.BarCode, t.name as ProductName,
ifnull(t1.qty,0) as PreInventoryQuantity,ifnull(t1.amount,0) as PreInventoryAmount,
ifnull(t2.qty,0) as PurchaseQuantity,ifnull(t2.amount,0) as PurchaseAmount,
ifnull(t4.qty,0) as SaleQuantity,ifnull(t4.amount,0) as SaleCostAmount,
ifnull(t5.amount,0) as SaleAmount,
ifnull(t3.qty,0) as EndInventoryQuantity ,ifnull(t3.amount,0) as EndInventoryAmount,@UpdatedOn from 
storeinventory s left join product t on t.Id = s.ProductId
 LEFT JOIN 
 (
	select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and  createdOn <@StartDate
group by productid
) t1 on t.Id = t1.productid left join 
(
		select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate    and BillType in (51,52,53,60)
group by productid
) t2 on t.Id = t2.productid
left  join 
(
select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and createdOn <@EndDate
group by productid
) t3 on t.Id = t3.productid
left join 
(
	select productid,abs(SUM(changequantity)) as qty,abs(sum(changequantity*price)) as amount from storeinventoryhistory 
where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate   and BillType in (1,2)
group by productid
) t4 on t.Id = t4.productid
left join 
(
	select i.productid,sum(i.Quantity) qty,sum(i.realprice*i.Quantity) amount from saleorder s inner join saleorderitem i 
on s.id= i.saleorderid
where s.storeid=@StoreId and s.`Status` =3 and s.UpdatedOn BETWEEN @StartDate and @EndDate 
group by i.productid
) t5 on t.Id = t5.productid where s.StoreId = @StoreId";
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = startDate.AddMonths(1); // 统计当月所有数据
            var stores = _db.Table.FindAll<Store>();           
            foreach (var store in stores)
            {
                try
                {
                    _log.Info("[{0}]进销存明细报表生成开始", store.Name);
                    watchEvery.Start();                    
                    _db.Command.Execute(sql, new { YearMonth = yearMonth, StartDate = startDate, EndDate = endDate, UpdatedOn = DateTime.Now, StoreId = store.Id },180);
                    watchEvery.Stop();
                    _log.Info("[{0}]进销存明细报表生成结束,耗时{1}毫秒", store.Name, watchEvery.ElapsedMilliseconds);
                    watchEvery.Reset();
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                    watchEvery.Stop();
                    _log.Info("[{0}]进销存明细报表生成出错,耗时{1}毫秒", store.Name, watchEvery.ElapsedMilliseconds);
                    watchEvery.Reset();
                }
            }
            watch.Stop();
            _log.Info("进销存明细报表自动任务结束,耗时{0}毫秒", watch.ElapsedMilliseconds);
            _log.Info("----------------------------------");
        }
    }
}
