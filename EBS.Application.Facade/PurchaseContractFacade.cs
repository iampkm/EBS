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
using EBS.Application.Facade.Mapping;
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
            var entity = new PurchaseContract();
            entity = model.MapTo<PurchaseContract>();
            entity.AddPurchaseContractItem(model.ConvertJsonToPurchaseContractItem());
            entity.UpdatedBy = entity.CreatedBy;
            _service.Create(entity);
            var reason = "创建合同";
            _processHistoryService.Track(model.CreatedBy, model.CreatedByName, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
        }

        public void Edit(EditPurchaseContract model)
        {
            var entity = _db.Table.Find<PurchaseContract>(model.Id);
            entity = model.MapTo<PurchaseContract>();
            entity.AddPurchaseContractItem(model.ConvertJsonToPurchaseContractItem());
            entity.UpdatedOn = DateTime.Now;
            _service.Update(entity);
            var reason = "修改合同";
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy, string editor, string reason)
        {
            var entity = _db.Table.Find<PurchaseContract>(id);
            entity.Cancel();
            entity.EditBy(editBy);
            _db.Update(entity);
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
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
