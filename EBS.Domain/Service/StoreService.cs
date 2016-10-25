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
   public class StoreService
    {
        IDBContext _db;
        public StoreService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(Store model)
        {
            if (_db.Table.Exists<Store>(n => n.Name == model.Name))
            {
                throw new Exception("名称重复!");
            }
            _db.Insert(model);           
        }

        public void Update(Store model)
        {
            if (_db.Table.Exists<Store>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
            }
            var entity = _db.Table.Find<Store>(model.Id);
            entity.Name = model.Name;
          //  entity.Description = model.Description;
            _db.Update(entity);
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<Store>(arrIds);
            _db.SaveChange();
            //删除权限
        }
    }
}
