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
    }
}
