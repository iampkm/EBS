using Dapper.DBContext;
using EBS.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Service
{
   public class AdjustStorePriceService
    {
       IDBContext _db;
        public AdjustStorePriceService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void CheckExists(AdjustStorePrice model)
        {
            if (_db.Table.Exists<AdjustStorePrice>(n => n.Code == model.Code))
            {
                throw new Exception("调价单号已经存在");
            }           
        }

        public AdjustStorePrice Create(Product product, decimal adjustPrice,string code,int editBy)
        {
            AdjustStorePrice model = new AdjustStorePrice();
            model.Code = code;
            model.Status = ValueObject.AdjustStorePriceStatus.Create;
            model.CreatedBy = editBy;
            model.UpdatedBy = editBy;
            model.AddItem(product, adjustPrice);
            return model;
        }

        public void Update(AdjustStorePrice model)
        {
            if (_db.Table.Exists<AdjustStorePriceItem>(n => n.AdjustStorePriceId == model.Id))
            {
                _db.Delete<AdjustStorePriceItem>(n => n.AdjustStorePriceId == model.Id);
            }
            _db.Insert<AdjustStorePriceItem>(model.Items.ToArray());
            _db.Update(model);
        }
    }
}
