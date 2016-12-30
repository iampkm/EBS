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

        public void ValidateBarCode(string barCode)
        {
            if (string.IsNullOrEmpty(barCode))
            {
                throw new Exception("条码不能为空");
            }
            if (_db.Table.Exists<Product>(n => n.BarCode == barCode))
            {
                throw new Exception("条码已存在");
            }
        }

        //public void PublishToggle(string ids, bool isPublish)
        //{
        //    if (string.IsNullOrEmpty(ids))
        //    {
        //        throw new Exception("id 参数为空");
        //    }
        //    var arrIds = ids.Split(',').ToIntArray();
        //    var products = _db.Table.Find<Product>(arrIds);
        //    foreach (var item in products)
        //    {
        //        item.IsPublish = isPublish;
        //    }
        //    _db.Update<Product>(products.ToArray());
        //}

        public List<Product> ConvertToProduct(string productsInput)
        {
            List<Product> products = new List<Product>(3000);
            string[] productIdArray = productsInput.Trim('\n').Split('\n');
            foreach (var item in productIdArray)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (item.Contains("\t"))
                {
                    string[] columns = item.Split('\t');
                    var templength = 0m;
                    var tempwidth = 0m;
                    var tempHeight = 0m;
                    Decimal.TryParse(columns[7], out templength);
                    Decimal.TryParse(columns[8], out tempwidth);
                    Decimal.TryParse(columns[9], out tempHeight);
                    var brandId = 0;
                    int.TryParse(columns[2], out brandId);
                    var product = new Product()
                    {
                        Name = columns[0].Length > 10 ? columns[0].Substring(0, 10) : columns[0],
                        ShowName = columns[0],
                        CategoryId = columns[1],
                        BrandId = brandId,
                        BarCode = columns[3],
                        Specification = columns[4],
                        SpecificationQuantity = string.IsNullOrEmpty(columns[5]) ? "1" : columns[5],
                        MadeIn="详见商品",
                        Grade ="合格",
                        Unit = columns[6],
                        Length = templength,
                        Width = tempwidth,
                        Height = tempHeight,
                        InputRate = 17,
                        OutRate = 17,
                        CreatedOn = DateTime.Now,
                        IsGift = false
                    };
                    products.Add(product);
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
