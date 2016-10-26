using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Query;
using Dapper.DBContext;
using EBS.Domain.Entity;
namespace EBS.Query.Service
{
    public class AreaQueryService : IAreaQuery
    {
        IQuery _query;
        public AreaQueryService(IQuery query)
        {
            this._query = query;
        }

        public IEnumerable<AreaTreeNode> GetTree()
        {
            IEnumerable<AreaTreeNode> trees = _query.FindAll<Area>().Select(n => new AreaTreeNode()
               {
                   id = n.Id,
                   pId = GetParentNodeId(n),
                   name = string.Format("[{0}] {1}", n.Id, n.Name),
                   text = n.FullName
               });

            return trees;
        }

        public string GetParentNodeId(Area node)
        {
            string parentId = null;
            switch (node.Level)
            {
                case 2:
                    parentId = node.Id.Substring(0, node.Id.Length - 4) + "0000";
                    break;
                case 3:
                    parentId = node.Id.Substring(0, node.Id.Length - 2) + "00";
                    break;
                default:
                    parentId = null;
                    break;
            }
            return parentId;
        }
    }
}
