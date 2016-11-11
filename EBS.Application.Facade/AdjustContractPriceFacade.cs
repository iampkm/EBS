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
   public class AdjustContractPriceFacade:IAdjustContractPriceFacade
    {
       IDBContext _db;
        AdjustContractPriceService _service;
        ProcessHistoryService _processHistoryService;
        BillSequenceService _sequenceService;
        public AdjustContractPriceFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new AdjustContractPriceService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
        }
        public void Create(AdjustContractPriceModel model)
        {
            var entity = new AdjustContractPrice();
            entity = model.MapTo<AdjustContractPrice>();
            entity.SetItems(model.ConvertJsonToItem());
            entity.CreatedBy = model.UpdatedBy;
            entity.Code = _sequenceService.GenerateNewCode(BillIdentity.AdjustContractPrice);
            _service.Create(entity);
            _db.SaveChange();
            var reason = "创建合同调价单";
            entity = _db.Table.Find<AdjustContractPrice>(n => n.Code == entity.Code);
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, FormType.AdjustContractPrice, reason);
            _db.SaveChange();
        }

        public void Edit(AdjustContractPriceModel model)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(model.Id);
            entity = model.MapTo<AdjustContractPrice>(entity);
            entity.AddItems(model.ConvertJsonToItem());
            entity.UpdatedOn = DateTime.Now;
            _service.Update(entity);
            var reason = "修改合同调价单";
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, FormType.AdjustContractPrice, reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy, string editor, string reason)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(id);
            entity.Cancel();
            entity.EditBy(editBy);
            _db.Update(entity);
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.AdjustContractPrice, reason);
            _db.SaveChange();
        }


        public void Submit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(id);
            entity.Submit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "提交审核";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.AdjustContractPrice, reason);
            _db.SaveChange();

        }

        public void Audit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(id);
            entity.Audit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "审核通过";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.AdjustContractPrice, reason);
            _db.SaveChange();
        }
    }
}
