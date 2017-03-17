using EBS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application
{
   public interface ITransferOrderFacade
    {
       void Create(TransferOrderModel model);
       void Edit(TransferOrderModel model);
       void Submit(int id, int editBy, string editByName);
       void Audit(int id, int editBy, string editByName);
       void Cancel(int id, int editBy, string editByName);
       void Reject(int id, int editBy, string editByName);
    }
}
