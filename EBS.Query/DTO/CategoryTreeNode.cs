using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    /// <summary>
    /// ztree 节点格式
    /// </summary>
   public class CategoryTreeNode
    {
       public string id { get; set; }

       public string pId { get; set; }

       public string name { get; set; }

       public string text { get; set; }
    }
}
