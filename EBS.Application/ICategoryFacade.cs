using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application
{
   public interface ICategoryFacade
    {  
       /// <summary>
       /// 返回新增节点Id
       /// </summary>
       /// <param name="parentId"></param>
       /// <param name="name"></param>
       /// <returns></returns>
       string Create(string parentId, string name);

       void Edit(string id, string name);

       void Delete(string id);
    }
}
