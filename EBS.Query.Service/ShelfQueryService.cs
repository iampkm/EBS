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
namespace EBS.Query.Service
{
    public class ShelfQueryService : IShelfQuery
    {
        IQuery _query;
        public ShelfQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<DTO.ShelfTreeNode> GetShelfTree(int storeId)
        {
            //货架
            List<ShelfTreeNode> trees = new List<ShelfTreeNode>();
           var shelfs= _query.FindAll<Shelf>(n => n.StoreId == storeId);
            foreach(var shelf in shelfs)
            {
                var shelfNode = new ShelfTreeNode(shelf.Id,0,string.Format("{0}({1})",shelf.Code,shelf.Name),shelf.Code);
                trees.Add(shelfNode);
                 // 层
                var layers = _query.FindAll<ShelfLayer>(n=>n.ShelfId == shelf.Id);
                foreach(var layer in layers)
                {
                     var layerNode = new ShelfTreeNode(layer.Id,shelf.Id,string.Format("{0}({1}层)",layer.Code,layer.Number),layer.Code);
                     trees.Add(layerNode);
                    // 商品
                    var products = _query.FindAll<ShelfLayerProduct>(n=>n.ShelfLayerId==layer.Id);
                    foreach (var product in products)
                    {
                        var productNode = new ShelfTreeNode(product.Id, layer.Id, string.Format("{0}({1}列)", layer.Code, layer.Number), layer.Code);
                        trees.Add(productNode);
                    }
                }

            }

            return trees;
        }


        public ShelfTreeNode QueryShelf(int storeId, string code)
        {
            var model= _query.Find<Shelf>(n => n.Code == code && n.StoreId == storeId);
            return new ShelfTreeNode(model.Id, 0, string.Format("{0}({1})", model.Code, model.Name), model.Code);
        }

        public ShelfTreeNode QueryShelfLayer(int shelfId, string code)
        {
            var model = _query.Find<ShelfLayer>(n => n.Code == code && n.ShelfId == shelfId);
            return new ShelfTreeNode(model.Id, 0, string.Format("{0}({1}层)", model.Code, model.Number), model.Code);
        }

        public ShelfTreeNode QueryProduct(int shelfLayerId, string code)
        {
            var model = _query.Find<ShelfLayerProduct>(n => n.Code == code && n.ShelfLayerId == shelfLayerId);
            return new ShelfTreeNode(model.Id, 0, string.Format("{0}({1}列)", model.Code, model.Number), model.Code);
        }
    }
}
