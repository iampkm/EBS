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
   public class AdjustSalePriceService
    {
        IDBContext _db;
        public AdjustSalePriceService(IDBContext dbContext)
        {
            this._db = dbContext;
        }

        public void Create(AdjustSalePrice model)
        {
            if (_db.Table.Exists<AdjustSalePrice>(n => n.Code == model.Code))
            {
                throw new Exception("调价单号已经存在");
            }
            _db.Insert(model);
        }

        public AdjustSalePrice Create(Product product, decimal adjustPrice,string code,int editBy,string editor)
        {
            AdjustSalePrice model = new AdjustSalePrice();
            model.Code = code;
            model.Status = ValueObject.AdjustSalePriceStatus.Valid;
            model.CreatedBy = editBy;
            model.UpdatedBy = editBy;
            model.AddItem(product, adjustPrice);
            return model;
        }

        public void Update(AdjustSalePrice model)
        {
            if (_db.Table.Exists<AdjustSalePrice>(n => n.Code == model.Code && n.Id != model.Id))
            {
                throw new Exception("调价单号不能重复!");
            }
            if (_db.Table.Exists<AdjustSalePriceItem>(n => n.AdjustSalePriceId == model.Id))
            {
                _db.Delete<AdjustSalePriceItem>(n => n.AdjustSalePriceId == model.Id);
            }
            _db.Insert<AdjustSalePriceItem>(model.Items.ToArray());
            _db.Update(model);
        }


    }
}
