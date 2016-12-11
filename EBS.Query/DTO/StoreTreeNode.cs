using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class StoreTreeNode
    {
        public int id { get; set; }

        public string code { get; set; }
        public string name { get; set; }
        public List<StoreTreeNode> children { get; set; }
    }
}
