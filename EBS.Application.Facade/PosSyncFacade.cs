using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Application.Facade.Mapping;
using Dapper.DBContext;
namespace EBS.Application.Facade
{
   public class PosSyncFacade:IPosSyncFacade
    {
       IDBContext _db;

        public PosSyncFacade(IDBContext dbContext)
        {
            _db = dbContext;

        }
        public void HandlerMessage(Message.SaleOrderSyncMessage message)
        {
              var entity= message.MapTo<SaleOrder>();
              _db.Insert(entity);
              _db.SaveChange();
              
        }
    }
}
