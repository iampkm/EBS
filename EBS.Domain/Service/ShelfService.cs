using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Domain.Service;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
namespace EBS.Domain.Service
{
    public class ShelfService
    {
        IDBContext _db;

        public ShelfService(IDBContext db)
        {
            _db = db;
        }

        public Shelf CreateShelf(int storeId, string name, string code)
        {
            var model = new Shelf();
            model.Code = code;
            if (string.IsNullOrWhiteSpace(model.Code))
            {
                model.Code = GenerateNewCode(storeId);
            }
            if (storeId == 0) { throw new Exception("门店不能为空"); }
            if (!model.Code.IsNumeric()) { throw new ArgumentException(string.Format("货架码{0}必须是数字", model.Code)); }
            if (model.Code.Length != 4) { throw new ArgumentException(string.Format("货架码{0}必须是4位数字", model.Code)); }
            if (_db.Table.Exists<Shelf>(n => n.StoreId == storeId && n.Code == model.Code))
            {
                throw new ArgumentException(string.Format("货架码{0}已经存在", model.Code));
            }
            model.StoreId = storeId;
            model.Name = name;
            model.Number = int.Parse(model.Code.Substring(2, 2));  //number 保持与Code 一致
            return model;
        }

        public string GenerateNewCode(int storeId)
        {
            var shelfList = _db.Table.FindAll<Shelf>(n => n.StoreId == storeId).OrderBy(n => n.Code).ToList();
            if (shelfList.Count == 0)
            {
                return "1001";
            }
            
            var newCode =Convert.ToInt32(shelfList.Last().Code)+1;
            for (int i = 0; i < shelfList.Count; i++)
            {
                var startNumber = 1000;
                if (startNumber + i + 1 != Convert.ToInt32(shelfList[i].Code))
                {
                    //如果有空缺，取空缺Number
                    newCode = startNumber+ i + 1;
                    break;
                }
            }
            if (newCode > 9999) throw new Exception("货架号已经抵达上线9999");
            return newCode.ToString();
        }

        public ShelfLayer CreateShelfLayer(int shelfId)
        {
            var shelfModel = _db.Table.Find<Shelf>(shelfId);
            if (shelfModel == null) { throw new ArgumentException(string.Format("请选择货架")); }
            var allShelfLayers = _db.Table.FindAll<ShelfLayer>(n => n.ShelfId == shelfId).OrderBy(n => n.Number).ToList();
            var model = new ShelfLayer();
            if (allShelfLayers.Count == 0)
            {
                model.Number = 1;
            }
            else
            {
                // 默认取最大值加1
                model.Number = allShelfLayers.Max(n => n.Number) + 1;
                for (int i = 0; i < allShelfLayers.Count; i++)
                {
                    if (i + 1 != allShelfLayers[i].Number)
                    {
                        //如果有空缺，取空缺Number
                        model.Number = i + 1;
                        break;
                    }
                }
            }
            model.ShelfId = shelfModel.Id;
            //新货架层编码
            model.Code = shelfModel.Code + model.Number.ToString().PadLeft(2, '0');
            return model;
        }

        public ShelfLayerProduct CreateProduct(int storeId, int shelfLayerId, string productCodeOrBarCode)
        {
            if (string.IsNullOrWhiteSpace(productCodeOrBarCode)) { throw new ArgumentException("商品编码或条码不能为空"); }
            if (shelfLayerId == 0) { throw new ArgumentException("请选择一个货架层"); }
            var product = _db.Table.Find<Product>(n => n.Code == productCodeOrBarCode || n.BarCode == productCodeOrBarCode);
            if (product == null) { throw new ArgumentException("商品不存在"); }
            var layer = _db.Table.Find<ShelfLayer>(shelfLayerId);
            if (layer == null) { throw new ArgumentException("货架层不存在"); }
            if (_db.Table.Exists<ShelfLayerProduct>(n => n.ShelfLayerId == shelfLayerId && n.ProductId == product.Id))
            {
                throw new Exception("该货架层中已经存在此商品");
            }
            var model = new ShelfLayerProduct();
            var goodsList = _db.Table.FindAll<ShelfLayerProduct>(n => n.ShelfLayerId == shelfLayerId);
            if (goodsList.Count() == 0)
            {
                model.Code = layer.Code + "01";
                model.Number = 1;
            }
            else
            {
                var lastGoods = goodsList.OrderByDescending(n => n.Number).FirstOrDefault();
                model.Number = lastGoods.Number + 1;
                model.Code = layer.Code + model.Number.ToString().PadLeft(2, '0');
            }
            model.ProductId = product.Id;
            model.ShelfLayerId = layer.Id;
            model.StoreId = storeId;
            return model;
        }

    }
}
