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

       string CreateProduct(int storeId, int shelfLayerId, string productCodeOrBarCode);
       /// <summary>
       /// 插入商品
       /// </summary>
       /// <param name="productCodeOrBarCode">插入的商品编码或条码</param>
       /// <param name="shelfProductId">插入位置商品</param>
       /// <returns></returns>
       string InsertBefore(string productCodeOrBarCode, int shelfProductId);

        void EditShelf(int id, string name);

        void DeleteAll(int id, string code);


    }
}
