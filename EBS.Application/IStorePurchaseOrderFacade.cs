using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IStorePurchaseOrderFacade
    {
       void Create(CreateStorePurchaseOrder model);
       void Edit(EditStorePurchaseOrder model);
        void Delete(int id, int editBy, string editor, string reason);
        void Submit(int id, int editBy, string editor);
        void ReceivedGoods(ReceivedGoodsStorePurchaseOrder model);
        void SaveInventory(int id, int editBy, string editor);
    }
}
