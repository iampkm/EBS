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
namespace EBS.Application.Facade
{
    public class ProductFacade : IProductFacade
    {
        IDBContext _db;
        ProductService _productSkuService;
        public ProductFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _productSkuService = new ProductService(this._db);
        }
        public void Create(ProductModel model)
        {
            Product entity = new Product()
            {
                Name = model.Name,
                ShowName = model.ShowName,
                SellingPoint = model.SellingPoint,
                CategoryId = model.CategoryId,
                BarCode = model.BarCode,
                BrandId = model.BrandId,
                Description = model.Description,
                Height = model.Height,
                Length = model.Length,
                Weight = model.Weight,
                Width = model.Width,
                InputRate = model.InputRate,
                OutRate = model.OutRate,
                IsGift = model.IsGift,
                IsPublish = model.IsPublish,
                Keywords = model.Keywords,
                OldPrice = model.OldtPrice,
                SalePrice = model.SalePrice,
                WholeSalePrice = model.WholeSalePrice,
                Specification = model.Specification,
                SubSkuCode = model.SubSkuCode,
                SubSkuQuantity = model.SubSkuQuantity
            };
            _productSkuService.Create(entity);
            _db.SaveChange();
        }

        public void Edit(ProductModel model)
        {
            var entity = _db.Table.Find<Product>(model.Id);
            entity.Name = model.Name;
            entity.ShowName = model.ShowName;
            entity.SellingPoint = model.SellingPoint;
            entity.CategoryId = model.CategoryId;
            entity.BarCode = model.BarCode;
            entity.BrandId = model.BrandId;
            entity.Description = model.Description;
            entity.Height = model.Height;
            entity.Length = model.Length;
            entity.Weight = model.Weight;
            entity.Width = model.Width;
            entity.InputRate = model.InputRate;
            entity.OutRate = model.OutRate;
            entity.IsGift = model.IsGift;
            entity.IsPublish = model.IsPublish;
            entity.Keywords = model.Keywords;
            entity.OldPrice = model.OldtPrice;
            entity.SalePrice = model.SalePrice;
            entity.WholeSalePrice = model.WholeSalePrice;
            entity.Specification = model.Specification;
            entity.SubSkuCode = model.SubSkuCode;
            entity.SubSkuQuantity = model.SubSkuQuantity;
            _productSkuService.Update(entity);
            _db.SaveChange();
        }


        public void PublishToggle(string ids, bool isPublish)
        {
            _productSkuService.PublishToggle(ids, isPublish);
            _db.SaveChange();
        }
    }
}
