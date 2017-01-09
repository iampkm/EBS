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
            var contracts = _db.Table.FindAll<PurchaseContract>(sql,
                 new { SupplierId = model.SupplierId, Status = PurchaseContractStatus.Cancel, Today = DateTime.Now.Date }).ToList();
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

        public void AdjustContractPrice(AdjustContractPrice entity)
        {
            if (entity.Items.Count() == 0) throw new Exception("调价明细为空");
            string sql = @" select i.* from purchasecontract 
c inner join purchasecontractitem i on c.Id = i.PurchaseContractId
where c.`Status` = 3 and FIND_IN_SET(@StoreId, c.StoreIds) and c.SupplierId=@SupplierId and i.ProductId in @ProductIds order by c.Id desc";
            var productIds = entity.Items.Select(n => n.ProductId).ToArray();
            //已存在的所有要更新合同明细
            var updateList = _db.Table.FindAll<PurchaseContractItem>(sql, 
                new { StoreId = entity.StoreId, SupplierId = entity.SupplierId, ProductIds = productIds }).ToList();
            // 该供应商最后一个合同           
            string sqlContract = "select * from purchasecontract c where c.`Status` = 3 and FIND_IN_SET(@StoreId, c.StoreIds) and c.SupplierId=@SupplierId order by c.Id desc  LIMIT 1 ";
            var contract = _db.Table.Find<PurchaseContract>(sqlContract, new { StoreId = entity.StoreId, SupplierId = entity.SupplierId });
            if (contract == null) { throw new Exception("供应商无合同，不能调价"); }
            List<PurchaseContractItem> insertList = new List<PurchaseContractItem>();
            foreach (var item in entity.Items)
            {
                // 更新明细中不存在的，就是需要添加的
                var model = updateList.FirstOrDefault(n => n.ProductId == item.ProductId);
                if (model==null)
                {
                    model = new PurchaseContractItem()
                    {
                        PurchaseContractId = contract.Id,
                        ProductId = item.ProductId,
                        ContractPrice = item.AdjustPrice,
                        Status = SupplyStatus.Supplying,
                    };
                    insertList.Add(model);
                }
                else {
                    model.ContractPrice = item.AdjustPrice;
                }
            }
            if (insertList.Count > 0)
            {
                _db.Insert<PurchaseContractItem>(insertList.ToArray());
            }
            if (updateList.Count > 0)
            {
                _db.Update<PurchaseContractItem>(updateList.ToArray());
            }
        }
    }
}
