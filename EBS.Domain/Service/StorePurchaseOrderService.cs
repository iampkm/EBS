using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
namespace EBS.Domain.Service
{
   public class StorePurchaseOrderService
    {
        IDBContext _db;       
        public StorePurchaseOrderService(IDBContext dbContext)
        {
            this._db = dbContext;
        }
        public void Create(StorePurchaseOrder model)
        {
            
            if (_db.Table.Exists<StorePurchaseOrder>(n => n.Code == model.Code))
            {
                throw new Exception("采购单号已经存在");
            }           
            _db.Insert(model);
        }

        public void UpdateWithItem(StorePurchaseOrder model)
        {
            if (_db.Table.Exists<StorePurchaseOrder>(n => n.Code == model.Code && n.Id != model.Id))
            {
                throw new Exception("采购单号不能重复!");
            }
            if (_db.Table.Exists<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == model.Id))
            {
                _db.Delete<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == model.Id);
            }          
            _db.Insert<StorePurchaseOrderItem>(model.Items.ToArray());
            _db.Update(model);
        }
      
    }
}
