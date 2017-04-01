using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Task;
using Dapper.DBContext;
using EBS.Infrastructure.Log;
using EBS.Infrastructure;
using EBS.Domain.Entity;
namespace EBS.Domain.Service
{
    public class SaleReportTask : ITask
    {
         IDBContext _db;
        ILogger _log;
        DateTime _currentDate;
        SaleReportService _saleReportService;
        public SaleReportTask()
        {
            _db = AppContext.Current.Resolve<IDBContext>();
            _log = AppContext.Current.Resolve<ILogger>();
             _currentDate = DateTime.Now;
             _saleReportService = new SaleReportService(this._db);

        }

        public void Execute()
        {
            //自动任务跑前一天的销售数据
            DateTime endDate = _currentDate.Date;
            DateTime beginDate = endDate.AddDays(-1);
            _log.Info("开始生成{0}~{1} 的销售报表", beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            _saleReportService.CreateSaleReport(beginDate, endDate);
            _log.Info("生成{0}~{1} 的销售报表结束", beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            _log.Info("=========================");           
        }
    }
}
