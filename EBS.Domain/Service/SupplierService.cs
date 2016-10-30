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
   public class SupplierService
    {
        IDBContext _db;
        public SupplierService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(Supplier model)
        {
            if (_db.Table.Exists<Supplier>(n => n.Code == model.Code))
            {
                throw new Exception("编码重复，请修改!");
            }
            _db.Insert(model);           
        }

        public void Update(Supplier model)
        {
            if (_db.Table.Exists<Supplier>(n => n.Code == model.Code))
            {
                throw new Exception("编码重复，请修改!");
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
            _db.Delete<Supplier>(arrIds);
            _db.SaveChange();
            //删除权限
        }
    }
}
