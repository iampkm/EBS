using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IRoleFacade
    {
       void Create(RoleModel model);
       void Edit(RoleModel model);

       void Delete(string ids);


    }
}
