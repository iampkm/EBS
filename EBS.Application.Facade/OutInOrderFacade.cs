using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application;
using Dapper.DBContext;
using EBS.Domain.Service;
using EBS.Domain.Entity;
using EBS.Application.DTO;
using EBS.Infrastructure.Extension;
using Newtonsoft.Json;
using EBS.Application.Facade.Mapping;
using EBS.Domain.ValueObject;
using EBS.Infrastructure;
namespace EBS.Application.Facade
{
    public class OutInOrderFacade : IOutInOrderFacade
    {
        IDBContext _db;
        // OutInOrderService _service;
        BillSequenceService _sequenceService;
        StoreInventoryService _inventoryService;
        ProcessHistoryService _processHistoryService;
        OutInOrderService _service;
        public OutInOrderFacade(IDBContext dbContext)
        {
            _db = dbContext;
            // _service = new OutInOrderService(this._db);
            _sequenceService = new BillSequenceService(this._db);
            _inventoryService = new StoreInventoryService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _service = new OutInOrderService(this._db);
        }
        public void Create(OutInOrderModel model)
        {
            OutInOrder entity = model.MapTo<OutInOrder>();
            entity.CreatedBy = model.EditBy;
            entity.CreatedByName = model.EditByName;
            entity.UpdatedBy = model.EditBy;
            entity.UpdatedByName = model.EditByName;
           // entity.Code = _sequenceService.GenerateNewCode(BillIdentity.OtherInOrder);
            // 明细
            var items = JsonConvert.DeserializeObject<List<OutInOrderItem>>(model.ItemsJson);
            var orderType = _db.Table.Find<OutInOrderType>(entity.OutInOrderTypeId);
            if (orderType == null)
            {
                throw new FriendlyException("业务类别为空");
            }
            //entity.SetList(items, orderType);
            //_db.Insert(entity);

            ////跟踪记录
            //var reason = "创建其他入库单";
            //var history = new ProcessHistory(model.EditBy, model.EditByName, (int)entity.Status, entity.Id, BillIdentity.OtherInOrder.ToString(), reason);
            //_db.Command.AddExecute(history.CreateSql(entity.GetType().Name, entity.Code), history);

            //_db.SaveChange();
            //var modelEntity = _db.Table.Find<OutInOrder>(n => n.Code == entity.Code);
            //model.Id = modelEntity.Id;
            //model.Code = entity.Code;
            //entity.Id = modelEntity.Id;

            //if (model.SaveAndSubmit)
            //{
            //    Submit(model.Id, model.EditBy, model.EditByName);
            //}

            var reason = "创建其他入库单";
            var billIdentity = BillIdentity.OtherInOrder;
            if (orderType.OutInInventory == OutInInventoryType.Out)
            {
                reason = "创建其他出库单";
                billIdentity = BillIdentity.OtherOutOrder;
            }

            var entitys = _service.SplitOrderItem(entity,orderType);
            foreach (var order in entitys)
            {
                entity.Code = _sequenceService.GenerateNewCode(billIdentity);
                entity.SetItems(order.Items.ToList());
                _db.Insert(entity);
                var history = new ProcessHistory(entity.CreatedBy, entity.CreatedByName, (int)entity.Status, entity.Id, billIdentity.ToString(), reason);
                _db.Command.AddExecute(history.CreateSql(entity.GetType().Name, entity.Code), history);
                _db.SaveChange();
            }

        }


        public void Audit(int id, int editBy, string editByName)
        {
            var entity = _db.Table.Find<OutInOrder>(id);
            var entityItems = _db.Table.FindAll<OutInOrderItem>(n => n.OutInOrderId == id).ToList();
            entity.Items = entityItems;
            entity.Audit(editBy, editByName);
            var reason = "审核单据";
            _db.Update(entity);
            _processHistoryService.Track(entity.UpdatedBy, editByName, (int)entity.Status, entity.Id, BillIdentity.OtherInOrder.ToString(), reason);
            // _inventoryService.CheckIsExists(entity.Code);
            // _inventoryService.TransaferInventory(entity);
            _db.SaveChange();
        }

        public void Cancel(int id, int editBy, string editByName, string reason)
        {
            var entity = _db.Table.Find<OutInOrder>(id);
            entity.Cancel(editBy, editByName);
            _db.Update(entity);
            _processHistoryService.Track(entity.UpdatedBy, editByName, (int)entity.Status, entity.Id, BillIdentity.OtherInOrder.ToString(), reason);
            _db.SaveChange();
        }

        public void Edit(OutInOrderModel model)
        {
            var entity = _db.Table.Find<OutInOrder>(model.Id);
            entity = model.MapTo<OutInOrder>(entity);
            entity.UpdatedBy = model.EditBy;
            entity.UpdatedOn = DateTime.Now;
            _db.Update(entity);

            var items = JsonConvert.DeserializeObject<List<OutInOrderItem>>(model.ItemsJson);
            if (items.Count == 0) { throw new FriendlyException("明细为空"); }
            var orderType = _db.Table.Find<OutInOrderType>(entity.OutInOrderTypeId);
            if (orderType == null)
            {
                throw new FriendlyException("业务类别为空");
            }
            entity.AddRange(items, orderType);
            _db.Delete<OutInOrderItem>(n => n.OutInOrderId == entity.Id);
            _db.Insert(entity.Items.ToArray());
            _db.SaveChange();
        }

        public void Submit(int id, int editBy, string editByName)
        {
            var entity = _db.Table.Find<OutInOrder>(id);
            if (entity == null) { throw new Exception("单据不存在"); }
            entity.Items = _db.Table.FindAll<OutInOrderItem>(n => n.OutInOrderId == id).ToList();
            // 修改单据状态
            entity.Submit(editBy, editByName);
            _db.Update(entity);
            var reason = "提交单据";
            _processHistoryService.Track(editBy, editByName, (int)entity.Status, entity.Id, BillIdentity.OtherInOrder.ToString(), reason);
            // 如果调入库存商品不存在，创建商品信息
            var notExistsProduct = _inventoryService.CheckNotExistsProduct(entity);
            if (notExistsProduct.Count() > 0)
            {
                _db.Insert(notExistsProduct.ToArray());

            }
            _db.SaveChange();
        }

        public void Reject(int id, int editBy, string editByName)
        {
            var entity = _db.Table.Find<OutInOrder>(id);
            entity.Reject(editBy, editByName);
            _db.Update(entity);
            var reason = "驳回单据";
            _processHistoryService.Track(editBy, editByName, (int)entity.Status, entity.Id, BillIdentity.OtherInOrder.ToString(), reason);
            _db.SaveChange();
        }


        public void FinanceAudit(int id, int editBy, string editByName)
        {
            var entity = _db.Table.Find<OutInOrder>(id);
            entity.FinanceAudit(editBy, editByName);
            var reason = "财务复审";
            _db.Update(entity);
            _processHistoryService.Track(entity.UpdatedBy, editByName, (int)entity.Status, entity.Id, BillIdentity.OtherInOrder.ToString(), reason);
            _db.SaveChange();
        }

        public void CancelFinanceAudit(int id, int editBy, string editByName)
        {
            var entity = _db.Table.Find<OutInOrder>(id);
            entity.CancelFinanceAudit(editBy, editByName);
            var reason = "撤销财务复审";
            _db.Update(entity);
            _processHistoryService.Track(entity.UpdatedBy, editByName, (int)entity.Status, entity.Id, BillIdentity.OtherInOrder.ToString(), reason);
            _db.SaveChange();
        }
    }
}
