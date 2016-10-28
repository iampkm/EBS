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

        public void Create(PurchaseContract model,Dictionary<int,decimal> productPriceDic)
        {           
            // 同一个门店，同一个时间段内，与一个供应商，同一种经营方式只能有一个合同
            if (_db.Table.Exists<PurchaseContract>(n => n.SupplierId == model.SupplierId && n.Cooperate == model.Cooperate && n.StoreId == model.StoreId && (n.EndDate > model.EndDate || n.EndDate > model.StartDate)))
            {
                throw new Exception("该门店已经与该供应商在签约时间内存在相同经营性质的合同"); 
            }
            // new code
            model.GenerateNewCode();
            //add items
            var products = _db.Table.Find<ProductSku>(productPriceDic.Keys.ToArray()).ToList();
            model.AddPurchaseContractItem(products, productPriceDic);
            
            _db.Insert(model);           
        }

        public void Update(PurchaseContract model)
        {
            if (_db.Table.Exists<PurchaseContract>(n => n.Name == model.Name && n.Id != model.Id))
            {
                throw new Exception("名称重复!");
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
            _db.Delete<PurchaseContract>(arrIds);
            _db.SaveChange();
            //删除权限
        }
    }
}
