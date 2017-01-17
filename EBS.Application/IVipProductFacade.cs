using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IVipProductFacade
    {
       void Create(VipProductModel model);

       void Edit(VipProductModel model);
    }
}
