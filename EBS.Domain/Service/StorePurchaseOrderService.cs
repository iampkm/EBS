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
   public class StorePurchaseOrderService
    {
        IDBContext _db;
        public StorePurchaseOrderService(IDBContext dbContext)
        {
            this._db = dbContext;
        }
        public void Create(StorePurchaseOrder model, Dictionary<int, decimal> productPriceDic)
        {
          
            if (_db.Table.Exists<StorePurchaseOrder>(n => n.Code == model.Code))
            {
                throw new Exception("合同编号已经存在");
            }
            //var products = _db.Table.Find<ProductSku>(productPriceDic.Keys.ToArray()).ToList();
            //model.AddPurchaseContractItem(products, productPriceDic);

            _db.Insert(model);
        }

        public void Update(StorePurchaseOrder model, Dictionary<int, decimal> productPriceDic)
        {
            if (_db.Table.Exists<StorePurchaseOrder>(n => n.Code == model.Code && n.Id != model.Id))
            {
                throw new Exception("合同编码不能重复!");
            }
            _db.Delete<PurchaseContractItem>(n => n.PurchaseContractId == model.Id);
            var products = _db.Table.Find<Product>(productPriceDic.Keys.ToArray()).ToList();
            //model.AddPurchaseContractItem(products, productPriceDic);
            //_db.Insert<PurchaseContractItem>(model.Items.ToArray());
            _db.Update(model);
        }
    }
}
