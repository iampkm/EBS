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
    }
}
