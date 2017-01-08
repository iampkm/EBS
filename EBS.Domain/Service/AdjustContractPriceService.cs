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
   public class AdjustContractPriceService
    {
       IDBContext _db;
        public AdjustContractPriceService(IDBContext dbContext)
        {
            this._db = dbContext;
        }
        public void ValidateItems(AdjustContractPrice model)
        {
            if (model.Items.Count() == 0)
            {
                throw new Exception("明细不能为空");
            }
        }

        public void Update(AdjustContractPrice model)
        {

            ValidateItems(model);

            if (_db.Table.Exists<AdjustContractPriceItem>(n => n.AdjustContractPriceId == model.Id))
            {
                _db.Delete<AdjustContractPriceItem>(n => n.AdjustContractPriceId == model.Id);
            }           
            _db.Insert<AdjustContractPriceItem>(model.Items.ToArray());
            _db.Update(model);
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<AdjustContractPrice>(arrIds);
            //删除权限
        }
    }
}
