using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    public class ShelfTreeNode
    {
        public ShelfTreeNode(int id, string name, string showName, string code)
        {
            this.id = id;
            this.name = name;
            this.code = code;
            this.showName = showName;
            this.children = new List<ShelfTreeNode>();
        }

        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 显示名  code + name 
        /// </summary>
        public string showName { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }

        public List<ShelfTreeNode> children { get; set; }
    }
}
