using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Domain.Service;
using EBS.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.Facade
{
   public class SaleReportFacade:ISaleReportFacade
    {
       IDBContext _db;
       SaleReportService _saleReportService;
       ILogger _log;
       public SaleReportFacade(IDBContext dbContext,ILogger log)
       {
           this._db = dbContext;
           _saleReportService = new SaleReportService(this._db);
           _log = log;
       }
        public void Create(DateTime beginDate, DateTime endDate)
        {
            if (beginDate > endDate) throw new Exception("开始日期不能大于结束日期");
            _log.Info("开始生成{0}~{1} 的销售报表", beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            var from = beginDate;
            var to = endDate.AddDays(1);  //每次取一天的数据进行处理
           
            while (from < to)
            {
                var tomorrow = from.AddDays(1);
                _saleReportService.CreateSaleReport(from, tomorrow);
                _log.Info("生成{0}的销售报表数据", from.ToString("yyyy-MM-dd"));
                from = tomorrow;
            }
            _log.Info("生成{0}~{1} 的销售报表结束", beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            _log.Info("=========================");
        }
    }
}
