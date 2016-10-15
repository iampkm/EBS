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
    public class MenuService
    {
        IDBContext _db;
        public MenuService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(Menu model)
        {
            if (_db.Table.Exists<Menu>(n => n.Name == model.Name))
            {
                throw new Exception("名称重复!");
            }
            _db.Insert(model);
            _db.SaveChange();
        }

        public void Update(Menu model)
        {
            if (_db.Table.Exists<Menu>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
            }
            _db.Update(model);
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<Menu>(arrIds);
            _db.SaveChange();
        }

    }
}
