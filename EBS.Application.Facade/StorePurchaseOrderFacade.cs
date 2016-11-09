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
        StoreInventoryService _storeInventoryService;
        StoreInventoryBatchService _storeBatchService;
        public StorePurchaseOrderFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new StorePurchaseOrderService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
            _storeInventoryService = new StoreInventoryService(this._db);
            _storeBatchService = new StoreInventoryBatchService(this._db);

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
            entity = model.MapTo<StorePurchaseOrder>(entity);
            entity.AddItems(model.ConvertJsonToItem());
            _service.UpdateWithItem(entity);
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
            entity = model.MapTo<StorePurchaseOrder>(entity);
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == model.Id).ToList();
            entity.SetItems(entityItems);
            var reason= entity.UpdateReceivedGoodsItems(model.ConvertJsonToItem());
            _db.Update(entity);
            _db.Update(entity.Items.ToArray());
            _processHistoryService.Track(model.ReceivedBy, model.ReceivedByName, (int)entity.Status, entity.Id, FormType.StorePurchaseOrder, reason);
            // 添加库存中不存在的商品
            _storeInventoryService.CreateProductNotInInventory(entity);
            _db.SaveChange();
        }

        public void SaveInventory(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(id);
            if (entity == null) { throw new Exception("单据不存在"); }
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == entity.Id).ToList();
            entity.SetItems(entityItems);
            entity.UpdateStatus(editBy, editor);
            _db.Update(entity);
            var reason = "入库";
           _processHistoryService.Track(entity.StoragedBy, entity.StoragedByName, (int)entity.Status, entity.Id, FormType.StorePurchaseOrder, reason);
            // 写入库存,库存历史纪录
            _storeInventoryService.StockInProducts(entity);
            // 写入库存批次记录
            _storeBatchService.SaveBatch(entity);
            // 写入商品移动平均成本价

            _db.SaveChange();
        }
    }
}
