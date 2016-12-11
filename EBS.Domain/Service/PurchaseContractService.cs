using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Infrastructure.Extension;
using EBS.Domain.ValueObject;

namespace EBS.Domain.Service
{
   public class PurchaseContractService
    {
        IDBContext _db;
        public PurchaseContractService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void ValidateContract(PurchaseContract model)
        {           
            
            // 验证 同一个门店，同一个时间段内，与一个供应商，只能有一个合同
            string sql = @"select * from PurchaseContract where SupplierId=@SupplierId and Status>@Status 
and StartDate<=@Today and EndDate>=@Today";
           var contracts= _db.Table.FindAll<PurchaseContract>(sql, 
                new { SupplierId=model.SupplierId,Status=PurchaseContractStatus.Cancel,Today=DateTime.Now.Date }).ToList();
            var storeArray = model.StoreIds.Split(',');
            foreach (var storeId in storeArray)
            {
                if (contracts.Exists(n => n.GetStores().Contains(storeId)))
                {
                    throw new Exception("门店与该供应商已经签有合同");
                }
            }    
        }

        public void ValidateContractCode(string code)
        {
            // 验证编码重复
            if (_db.Table.Exists<PurchaseContract>(n => n.Code == code))
            {
                throw new Exception("合同编号已经存在");
            }
        }
    }
}
