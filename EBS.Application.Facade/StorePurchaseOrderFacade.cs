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
            var reason = "创建采购单";
            var billIdentity = BillIdentity.StorePurchaseOrder;
            if (entity.OrderType == OrderType.Refund)
            {
                reason = "创建采购退单";
                billIdentity = BillIdentity.StorePurchaseRefundOrder;
            }

            var entitys = _service.SplitOrderItem(entity);
            foreach (var order in entitys)
            {
                entity.Code = _sequenceService.GenerateNewCode(billIdentity);
                entity.SetItems(order.Items.ToList());
                _db.Insert(entity);
                var history = new ProcessHistory(model.CreatedBy, model.CreatedByName, (int)entity.Status, entity.Id, billIdentity.ToString(), reason);
                _db.Command.AddExecute(history.CreateSql(entity.GetType().Name, entity.Code),history);
                _db.SaveChange();
           }
        }

        public void Edit(EditStorePurchaseOrder model)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(model.Id);
            entity = model.MapTo<StorePurchaseOrder>(entity);
            entity.AddItems(model.ConvertJsonToItem());
            _service.UpdateWithItem(entity);
            var reason = "修改采购单";
            var billIdentity = BillIdentity.StorePurchaseOrder;
            if (entity.OrderType == OrderType.Refund)
            {
                reason = "修改采购退单";
                billIdentity = BillIdentity.StorePurchaseRefundOrder;
            }
            _processHistoryService.Track(model.CreatedBy, model.CreatedByName, (int)entity.Status, entity.Id, billIdentity.ToString(), reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy, string editor, string reason)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(id);
            entity.Cancel();
            _db.Update(entity);
            var billIdentity = BillIdentity.StorePurchaseOrder;
            if (entity.OrderType == OrderType.Refund)
            {
                billIdentity = BillIdentity.StorePurchaseRefundOrder;
            }
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, billIdentity.ToString(), reason);
            _db.SaveChange();
        }


        public void FinanceAuditd(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(id);
            entity.FinanceAuditd(editBy,editor);
            _db.Update(entity);
            var reason = "财务已审";
            var billIdentity = BillIdentity.StorePurchaseOrder;
            if (entity.OrderType == OrderType.Refund)
            {
                billIdentity = BillIdentity.StorePurchaseRefundOrder;
            }
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, billIdentity.ToString(), reason);
            _db.SaveChange();

        }
        /// <summary>
        /// 收货
        /// </summary>
        /// <param name="model"></param>
        public void ReceivedGoods(ReceivedGoodsStorePurchaseOrder model)
        {
            //修改明细数据和生产日期/保质期
            var entity = _db.Table.Find<StorePurchaseOrder>(model.Id);
            entity = model.MapTo<StorePurchaseOrder>(entity);            
            entity.ReceivedGoods(model.ReceivedBy, model.ReceivedByName); 
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == model.Id).ToList();
            entity.SetItems(entityItems);
            entity.UpdateReceivedGoodsItems(model.ConvertJsonToItem());
            _db.Update(entity);
            _db.Update(entity.Items.ToArray());
            var reason = "保存本次收货";
            var billIdentity = BillIdentity.StorePurchaseOrder;
            if (entity.OrderType == OrderType.Refund)
            {
                reason = "保存本次退货";
                billIdentity = BillIdentity.StorePurchaseRefundOrder;
            }
            _processHistoryService.Track(model.ReceivedBy, model.ReceivedByName, (int)entity.Status, entity.Id, billIdentity.ToString(), reason);
            // 添加库存中不存在的商品
            var notExistsInventorys= _storeInventoryService.CheckProductNotInInventory(entity);
            if (notExistsInventorys.Count() > 0)
            {
                _db.Insert<StoreInventory>(notExistsInventorys.ToArray());
            }     
            _db.SaveChange();
        }
       /// <summary>
       /// 入库
       /// </summary>
       /// <param name="id"></param>
       /// <param name="editBy"></param>
       /// <param name="editor"></param>
        public void SaveInventory(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(id);
            if (entity == null) { throw new Exception("单据不存在"); }
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == entity.Id).ToList();
            entity.SetItems(entityItems);
            entity.Finished(editBy, editor);
            _db.Update(entity);
            var reason = "入库";
           _processHistoryService.Track(entity.StoragedBy, entity.StoragedByName, (int)entity.Status, entity.Id, BillIdentity.StorePurchaseOrder.ToString(), reason);
            
            // 写入库存,库存历史纪录
            _storeInventoryService.StockInProducts(entity);
            _db.SaveChange();
        }

        public void GetOutOfInventory(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<StorePurchaseOrder>(id);
            if (entity == null) { throw new Exception("单据不存在"); }
            var entityItems = _db.Table.FindAll<StorePurchaseOrderItem>(n => n.StorePurchaseOrderId == entity.Id).ToList();
            entity.SetItems(entityItems);
            entity.Finished(editBy, editor);
            _db.Update(entity);
            var reason = "出库";
            _processHistoryService.Track(entity.StoragedBy, entity.StoragedByName, (int)entity.Status, entity.Id, BillIdentity.StorePurchaseRefundOrder.ToString(), reason);
            //扣减库存，并记录库存流水
            _storeInventoryService.StockOutInventory(entity);
            _db.SaveChange();

        }
    }
}
