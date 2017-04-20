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
        PurchaseSaleInventoryService _service;
        public PurchaseSaleInventoryTask()
       {
           _db = AppContext.Current.Resolve<IDBContext>();
           _log = AppContext.Current.Resolve<ILogger>();
            _service = new PurchaseSaleInventoryService(_db, _log);
        }
       

       public void Execute()
       {
            _service.Generate(DateTime.Now);
       }
    }
}
