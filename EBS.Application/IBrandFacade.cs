using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EBS.Application
{
   public interface IBrandFacade
    {
        void Create(string name);
        void Edit(int id, string name);
        void Delete(string ids);
    }
}
