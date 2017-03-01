using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IShelfFacade
    {
       string CreateShelf(int storeId, string name, string code);

       string CreateShelfLayer(int shelfId);
        /// <summary>
        /// 添加商品，返回添加的商品货架代码
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="shelfLayerId"></param>
        /// <param name="productCodeOrBarCode"></param>
        /// <param name="shelfProductId"></param>
        /// <returns></returns>
        string CreatePorduct(int storeId, int shelfLayerId, string productCodeOrBarCode, int shelfProductId);

        void EditShelf(int id, string name);

        void DeleteAll(int id, string code);

        void DeleteProduct(int shelfLayerId, string ids);


    }
}
