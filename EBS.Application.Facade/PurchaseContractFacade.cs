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
        ProcessHistoryService _processHistoryService;
        public PurchaseContractFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new PurchaseContractService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
        }
        public void Create(CreatePurchaseContract model)
        {
            PurchaseContract entity = new PurchaseContract()
            {
                Name = model.Name,  
                Code = model.Code,             
                SupplierId = model.SupplierId,
                Status = PurchaseContractStatus.Create,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.CreatedBy,
                Contact = model.Contact,
                StoreId = model.StoreId,
            };
            _service.Create(entity,model.ProductPriceDic);
            var reason = "创建合同";
            _processHistoryService.Track(model.CreatedBy, model.CreatedByName, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
        }
       
        public void Edit(EditPurchaseContract model)
        {
            var entity = _db.Table.Find<PurchaseContract>(model.Id);
            entity.Name = model.Name;
            entity.Code = model.Code;
            entity.StoreId = model.StoreId;
            entity.SupplierId = model.SupplierId;
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.UpdatedBy = model.UpdatedBy;
            entity.UpdatedOn = DateTime.Now;
            entity.Contact = model.Contact;
            _service.Update(entity,model.ProductPriceDic);
            var reason = "修改合同";
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy,string editor,string reason)
        {
            var entity = _db.Table.Find<PurchaseContract>(id);
            entity.Cancel();
            entity.EditBy(editBy);
            _db.Update(entity);
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.PurchaseContract,reason);
            _db.SaveChange();
        }


        public void Submit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<PurchaseContract>(id);
            entity.Submit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "提交审核";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
            
        }

        public void Audit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<PurchaseContract>(id);
            entity.Audit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "审核通过";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
        }
    }
}
