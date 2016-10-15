using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IMenuFacade
    {
       void Create(MenuModel model);
       void Edit(MenuModel model);

       void Delete(string ids);
    }
}
