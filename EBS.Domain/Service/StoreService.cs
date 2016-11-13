using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
namespace EBS.Domain.Service
{
   public class StoreService
    {
        IDBContext _db;
        public StoreService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(Store model)
        {
            if (_db.Table.Exists<Store>(n => n.Name == model.Name))
            {
                throw new Exception("名称重复!");
            }
            // 生成门店编码           
            var firstAreaId = model.AreaId.Substring(0,2);
            var result= _db.Table.FindAll<Store>(n => n.AreaId.Like(firstAreaId + "%"));            
            var maxAreaIdNumber =  result.Max(n=>n.Number);
            model.GenerateNewCode(maxAreaIdNumber);
            _db.Insert(model);           
        }

        public void Update(Store model,string oldAreaId)
        {
            if (_db.Table.Exists<Store>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
            }
            if (model.AreaId != oldAreaId)
            { 
                //如果区域发生改变，重新生成新的 code
                var firstAreaId = model.AreaId.Substring(0, 2);
                var result = _db.Table.FindAll<Store>(n => n.AreaId.Like(firstAreaId + "%"));
                var maxAreaIdNumber = result.Max(n => n.Number);
                model.GenerateNewCode(maxAreaIdNumber);
            }
            _db.Update(model);
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<Store>(arrIds);
            _db.SaveChange();
            //删除权限
        }
    }
}
