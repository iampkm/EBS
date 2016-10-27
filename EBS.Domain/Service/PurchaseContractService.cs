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
   public class PurchaseContractService
    {
        IDBContext _db;
        public PurchaseContractService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(PurchaseContract model)
        {
            if (_db.Table.Exists<PurchaseContract>(n => n.Name == model.Name))
            {
                throw new Exception("名称重复!");
            }
            _db.Insert(model);           
        }

        public void Update(PurchaseContract model)
        {
            if (_db.Table.Exists<PurchaseContract>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
            }
            _db.Update(model);
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<PurchaseContract>(arrIds);
            _db.SaveChange();
            //删除权限
        }
    }
}
