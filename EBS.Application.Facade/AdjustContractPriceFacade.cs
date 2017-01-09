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
        PurchaseContractService _contractService;
        public AdjustContractPriceFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new AdjustContractPriceService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
            _contractService = new PurchaseContractService(this._db);
        }
        public void Create(AdjustContractPriceModel model)
        {
            var entity = new AdjustContractPrice();
            entity = model.MapTo<AdjustContractPrice>();
            entity.SetItems(model.ConvertJsonToItem());
            _service.ValidateItems(entity);
            entity.CreatedBy = model.UpdatedBy;
            entity.Code = _sequenceService.GenerateNewCode(BillIdentity.AdjustContractPrice);
            _db.Insert(entity);
            // 直接调整合同价,不存在的商品，添加，存在的商品直接修改价格          
            var reason = "创建合同调价单";
            var history = new ProcessHistory(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, BillIdentity.AdjustContractPrice.ToString(), reason);
            _db.Command.AddExecute(history.CreateSql(entity.GetType().Name, entity.Code), history);

            _db.SaveChange();

        }

        public void Edit(AdjustContractPriceModel model)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(model.Id);
            entity.CheckEditStatus();
            entity = model.MapTo<AdjustContractPrice>(entity);
            entity.AddItems(model.ConvertJsonToItem());
            entity.UpdatedOn = DateTime.Now;
            _service.Update(entity);
            var reason = "修改合同调价单";
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, BillIdentity.AdjustContractPrice.ToString(), reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy, string editor, string reason)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(id);
            entity.Remark = reason;
            entity.Cancel();
            entity.EditBy(editBy);
            _db.Update(entity);
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustContractPrice.ToString(), reason);
            _db.SaveChange();
        }


        public void Submit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(id);
            entity.Submit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "提交审核";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustContractPrice.ToString(), reason);
            _db.SaveChange();

        }

        public void Audit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<AdjustContractPrice>(id);
            var entityItems = _db.Table.FindAll<AdjustContractPriceItem>(n => n.AdjustContractPriceId == id).ToList();
            entity.SetItems(entityItems);
            entity.Audit();
            entity.EditBy(editBy);
            _db.Update(entity);
            //调整合同价
            _contractService.AdjustContractPrice(entity);
            //供应商商品调价
            _service.AdjustSupplierProduct(entity);
            var reason = "审核通过";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustContractPrice.ToString(), reason);
            _db.SaveChange();
        }
    }
}
