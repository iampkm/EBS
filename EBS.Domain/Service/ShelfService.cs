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
           var shelfList = _db.Table.FindAll<Shelf>(n => n.StoreId == storeId).ToList();
           if (shelfList.Count == 0)
           {
               return "1001";
           }
           // 后两位
           int lastTwoNumber = shelfList.Max(n => n.Number) + 1;
           for (int i = 0; i < shelfList.Count; i++)
           {
               if (i + 1 != shelfList[i].Number)
               {
                   //如果有空缺，取空缺Number
                   lastTwoNumber = i + 1;
                   break;
               }
           }

           var lastShelf = shelfList.OrderByDescending(n => n.Number).FirstOrDefault();
           if (string.IsNullOrWhiteSpace(lastShelf.Code))
           {
               return string.Format("{0}{1}", "10", lastTwoNumber.ToString().PadLeft(2, '0'));
           }
           //后两位超过99进位
           int firstTwoNumber = Convert.ToInt32(lastShelf.Code.Substring(0, 2));
           if (lastTwoNumber > 99)
           {
               lastTwoNumber = 1;
               firstTwoNumber = Convert.ToInt32(lastShelf.Code.Substring(0, 2)) + 1;
           }
           if (firstTwoNumber > 99) throw new Exception("货架码头两位不能超过99");
           return string.Format("{0}{1}", firstTwoNumber.ToString().PadLeft(2, '0'), lastTwoNumber.ToString().PadLeft(2, '0'));
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
