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
    public class CategoryService
    {
        IDBContext _db;
        public CategoryService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public string Create(string parentId, string name)
        {
            // 分类 2位为一节：  10  0101  0102  010101

            if (_db.Table.Exists<Category>(n => n.Name == name))
            {
                throw new Exception(string.Format("分类{0}已经存在", name));
            }
            Category model = new Category();
            model.Name = name;
            if (!string.IsNullOrEmpty(parentId))
            {
                model.Level = parentId.Length / 2 + 1;
            }
            List<Category> nodes = new List<Category>();
            if (model.Level == 1)
            {
                model.FullName = model.Name;
                nodes = _db.Table.FindAll<Category>(n => n.Level == 1).OrderBy(n => n.Id).ToList();
            }
            else
            {
                var parentModel = _db.Table.Find<Category>(n => n.Id == parentId);
                model.FullName = string.Format("{0}-{1}", parentModel.FullName, model.Name);
                var parentIdValue = parentId + "%";
                nodes = _db.Table.FindAll<Category>(n => n.Id.Like(parentIdValue) && n.Level == model.Level).ToList();
            }

            if (model.Level == 1)
            {
                if (nodes.Count == 0)
                {
                    model.Id = "11";
                }
                else
                {
                    var maxNumber = nodes.Max(n => Convert.ToInt32(n.Id)) + 1;
                    var startNumber = 10;
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        var node = nodes[i];
                        var currentId = int.Parse(node.Id);
                        // ID 从11 开始，
                        if (startNumber + i + 1 != currentId)
                        {
                            //如果有空缺，取空缺Number
                            maxNumber = startNumber + i + 1;
                            break;
                        }
                    }

                    model.Id = maxNumber.ToString();
                }

            }
            else
            {
                if (nodes.Count == 0)
                {
                    model.Id = parentId + "01";
                }
                else
                {
                    // 默认最大值加1
                    var maxNumber = nodes.Max(n => Convert.ToInt32(n.Id.Substring(n.Id.Length - 2, 2))) + 1;
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        var node = nodes[i];
                        //取最后两位
                        var currentId = int.Parse(node.Id.Substring(node.Id.Length - 2, 2));
                        if (i + 1 != currentId)
                        {
                            //如果有空缺，取空缺Number
                            maxNumber = i+1;
                            break;
                        }
                    }
                    model.Id = parentId + maxNumber.ToString().PadLeft(2, '0');
                }
            }

            _db.Insert(model);
            return model.Id;
        }

        public void Update(string id, string name)
        {
            if (_db.Table.Exists<Category>(n => n.Name == name && n.Id != id))
            {
                throw new Exception("名称重复!");
            }
            var model = _db.Table.Find<Category>(id);
            model.FullName = model.FullName.Replace(model.Name, name);
            model.Name = name;
            _db.Update(model);
        }

        public void Delete(string id)
        {
            // var models = _db.Table.FindAll<Category>(n => n.Id.Like(id + "%"));
            var hasProduct = _db.Table.Exists<Product>(n => n.CategoryId.Like(id + "%"));
            if (hasProduct) throw new Exception("该品类及子类下有商品，不能删除");

            _db.Delete<Category>(n => n.Id.Like(id + "%"));
            // _db.Delete<Category>(models.ToArray());
        }
    }
}
