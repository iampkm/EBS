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
   public class RoleService
    {
         IDBContext _db;
         public RoleService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(Role model)
        {
            if (_db.Table.Exists<Role>(n => n.Name == model.Name))
            {
                throw new Exception("名称重复!");
            }
            _db.Insert(model);           
        }

        public void Update(Role model)
        {
            if (_db.Table.Exists<Role>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
            }
            var entity = _db.Table.Find<Role>(m => m.Id == model.Id);
            entity.Name = model.Name;
            entity.Description = model.Description;
            _db.Update(entity);
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            foreach (var id in arrIds)
            {
                if (_db.Table.Exists<RoleMenu>(m => m.RoleId == id))
                {
                    _db.Delete<RoleMenu>(m => m.RoleId == id);
                }
            }           
            _db.Delete<Role>(arrIds);
            //删除权限
        }
    }
}
