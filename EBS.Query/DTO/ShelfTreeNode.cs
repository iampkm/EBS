using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class ShelfTreeNode
    {
       public ShelfTreeNode(int id, int pId, string name, string code)
       {
           this.id = id;
           this.pId = pId;
           this.name = name;
           this.code = code;
       }

        public int id { get; set; }

        public int pId { get; set; }
        /// <summary>
        ///  code + name 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
    }
}
