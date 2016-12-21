using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application;
using Dapper.DBContext;
using EBS.Domain.Service;
using EBS.Domain.Entity;
using EBS.Application.DTO;
namespace EBS.Application.Facade
{
    public class RoleFacade : IRoleFacade
    {
        IDBContext _db;
        RoleService _service;
        public RoleFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new RoleService(this._db);
        }
        public void Create(RoleModel model)
        {
            Role entity = new Role(model.Name, model.Description);
            entity.AssignMenus(model.MenuIds);
            _service.Create(entity); // 框架自动实现 子外键关联对象添加
            _db.SaveChange();
        }

        public void Edit(RoleModel model)
        {
            Role entity = new Role(model.Name, model.Description,model.Id);
            _service.Update(entity);
            // 权限
            entity.AssignMenus(model.MenuIds);
            if (_db.Table.Exists<RoleMenu>(n => n.RoleId == model.Id))
            {
                _db.Delete<RoleMenu>(n => n.RoleId == model.Id);
            }           
            _db.Insert<RoleMenu>(entity.Items.ToArray());
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
            _service.Delete(ids);
            _db.SaveChange();
        }
    }
}
