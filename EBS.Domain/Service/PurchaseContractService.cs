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
   public class PurchaseContractService
    {
        IDBContext _db;
        public PurchaseContractService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(PurchaseContract model)
        {           
            // 同一个门店，同一个时间段内，与一个供应商，同一种经营方式只能有一个合同
            if (_db.Table.Exists<PurchaseContract>(n => n.Code == model.Code))
            {
                throw new Exception("合同编号已经存在"); 
            }
            _db.Insert(model);           
        }

        public void Update(PurchaseContract model)
        {
            if (_db.Table.Exists<PurchaseContract>(n => n.Code == model.Code && n.Id != model.Id))
            {
                throw new Exception("合同编码不能重复!");
            }
            if (_db.Table.Exists<PurchaseContractItem>(n => n.PurchaseContractId == model.Id))
            {
                _db.Delete<PurchaseContractItem>(n => n.PurchaseContractId == model.Id);
            }           
            _db.Insert<PurchaseContractItem>(model.Items.ToArray());
            _db.Update(model);
        }

        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var arrIds = ids.Split(',').ToIntArray();
            _db.Delete<PurchaseContract>(arrIds);
            //删除权限
        }
    }
}
