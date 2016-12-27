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
namespace EBS.Application.Facade
{
   public class TransferOrderFacade:ITransferOrderFacade
    {
        IDBContext _db;
        TransferOrderService _service;
        BillSequenceService _sequenceService;
        StoreInventoryService _inventoryService;
        
        public TransferOrderFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new TransferOrderService(this._db);
            _sequenceService = new BillSequenceService(this._db);
            _inventoryService = new StoreInventoryService(this._db);
        }
        public void Create(TransferOrderModel model)
        {
            TransferOrder entity = model.MapTo<TransferOrder>();
            entity.CreatedBy = model.EditBy;
            entity.UpdatedBy = model.EditBy;
            entity.Code = _sequenceService.GenerateNewCode(BillIdentity.TransferOrder);
            // 明细
            var items = JsonConvert.DeserializeObject<List<TransferOrderItem>>(model.Items);
            entity.Items = items;
            _db.Insert(entity);

            //跟踪记录
            var reason = "创建调拨单";
            var history = new ProcessHistory(model.EditBy, model.EditByName, (int)entity.Status, entity.Id, BillIdentity.TransferOrder.ToString(), reason);
            _db.Command.AddExecute(history.CreateSql(entity.GetType().Name, entity.Code), history);

            _db.SaveChange();
        }


        public void Audit(int id, int editBy, string editByName)
        {
            var entity = _db.Table.Find<TransferOrder>(id);
            var entityItems = _db.Table.FindAll<TransferOrderItem>(n => n.TransferOrderId == id).ToList();
            entity.Items = entityItems;

            _inventoryService.TransaferInventory(entity);

            _db.SaveChange();
        }

        public void Edit(TransferOrderModel model)
        {
            TransferOrder entity = _db.Table.Find<TransferOrder>(model.Id);       
            entity = model.MapTo<TransferOrder>(entity);

            entity.UpdatedBy = model.EditBy;
            entity.UpdatedOn = DateTime.Now;
            _db.Update(entity);
            _db.SaveChange();
        }
    }
}
