using EBS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application
{
   public interface IOutInOrderFacade
    {
       void Create(OutInOrderModel model);
       void Edit(OutInOrderModel model);
        void Submit(int id, int editBy, string editByName);
        void Audit(int id, int editBy, string editByName);
        void Cancel(int id, int editBy, string editByName);
        void Reject(int id, int editBy, string editByName);
    }
}
