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

        public List<Product> ConvertToProduct(string productsInput)
        {
            List<Product> products = new List<Product>();
            string[] productIdArray = productsInput.Trim().Split('\n');
            foreach (var item in productIdArray)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (item.Contains("\t"))
                {
                    string[] columns = item.Split('\t');
                    if (!products.Exists(n => n.Name == columns[0].Trim()))
                    {
                        var product = new Product()
                        {
                            Name = columns[0].Length > 10 ? columns[0].Substring(0, 10) : columns[0],
                            ShowName = columns[0],
                            CategoryId = columns[1],
                            BrandId = Convert.ToInt32(columns[2]),
                            BarCode = columns[3],
                            Specification = columns[4],
                            SpecificationQuantity = string.IsNullOrEmpty(columns[5]) ? "1" : columns[5],
                            Unit = columns[6],
                            InputRate = 17,
                            OutRate = 17,
                            CreatedOn = DateTime.Now,
                            IsPublish = true,
                            IsGift = false
                        };
                        products.Add(product);
                    }
                }
                else
                {
                    throw new Exception("请使用excel格式导入");
                }
            }
            return products;
        }
        /// <summary>
        /// 商品Id 键值对
        /// </summary>
        /// <param name="productSalePriceDic"></param>
        public void UpdateSalePrice(Dictionary<int, decimal> productSalePriceDic)
        {
            var products = _db.Table.Find<Product>(productSalePriceDic.Keys.ToArray());
            foreach (var product in products)
            {
                if (productSalePriceDic.ContainsKey(product.Id))
                {
                    product.SalePrice = productSalePriceDic[product.Id];
                }
            }
            _db.Update<Product>(products.ToArray());
        }
       
    }
}
