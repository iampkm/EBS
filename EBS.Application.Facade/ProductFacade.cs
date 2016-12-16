﻿using System;
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
using System.Diagnostics;
using EBS.Infrastructure.Extension;
namespace EBS.Application.Facade
{
    public class ProductFacade : IProductFacade
    {
        IDBContext _db;
        ProductService _productService;
        AdjustSalePriceService _adjustSalePriceService;
        ProcessHistoryService _processHistoryService;
        BillSequenceService _sequenceService;
        public ProductFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _productService = new ProductService(this._db);
            _adjustSalePriceService = new AdjustSalePriceService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
        }
        public void Create(ProductModel model)
        {
            Product entity = model.MapTo<Product>();
            entity.CreatedBy = model.UpdatedBy;
            entity.Code = _productService.GenerateNewCode(entity.CategoryId);
            _db.Insert(entity);
            // 有变价，记录变价
            if (entity.SalePrice > 0)
            {
                var newCode = _sequenceService.GenerateNewCode(BillIdentity.AdjustSalePrice);
                var adjustEntity = _adjustSalePriceService.Create(entity, model.SalePrice, newCode, model.UpdatedBy);
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
                var adjustEntity = _adjustSalePriceService.Create(entity, model.SalePrice, newCode, model.UpdatedBy);             
                _db.Insert(adjustEntity);
            }
            if (model.CategoryId != entity.CategoryId)
            {
                entity.Code = _productService.GenerateNewCode(entity.CategoryId);
            }
            entity = model.MapTo<Product>(entity);
            _db.Update(entity);
            _db.SaveChange();
        }

        public string Import(string productsIpput,int editor)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            var products = _productService.ConvertToProduct(productsIpput);
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
                product.Code = _productService.GenerateNewCode(product.CategoryId);
                successProducts.Add(product);
            }
            if (successProducts.Count == 0) {
                throw new Exception("符合导入条件的商品数为0，请检查模板格式");
            }
            _db.Insert<Product>(successProducts.ToArray());
            _db.SaveChange();
            st.Stop();
            return string.Format("导入结束。总耗时：{0}:{1}:{2}。{3}", st.Elapsed.Hours, st.Elapsed.Minutes, st.Elapsed.Seconds, errors);            
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
