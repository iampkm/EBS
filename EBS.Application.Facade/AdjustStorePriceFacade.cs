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
   public class AdjustStorePriceFacade:IAdjustStorePriceFacade
    {
       IDBContext _db;
        AdjustStorePriceService _service;
        ProcessHistoryService _processHistoryService;
        BillSequenceService _sequenceService;
        ProductService _productService;
        StoreInventoryService _storeInventoryService;
        public AdjustStorePriceFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new AdjustStorePriceService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
            _productService = new ProductService(this._db);
            _storeInventoryService = new StoreInventoryService(this._db);
        }
        public void Create(AdjustStorePriceModel model)
        {
            var entity = new AdjustStorePrice();
            entity = model.MapTo<AdjustStorePrice>();
            entity.AddItems(model.ConvertJsonToItem());
            entity.CreatedBy = model.UpdatedBy;
            entity.Code = _sequenceService.GenerateNewCode(BillIdentity.AdjustStorePrice);
            _db.Insert(entity);
            var reason = "创建门店调价单";
            var history = new ProcessHistory(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, BillIdentity.AdjustStorePrice.ToString(), reason);
            _db.Command.AddExecute(history.CreateSql(entity.GetType().Name, entity.Code), history);
            _db.SaveChange();
        }

        public void Edit(AdjustStorePriceModel model)
        {
            var entity = _db.Table.Find<AdjustStorePrice>(model.Id);
            entity = model.MapTo<AdjustStorePrice>(entity);
            entity.AddItems(model.ConvertJsonToItem());
            entity.UpdatedOn = DateTime.Now;
            _service.Update(entity);
            var reason = "修改门店调价单";
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, BillIdentity.AdjustStorePrice.ToString(), reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy, string editor, string reason)
        {
            var entity = _db.Table.Find<AdjustStorePrice>(id);
            entity.Cancel();
            entity.EditBy(editBy);
            _db.Update(entity);
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustStorePrice.ToString(), reason);
            _db.SaveChange();
        }


        public void Submit(int id, int editBy, string editor)
        {
           
            var entity = _db.Table.Find<AdjustStorePrice>(id);
            if (entity == null) { throw new Exception("单据不存在"); }
            //// 根据明细修改商品价格
            //var items = _db.Table.FindAll<AdjustStorePriceItem>(n => n.AdjustStorePriceId == entity.Id);
            //Dictionary<int, decimal> productSalePriceDic = new Dictionary<int, decimal>();
            //items.ToList().ForEach(n => productSalePriceDic.Add(n.ProductId, n.AdjustPrice));
            //_productService.UpdateSalePrice(productSalePriceDic);
            // 修改单据状态
            entity.Submit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "提交门店调价单";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustStorePrice.ToString(), reason);
            _db.SaveChange();

        }

        public void Audit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<AdjustStorePrice>(id);
            var  entityItems = _db.Table.FindAll<AdjustStorePriceItem>(n => n.AdjustStorePriceId == id).ToList();
            entity.AddItems(entityItems);
            entity.Audit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "审核通过";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustStorePrice.ToString(), reason);

            //修改库存门店价
            _storeInventoryService.UpdateStoreSalePrice(entity);

            _db.SaveChange();
        }


        public void Reject(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<AdjustStorePrice>(id);
            entity.Reject();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "驳回门店单据";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustStorePrice.ToString(), reason);
            _db.SaveChange();
        }
    }
}
