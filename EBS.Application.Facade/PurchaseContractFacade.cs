using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application;
using Dapper.DBContext;
using EBS.Domain.Service;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using EBS.Application.DTO;
using Newtonsoft.Json;
namespace EBS.Application.Facade
{
    public class PurchaseContractFacade : IPurchaseContractFacade
    {


        IDBContext _db;
        PurchaseContractService _service;
        public PurchaseContractFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new PurchaseContractService(this._db);
        }
        public void Create(CreatePurchaseContract model)
        {
            PurchaseContract entity = new PurchaseContract()
            {
                Name = model.Name,               
                Cooperate = (CooperateWay)model.Cooperate,
                SupplierId = model.SupplierId,
                Status = PurchaseContractStatus.Create,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.CreatedBy,
                Contact = model.Contact,
                StoreId = model.StoreId,
               // Items = model.Items
            };
            _service.Create(entity,model.ProductPriceDic);
            _db.SaveChange();
        }
       
        public void Edit(EditPurchaseContract model)
        {
            PurchaseContract entity = _db.Table.Find<PurchaseContract>(model.Id);
            entity.Name = model.Name;
            entity.Cooperate = Domain.ValueObject.CooperateWay.SellBySelf;
            entity.SupplierId = model.SupplierId;
            entity.Status = Domain.ValueObject.PurchaseContractStatus.Create;
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.CreatedBy = model.CreatedBy;
            entity.UpdatedBy = model.CreatedBy;
            entity.Contact = model.Contact;
            //  entity.Items = model.Items; 
            _service.Update(entity);
            _db.SaveChange();
        }

        public void Delete(string ids)
        {
            _service.Delete(ids);
            _db.SaveChange();
        }
    }
}
