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
       void Create(CreatePurchaseContract model);
       void Edit(EditPurchaseContract model);
       void Delete(int id, int editBy, string editor, string reason);
       void Submit(int id, int editBy, string editor);
       void Audit(int id, int editBy, string editor);
    }
}
