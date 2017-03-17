using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
using EBS.Domain.Entity;
using Dapper.DBContext;
namespace EBS.Domain.Service
{
   public class TransferOrderService
    {
        IDBContext _db;
        public TransferOrderService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public void EditItem(TransferOrder entity)
        {
            //明细
            if (_db.Table.Exists<TransferOrderItem>(n => n.TransferOrderId == entity.Id))
            {
                _db.Delete<TransferOrderItem>(n => n.TransferOrderId == entity.Id);
            }

            _db.Insert(entity.Items.ToArray());
        }
    }
}
