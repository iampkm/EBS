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
   public class StorePurchaseOrderFacade:IStorePurchaseOrderFacade
    {
       
        IDBContext _db;
        StorePurchaseOrderService _service;
        ProcessHistoryService _processHistoryService;
        BillSequenceService _sequenceService;
        public StorePurchaseOrderFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new StorePurchaseOrderService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
        }
        public void Create(CreateStorePurchaseOrder model)
        {
            var entity = new StorePurchaseOrder();
            entity = model.MapTo<StorePurchaseOrder>();
            entity.AddItems(model.ConvertJsonToItem());
            entity.Code = _sequenceService.GenerateNewCode(BillIdentity.StorePurchaseOrder);
            _service.Create(entity);
            _db.SaveChange();
            var reason = "创建采购单";
            entity = _db.Table.Find<StorePurchaseOrder>(n => n.Code == entity.Code);
            _processHistoryService.Track(model.CreatedBy, model.CreatedByName, (int)entity.Status, entity.Id, FormType.StorePurchaseOrder, reason);
            _db.SaveChange();
        }

        public void Edit(EditStorePurchaseOrder model)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(model.Id);
            entity = model.MapTo<StorePurchaseOrder>();
            entity.AddItems(model.ConvertJsonToItem());
            _service.Update(entity);
            var reason = "修改采购单";
            _processHistoryService.Track(model.CreatedBy, model.CreatedByName, (int)entity.Status, entity.Id, FormType.StorePurchaseOrder, reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy, string editor, string reason)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(id);
            entity.Cancel();
            _db.Update(entity);
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.StorePurchaseOrder, reason);
            _db.SaveChange();
        }


        public void Submit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(id);
            entity.Submit();
            _db.Update(entity);
            var reason = "等待收货";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.StorePurchaseOrder, reason);
            _db.SaveChange();

        }

        public void ReceivedGoods(ReceivedGoodsStorePurchaseOrder model)
        {
            //修改明细数据和生产日期/保质期
            var entity = _db.Table.Find<StorePurchaseOrder>(model.Id);
            entity.ReceivedGoods();
            entity = model.MapTo<StorePurchaseOrder>();
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == model.Id).ToList();
            entity.AddItems(entityItems);
            entity.UpdateReceivedGoodsItems(model.ConvertJsonToItem());
            _service.UpdateModelAndItems(entity);
            var reason = "采购单收货";
            _processHistoryService.Track(model.ReceivedBy, model.ReceivedByName, (int)entity.Status, entity.Id, FormType.StorePurchaseOrder, reason);
            _db.SaveChange();
        }
    }
}
