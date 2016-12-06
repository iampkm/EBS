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
           var shelfs= _query.FindAll<Shelf>(n => n.StoreId == storeId).OrderBy(n=>n.Code).ToList();
            foreach(var shelf in shelfs)
            {
                var shelfNode = new ShelfTreeNode(shelf.Id,shelf.Name,string.Format("{0}({1})",shelf.Code,shelf.Name),shelf.Code);
                trees.Add(shelfNode);
                 // 层
                var layers = _query.FindAll<ShelfLayer>(n=>n.ShelfId == shelf.Id).OrderBy(n => n.Code).ToList(); 
                foreach(var layer in layers)
                {
                    var layerName = string.Format("{0}({1}层)", layer.Code, layer.Number);
                     var layerNode = new ShelfTreeNode(layer.Id,layerName, layerName, layer.Code);
                    shelfNode.children.Add(layerNode);
                    // 商品
                    var products = _query.FindAll<ShelfLayerProduct>(n=>n.ShelfLayerId==layer.Id).OrderBy(n => n.Code).ToList();
                    foreach (var product in products)
                    {
                        var layerProductName = string.Format("{0}({1}列)", layer.Code, layer.Number);
                        var productNode = new ShelfTreeNode(product.Id,layerProductName, layerProductName, layer.Code);
                        layerNode.children.Add(productNode);
                    }
                }

            }

            return trees;
        }


        public ShelfTreeNode QueryShelf(int storeId, string code)
        {
            var model= _query.Find<Shelf>(n => n.Code == code && n.StoreId == storeId);
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
    }
}
