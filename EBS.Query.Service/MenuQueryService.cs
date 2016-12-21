using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
namespace EBS.Query.Service
{
    public class MenuQueryService : IMenuQuery
    {
        IQuery _query;
        public MenuQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<Menu> GetList(Pager page, string name)
        {
            IEnumerable<Menu> rows;
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (page.IsPaging)
            {
                rows = this._query.FindPage<Menu>(page.PageIndex, page.PageSize).Where<Menu>(where, param);
                page.Total = this._query.Count<Menu>(where, param);
            }
            else
            {
                rows = this._query.FindAll<Menu>();
                page.Total = this._query.Count<Menu>();
            }
            return rows;
        }       

        public IList<Menu> LoadMenuTree()
        {
            // 根据当前角色对应权限菜单
            IEnumerable<Menu> rows = this._query.FindAll<Menu>();
            // 转化为树形结构
            List<Menu> oneMenus = rows.Where(n => n.ParentId == 0).OrderBy(n => n.DisplayOrder).ToList();
            List<Menu> tree = new List<Menu>();
            foreach (var item in oneMenus)
            {
                tree.Add(item);
                LoadChildren(tree, item, rows);
            }
            return tree;
        }
        public IList<Menu> LoadMenuTree(int roleId)
        {
            // 根据当前角色对应权限菜单
            IEnumerable<Menu> rows = LoadMenu(roleId);
            // 转化为树形结构
            List<Menu> oneMenus = rows.Where(n => n.ParentId == 0).OrderBy(n => n.DisplayOrder).ToList();
            List<Menu> tree = new List<Menu>();
            foreach (var item in oneMenus)
            {
                tree.Add(item);
                LoadChildren(tree, item, rows);
            }
            return tree;
        }

        private void LoadChildren(List<Menu> tree, Menu parent, IEnumerable<Menu> data)
        {
            var childrenMenus = data.Where(n => n.ParentId == parent.Id).OrderBy(n => n.DisplayOrder).ToList();
            foreach (var child in childrenMenus)
            {
                tree.Add(child);
                LoadChildren(tree, child, data);
            }
        }


        public IEnumerable<Menu> LoadMenu(int roleId)
        {
            string sql = @"select m.* from menu m inner JOIN rolemenu r on m.Id =  r.MenuId
where r.RoleId = @RoleId";
            var rows= _query.FindAll<Menu>(sql, new { RoleId = roleId });
            return rows;
        }
    }
}
