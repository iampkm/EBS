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
   public class ProductService
    {
         IDBContext _db;
         public ProductService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(Product model)
        {
            if (_db.Table.Exists<Product>(n => n.Name == model.Name))
            {
                throw new Exception("名称重复!");
            }
            _db.Insert(model);
        }

        public void Update(Product model)
        {
            if (_db.Table.Exists<Product>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
            }
            var entity = _db.Table.Find<Product>(model.Id);
            // 修改商品属性
            entity.Name = model.Name;
            entity.Keywords = model.Keywords;
            entity.ShowName = model.ShowName;
            entity.SellingPoint = model.SellingPoint;
            entity.SkuItems = model.SkuItems;
            _db.Update(entity);

        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<Product>(arrIds);
        }
    }
}
