using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface ISupplierFacade
    {
       void Create(SupplierModel model);
       void Edit(SupplierModel model);
        void Delete(string ids);
    }
}
