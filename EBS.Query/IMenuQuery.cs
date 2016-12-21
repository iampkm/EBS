using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Domain.Entity;
namespace EBS.Query
{
    public interface IMenuQuery
    {
        IEnumerable<Menu> GetList(Pager page, string name);

        IList<Menu> LoadMenuTree();
        IList<Menu> LoadMenuTree(int roleId);

        IEnumerable<Menu> LoadMenu(int roleId);
    }
}
