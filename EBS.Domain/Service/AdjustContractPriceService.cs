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

        /// <summary>
        /// 调整供应商商品价格
        /// </summary>
        /// <param name="entity"></param>
        public void AdjustSupplierProduct(AdjustContractPrice entity)
        {
            var supplierProducts = _db.Table.FindAll<SupplierProduct>(n => n.SupplierId == entity.SupplierId).ToList();
            List<SupplierProduct> insertList = new List<SupplierProduct>();
            List<SupplierProduct> updateList = new List<SupplierProduct>();
            foreach (var item in entity.Items)
            {
                var model = supplierProducts.FirstOrDefault(n => n.ProductId == item.ProductId);
                if (model == null)
                {
                    SupplierProduct newProduct = new SupplierProduct()
                    {
                        ProductId = item.ProductId,
                        SupplierId = entity.SupplierId,
                        Price = item.AdjustPrice,
                        UpdatedBy = entity.UpdatedBy,
                        Status = ValueObject.SupplierProductStatus.Supplying

                    };
                    insertList.Add(newProduct);
                }
                else
                {
                    model.Price = item.AdjustPrice;
                    updateList.Add(model);
                }
            }
            if (insertList.Count > 0)
            {
                _db.Insert<SupplierProduct>(insertList.ToArray());
            }
            if (updateList.Count > 0)
            {
                _db.Update<SupplierProduct>(updateList.ToArray());
            }

        }
    }
}
