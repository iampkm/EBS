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
using EBS.Infrastructure.Extension;
namespace EBS.Query.Service
{
   public class StoreQueryService:IStoreQuery
    {
       IQuery _query;
       public StoreQueryService(IQuery query)
        {
            this._query = query;
        }
       public IEnumerable<StoreDto> GetPageList(Pager page, string name,string code, string canViewStores)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (!string.IsNullOrEmpty(code))
            {
                where += "and t0.Code like @Code ";
                param.Code = string.Format("{0}%", code);
            }
            if (!string.IsNullOrEmpty(canViewStores))
            {
                where += "and t0.Id in @CanViewStore ";
                param.CanViewStore = canViewStores.Split(',').ToIntArray();
            }
            string sql = "select t0.*,t1.FullName from Store t0 left join Area t1 on t0.AreaId=t1.Id where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StoreDto>(sql, param);
            page.Total = this._query.Count<Store>(where, param);
            
            return rows;
        }

        public IEnumerable<StoreTreeNode> LoadStore()
        {
            // 查询一级区域
            var areaRows = this._query.FindAll<Area>(n => n.Level == 1);
            var stores = this._query.FindAll<Store>();
            // 组装ztree树形结构
            List<StoreTreeNode> list = new List<StoreTreeNode>();
            foreach (var area in areaRows)
            {                
                // 找当前区域门店
                var areaID = area.Id.Substring(0, 2);
                var areaStores = stores.Where(n => n.AreaId.IndexOf(areaID)==0);
                //只加载有门店的区域
                if (!areaStores.Any()) { continue;  }
                List<StoreTreeNode> storelist = new List<StoreTreeNode>();
                foreach (var store in areaStores)
                {
                    var secondLayer = new StoreTreeNode()
                    {
                        id = store.Id,
                        code = store.Code,
                        name = store.Name
                    };
                    storelist.Add(secondLayer);
                }
                var firtLayer = new StoreTreeNode()
                {
                    id = Convert.ToInt32(area.Id),
                    code = area.FullName,
                    name = area.Name
                };
                firtLayer.children = storelist;
                list.Add(firtLayer);
            }
            return list;
        }
    }
}
