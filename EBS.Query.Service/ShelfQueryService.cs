using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Query.DTO;
using EBS.Infrastructure.Caching;
namespace EBS.Query.Service
{
    public class ShelfQueryService : IShelfQuery
    {
        IQuery _query;
        ICacheManager _cacheService;
        public ShelfQueryService(IQuery query,ICacheManager cache)
        {
            this._query = query;
            _cacheService = cache;
        }
        public IEnumerable<DTO.ShelfTreeNode> GetShelfTree(int storeId)
        {
            //var result = _cacheService.Get<IEnumerable<ShelfTreeNode>>(CacheKeys.EBS_Shelf_All,() => {
                //货架
                var trees = new List<ShelfTreeNode>();
                //var shelfs = _query.FindAll<Shelf>(n => n.StoreId == storeId).OrderBy(n => n.Code).ToList();
                string sql = @"select s.Id,s.StoreId,s.`Code`,s.Number,s.`Name`,l.Id as LayerId,l.`Code` as ShelfLayerCode,l.Number as ShelfLayerNumber,l.ShelfId,p.Id as shelflayerproductId,p.ShelfLayerId,p.`Code` as ShelfLayerProductCode,p.Number as ShelfLayerProductNumber
 from shelf s  
left JOIN  shelflayer l on s.Id = l.ShelfId
left join shelflayerproduct p on l.Id = p.ShelfLayerId
where s.StoreId=@StoreId order by s.code ";
                var shelfs = _query.FindAll<ShelfInfoDto>(sql, new { StoreId = storeId });
                foreach (var shelf in shelfs.Where(n=>n.Code.Length==4))
                {
                    var shelfNode = new ShelfTreeNode(shelf.Id, shelf.Name, string.Format("{0}({1})", shelf.Code, shelf.Name), shelf.Code);
                    if (trees.Exists(n => n.code == shelf.Code))
                    {
                        continue; //剔除重复
                    }
                    trees.Add(shelfNode);
                    // 层
                   // var layers = _query.FindAll<ShelfLayer>(n => n.ShelfId == shelf.Id).OrderBy(n => n.Code).ToList();
                    var layers = shelfs.Where(n => n.ShelfId == shelf.Id&&n.ShelfLayerCode.Length==6).OrderBy(n => n.ShelfLayerCode).ToList();
                    foreach (var layer in layers)
                    {
                        var layerName = string.Format("{0}({1}层)", layer.ShelfLayerCode, layer.ShelfLayerNumber);
                        var layerNode = new ShelfTreeNode(layer.LayerId, layerName, layerName, layer.ShelfLayerCode);
                        if (shelfNode.children.Exists(n => n.code == layer.ShelfLayerCode))
                        {
                            continue;
                        }
                        shelfNode.children.Add(layerNode);
                        // 商品
                        //var products = _query.FindAll<ShelfLayerProduct>(n => n.ShelfLayerId == layer.Id).OrderBy(n => n.Code).ToList();
                        var products = shelfs.Where(n => n.ShelfLayerId == layer.LayerId && n.ShelfLayerProductCode.Length == 8)
                                    .OrderBy(n => n.ShelfLayerProductCode).ToList();
                        foreach (var product in products)
                        {
                            var layerProductName = string.Format("{0}({1}列)", product.ShelfLayerProductCode, product.ShelfLayerProductNumber);
                            var productNode = new ShelfTreeNode(product.ShelfLayerProductId, layerProductName, layerProductName, product.ShelfLayerProductCode);
                            if (layerNode.children.Exists(n => n.code == product.ShelfLayerProductCode))
                            {
                                continue;
                            }
                            layerNode.children.Add(productNode);
                        }
                    }

                }
                return trees;
            //});
            //return result;
        }


        public ShelfTreeNode QueryShelf(int storeId, string code)
        {
            var model = _query.Find<Shelf>(n => n.Code == code && n.StoreId == storeId);
            return new ShelfTreeNode(model.Id, model.Name, string.Format("{0}({1})", model.Code, model.Name), model.Code);
        }

