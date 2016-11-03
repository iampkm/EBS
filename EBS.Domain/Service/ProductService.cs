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
             model.Code = GenerateNewCode(model.CategoryId);
             _db.Insert(model);
         }

         public string GenerateNewCode(string categoryId)
         {
             // 生成一个新的 Code 序列号
             if (string.IsNullOrEmpty(categoryId)) throw new Exception("分类编码为空");
             categoryId = categoryId.Substring(0, 2);
             var codeSequence = new ProductCodeSequence();
             _db.Insert<ProductCodeSequence>(codeSequence);
             _db.SaveChange();
             codeSequence = _db.Table.Find<ProductCodeSequence>(n => n.GuidCode == codeSequence.GuidCode);
             var sequenceId = codeSequence.Id > 999999 ? codeSequence.Id.ToString() : codeSequence.Id.ToString().PadLeft(6, '0');
             return categoryId + sequenceId;
         }

         public void Update(Product model)
         {
             if (_db.Table.Exists<Product>(n => n.Name == model.Name && n.Id != model.Id))
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
             _db.Delete<Product>(arrIds);
         }

         public void PublishToggle(string ids, bool isPublish)
         {
             if (string.IsNullOrEmpty(ids))
             {
                 throw new Exception("id 参数为空");
             }
             var arrIds = ids.Split(',').ToIntArray();
             var products = _db.Table.Find<Product>(arrIds);
             foreach (var item in products)
             {
                 item.IsPublish = isPublish;
             }
             _db.Update<Product>(products.ToArray());
         }
    }
}
