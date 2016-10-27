using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IPurchaseContractFacade
    {
        void Create(PurchaseContractModel model);
        void Edit(PurchaseContractModel model);
        void Delete(string ids);
    }
}