        public ShelfTreeNode QueryShelfLayer(int shelfId, string code)
        {
            var model = _query.Find<ShelfLayer>(n => n.Code == code && n.ShelfId == shelfId);
            var showName = string.Format("{0}({1}层)", model.Code, model.Number);
            return new ShelfTreeNode(model.Id, showName, showName, model.Code);
        }

        public ShelfTreeNode QueryProduct(int shelfLayerId, string code)
        {
            var model = _query.Find<ShelfLayerProduct>(n => n.Code == code && n.ShelfLayerId == shelfLayerId);
            var showName = string.Format("{0}({1}列)", model.Code, model.Number);
            return new ShelfTreeNode(model.Id, showName, showName, model.Code);
        }


        public IEnumerable<PrintShelfDto> GetPrintShelfInfo(string shelfIds)
        {
            if (string.IsNullOrWhiteSpace(shelfIds)) throw new Exception("货架码不能为空");
            string sql = @"select s.Id,s.Code,s.Name,d.Id as StoreId,d.Name as StoreName from Shelf s left join Store d on s.StoreId = d.Id
where s.Id in ({0})";
            sql = string.Format(sql, shelfIds);
            var shelfs = _query.FindAll<PrintShelfDto>(sql, null);
            foreach (var shelf in shelfs)
            {
                shelf.Items = QueryShelfProduct(shelf.StoreId, shelf.Code);
            }
            return shelfs;
        }



        public IEnumerable<PrintShelfGridDto> GetShelfGridInfo(string shelfIds)
        {
            if (string.IsNullOrWhiteSpace(shelfIds)) throw new Exception("打印货架码不能为空");
            string sql = @"select s.Id,s.Code,s.Name,d.Id as StoreId,d.Name as StoreName from Shelf s left join Store d on s.StoreId = d.Id
where s.Id in ({0})";
            sql = string.Format(sql, shelfIds);
            var shelfs = _query.FindAll<PrintShelfGridDto>(sql, null);
            foreach (var shelf in shelfs)
            {
                shelf.Layers = _query.FindAll<ShelfLayer>(n => n.ShelfId == shelf.Id).Select(n => new ShelfLayerGridDto()
                {
                    Id = n.Id,
                    Code = n.Code,
                    Number = n.Number,
                    ShelfId = n.ShelfId
                }).ToList();
                foreach (var layer in shelf.Layers)
                {
                    var psql = @"select s.*,p.Name as ProductName,p.code as ProductCode,p.BarCode,p.Specification,p.SalePrice from shelflayerproduct s inner join product p on s.productId= p.id
where s.shelflayerid=@ShelfLayerId ";

                    layer.Items = _query.FindAll<ShelfLayerProductDto>(psql, new { ShelfLayerId = layer.Id });
                    if (layer.Items.Count() > shelf.MaxColumn)
                    {
                        shelf.MaxColumn = layer.Items.Count();
                    }
                }
            }
            return shelfs;
        }       

        public IEnumerable<ShelfLayerProductDto> QueryShelfProduct(int storeId, string code, string productCodeOrBarCode="", string productName="")
        {
            //var result = _cacheService.Get<IEnumerable<ShelfLayerProductDto>>(CacheKeys.EBS_Shelf_Products, () =>
            //{

                dynamic param = new ExpandoObject();
                string where = "";
                if (!string.IsNullOrEmpty(productCodeOrBarCode))
                {
                    where += "and ( p.Code=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode ) ";
                    param.ProductCodeOrBarCode = productCodeOrBarCode;
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    where += "and p.Name like @ProductName ";
                    param.ProductName = string.Format("{0}%", productName);
                }
                //门店 和货架码是必填参数
                param.StoreId = storeId;
                param.Code = code + "%";
                string psql = @"select s.*,p.Name as ProductName,p.code as ProductCode,p.BarCode,p.Specification,p.SalePrice from shelflayerproduct s inner join product p on s.productId= p.id
where s.StoreId=@StoreId and s.code like @Code " + where;
                var rows = _query.FindAll<ShelfLayerProductDto>(psql, param);
                return rows;
            //});

            //return result;
        }
    }
}
