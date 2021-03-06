﻿using System;
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
   public class SupplierService
    {
        IDBContext _db;
        public SupplierService(IDBContext dbContext)
        {
            this._db = dbContext;
        }       
        public string GenerateNewCode(int type)
        { 
            var typeCode = type.ToString()+"%";
            string sql = "select code from Supplier where code like @Code order by code desc limit 1";
            var maxModel= _db.Table.Find<Supplier>(sql, new { Code = typeCode});
             var number = 1;
            if (maxModel == null)
            {               
                return string.Format("{0}{1}", type, number.ToString().PadLeft(3, '0'));
            }
            else {
                int.TryParse(maxModel.Code.Substring(1), out number);               
                 number += 1;
                var codeNumber = number > 999 ? number.ToString() : number.ToString().PadLeft(3, '0');
                return string.Format("{0}{1}", type, codeNumber);
            }
        }
        public int[] ValidateSupplierIds(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception("id 参数为空");
            }
            var idArray = ids.Split(',').ToIntArray();
            foreach(var supplierId in idArray)
            {
                if (_db.Table.Exists<PurchaseContract>(n => n.SupplierId == supplierId))
                {
                    throw new Exception("有供应商已经被使用，不能删除");
                }
            }
            return idArray;
        }

        public SupplierProduct MarkWaitSupply(int id, int updatedBy)
        {
            var model = _db.Table.Find<SupplierProduct>(id);
            model.CompareStatus = ComparePriceStatus.Success;
            model.UpdatedBy = updatedBy;
            return model;
        }
        public SupplierProduct UnMarkWaitSupply(int id, int updatedBy)
        {
            var model = _db.Table.Find<SupplierProduct>(id);
            model.CompareStatus = ComparePriceStatus.Failed;
            model.UpdatedBy = updatedBy;
            return model;
        }

        public SupplierProduct ResetMark(int id, int updatedBy)
        {
            var model = _db.Table.Find<SupplierProduct>(id);
            model.CompareStatus = ComparePriceStatus.WaitCompare;
            model.UpdatedBy = updatedBy;
            return model;
        }

        public List<SupplierProduct> EditSupplyStatus(int purchaseContractId, int supplierId,int updatedBy)
        {
            string sql = @"select p.* from purchasecontractItem c 
inner join supplierproduct p on c.ProductId = p.ProductId
where c.purchaseContractId = @PurchaseContractId and p.SupplierId = @SupplierId ";
           var products= _db.Table.FindAll<SupplierProduct>(sql, new { PurchaseContractId = purchaseContractId, SupplierId = supplierId }).ToList();
            products.ForEach((p) => {
                p.Status = SupplierProductStatus.Supplying;
                p.CompareStatus = ComparePriceStatus.HadCompared;
                p.UpdatedBy = updatedBy;
                p.UpdatedOn = DateTime.Now;              
            });
            return products;
        }

    }
}
