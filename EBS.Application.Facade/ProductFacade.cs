using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
using EBS.Application;
using EBS.Domain.Entity;
using Dapper.DBContext;
using EBS.Domain.Service;
using EBS.Domain.ValueObject;
using EBS.Application.Facade.Mapping;
namespace EBS.Application.Facade
{
    public class ProductFacade : IProductFacade
    {
        IDBContext _db;
        ProductService _productSkuService;
        AdjustSalePriceService _adjustSalePriceService;
        ProcessHistoryService _processHistoryService;
        BillSequenceService _sequenceService;
        public ProductFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _productSkuService = new ProductService(this._db);
            _adjustSalePriceService = new AdjustSalePriceService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
        }
        public void Create(ProductModel model)
        {
            Product entity = model.MapTo<Product>();
            _productSkuService.Create(entity);
            // 有变价，记录变价
            if (entity.SalePrice > 0)
            {
                var newCode = _sequenceService.GenerateNewCode(BillIdentity.AdjustSalePrice);
                var adjustEntity = _adjustSalePriceService.Create(entity, model.SalePrice, newCode, 0, "");
                _db.Insert(adjustEntity);
            }
            _db.SaveChange();
        }

        public void Edit(ProductModel model)
        {
            var entity = _db.Table.Find<Product>(model.Id);
            // 有变价，记录变价
            if (model.SalePrice != entity.SalePrice)
            {
                var newCode = _sequenceService.GenerateNewCode(BillIdentity.AdjustSalePrice);
                var adjustEntity = _adjustSalePriceService.Create(entity, model.SalePrice, newCode, 0, "");             
                _db.Insert(adjustEntity);

            }
            entity = model.MapTo<Product>();
            _productSkuService.Update(entity);
            _db.SaveChange();
        }


        public void PublishToggle(string ids, bool isPublish)
        {
            _productSkuService.PublishToggle(ids, isPublish);
            _db.SaveChange();
        }

        public string Import(string productsIpput)
        {
            var products = _productSkuService.ConvertToProduct(productsIpput);
            string errors = "";
            var successProducts = new List<Product>();
            foreach (var product in products)
            {
                if (!_db.Table.Exists<Category>(n => n.Id == product.CategoryId))
                {
                    errors += string.Format("[{0}] 品类[{1}]错误 <br />", product.Name, product.CategoryId);
                    continue;
                }
                if (!_db.Table.Exists<Brand>(n => n.Id == product.BrandId))
                {
                    errors += string.Format("[{0}] 品牌ID[{1}]错误 <br />", product.Name, product.BrandId);
                    continue;
                }
                product.Code = _productSkuService.GenerateNewCode(product.CategoryId);
                successProducts.Add(product);
            }
            _db.Insert<Product>(successProducts.ToArray());
            _db.SaveChange();
            return errors == "" ? "导入成功" : errors;
        }

        private Dictionary<string, decimal> GetProductDic(string productIds)
        {
            Dictionary<string, decimal> dicProductPrice = new Dictionary<string, decimal>(1000);
            string[] productIdArray = productIds.Split('\n');
            foreach (var item in productIdArray)
            {
                if (item.Contains("\t"))
                {
                    string[] parentIDAndQuantity = item.Split('\t');
                    if (!dicProductPrice.ContainsKey(parentIDAndQuantity[0].Trim()))
                    {
                        dicProductPrice.Add(parentIDAndQuantity[0].Trim(), decimal.Parse(parentIDAndQuantity[1]));
                    }
                }
                else
                {
                    if (!dicProductPrice.ContainsKey(item.Trim()))
                    {
                        dicProductPrice.Add(item.Trim(), 0);
                    }
                }
            }

            return dicProductPrice;
        }
    }
}
