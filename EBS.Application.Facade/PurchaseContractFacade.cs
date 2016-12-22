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
        SupplierService _supplierService;
        public PurchaseContractFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new PurchaseContractService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _supplierService = new SupplierService(this._db);
        }
        public void Create(CreatePurchaseContract model)
        {
            var entity = new PurchaseContract();
            entity = model.MapTo<PurchaseContract>();
            entity.AddPurchaseContractItem(model.ConvertJsonToPurchaseContractItem());
            entity.UpdatedBy = entity.CreatedBy;
            _service.ValidateContractCode(entity.Code);
            _service.ValidateContract(entity);
            _db.Insert(entity);
            _db.SaveChange();
            var reason = "创建合同";
            entity = _db.Table.Find<PurchaseContract>(n => n.Code == entity.Code);
            _processHistoryService.Track(model.CreatedBy, model.CreatedByName, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
        }

        public void Edit(EditPurchaseContract model)
        {
            var entity = _db.Table.Find<PurchaseContract>(model.Id);
            if (model.Code != entity.Code)
            {
                _service.ValidateContractCode(model.Code);
            }
            entity = model.MapTo<PurchaseContract>();
            entity.AddPurchaseContractItem(model.ConvertJsonToPurchaseContractItem());
            entity.UpdatedOn = DateTime.Now;
            
            _service.ValidateContract(entity);
            _db.Update(entity);
            _db.Delete<PurchaseContractItem>(n => n.PurchaseContractId == model.Id);
            _db.Insert<PurchaseContractItem>(entity.Items.ToArray());           
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
            //审核通过后，修改商品明细在比价状态中的比价状态
           var supplyProducts= _supplierService.EditSupplyStatus(entity.Id, entity.SupplierId, editBy);
           if (supplyProducts.Any())
           {
               _db.Update(supplyProducts.ToArray());
           }           
            // 记录操作流程 
            var reason = "审核通过";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, FormType.PurchaseContract, reason);
            _db.SaveChange();
        }
    }
}
