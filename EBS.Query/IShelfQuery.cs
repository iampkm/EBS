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

       IEnumerable<ShelfTreeNode> QueryShelfLayerProduct(int storeId, int shelfLayerId);

       /// <summary>
       /// 货架信息
       /// </summary>
       /// <param name="shelfIds"></param>
       /// <returns></returns>
       IEnumerable<PrintShelfDto> GetPrintShelfInfo(string shelfIds);
        /// <summary>
        /// 货架棚格图
        /// </summary>
        /// <param name="shelfSysNos"></param>
        /// <returns></returns>
       IEnumerable<PrintShelfGridDto> GetShelfGridInfo(string shelfIds);

       /// <summary>
       /// 查询货架商品
       /// </summary>
       /// <param name="storeId">门店</param>
       /// <param name="code">货架代码</param>
       /// <returns></returns>
       IEnumerable<ShelfLayerProductDto> QueryShelfProduct(int storeId, string code, string productCodeOrBarCode, string productName);
    }
}
