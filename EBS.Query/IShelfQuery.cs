using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IShelfQuery
    {
       IEnumerable<ShelfTreeNode> GetShelfTree(int storeId);

       ShelfTreeNode QueryShelf(int storeId, string code);

       ShelfTreeNode QueryShelfLayer(int shelfId, string code);

       ShelfTreeNode QueryProduct(int shelfLayerId, string code);
    }
}
