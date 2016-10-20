using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Query.DTO;
namespace EBS.Query.Service
{
    public class CategoryQueryService : ICategoryQuery
    {

        IQuery _query;
        public CategoryQueryService(IQuery query)
        {
            this._query = query;
        }

        IEnumerable<CategoryTreeNode> ICategoryQuery.GetCategoryTree()
        {
            var categories = _query.FindAll<Category>().Select(n => new CategoryTreeNode()
            {
                id = n.Id,
                pId = n.Level == 1 ? null : n.Id.Substring(0, n.Id.Length - 2),
                name = string.Format("[{0}] {1}",n.Id,n.Name), 
                text = n.Name
            });
            return categories;
        }
    }
}
