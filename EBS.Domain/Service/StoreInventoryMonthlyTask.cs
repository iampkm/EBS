using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Task;
using Dapper.DBContext;
using EBS.Infrastructure;
using EBS.Infrastructure.Log;
namespace EBS.Domain.Service
{
    /// <summary>
    /// 月库存自动任务：每月1日执行，保存上月库存
    /// </summary>
   public class StoreInventoryMonthlyTask:ITask
    {
       IDBContext _db;
       ILogger _log;
       public StoreInventoryMonthlyTask()
       {
           _db = AppContext.Current.Resolve<IDBContext>();
           _log = AppContext.Current.Resolve<ILogger>();
       }
        public void Execute()
        {
            var today = DateTime.Now;
            var todayString =today.ToString("yyyy-MM-dd HH:mm:ss");
            _log.Info("执行{0}库存存储自动任务", todayString);
            if (today.AddDays(1).Day == 1)
            {
                // 每天 00:03分 执行
                _log.Info("时间条件{0}是月末，开始存储当月库存", todayString);
                string sql = @"insert into StoreInventoryMonthly (Monthly,StoreId,ProductId,Quantity,AvgCostPrice) 
            select @Monthly,StoreId,ProductId,Quantity,AvgCostPrice from StoreInventory";
                _db.Command.Execute(sql, new { Monthly = today.ToString("yyyy-MM") });
            }
            else
            {
                _log.Info("时间条件{0}不满足，结束任务", todayString);
            }
            _log.Info("任务执行完毕");

        }
    }
}
