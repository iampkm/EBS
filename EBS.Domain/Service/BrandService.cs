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
    public class BrandService
    {
        IDBContext _db;
        public BrandService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(Brand model)
        {
            if (_db.Table.Exists<Brand>(n => n.Name == model.Name))
            {
                throw new Exception("名称重复!");
            }
            _db.Insert(model);
            _db.SaveChange();
        }

        public void Update(Brand model)
        {
            if (_db.Table.Exists<Brand>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
            }
            var entity = _db.Table.Find<Brand>(model.Id);
            entity.Name = model.Name;
            _db.Update(entity);
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<Brand>(arrIds);
            _db.SaveChange();
        }
    }
}
