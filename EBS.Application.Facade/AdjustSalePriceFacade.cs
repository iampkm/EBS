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
   public class AdjustSalePriceFacade:IAdjustSalePriceFacade
    {
        IDBContext _db;
        AdjustSalePriceService _service;
        ProcessHistoryService _processHistoryService;
        BillSequenceService _sequenceService;
        ProductService _productService;
        public AdjustSalePriceFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new AdjustSalePriceService(this._db);
            _processHistoryService = new ProcessHistoryService(this._db);
            _sequenceService = new BillSequenceService(this._db);
            _productService = new ProductService(this._db);
        }
        /// <summary>
        /// 创建调价单就立即生效
        /// </summary>
        /// <param name="model"></param>
        public void Create(AdjustSalePriceModel model)
        {
            // 创建调价单
            var entity = new AdjustSalePrice();
            entity = model.MapTo<AdjustSalePrice>();
            var items = model.ConvertJsonToItem();
            entity.AddItems(items);
            entity.CreatedBy = model.UpdatedBy;
            entity.Code = _sequenceService.GenerateNewCode(BillIdentity.AdjustSalePrice);
            _service.Create(entity);
            // 修改商品价格
            Dictionary<int, decimal> productSalePriceDic = new Dictionary<int, decimal>();
            items.ToList().ForEach(n => productSalePriceDic.Add(n.ProductId, n.AdjustPrice));
            _productService.UpdateSalePrice(productSalePriceDic);
            // 修改单据状态
            entity.Submit();

            _db.SaveChange();
            var reason = "创建商品调价单";
            entity = _db.Table.Find<AdjustSalePrice>(n => n.Code == entity.Code);
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, BillIdentity.AdjustSalePrice.ToString(), reason);
            _db.SaveChange();
        }

        public void Edit(AdjustSalePriceModel model)
        {
            var entity = _db.Table.Find<AdjustSalePrice>(model.Id);
            entity = model.MapTo<AdjustSalePrice>(entity);
            entity.AddItems(model.ConvertJsonToItem());
            entity.UpdatedOn = DateTime.Now;
            _service.Update(entity);
            var reason = "修改商品调价单";
            _processHistoryService.Track(model.UpdatedBy, model.UpdatedByName, (int)entity.Status, entity.Id, BillIdentity.AdjustSalePrice.ToString(), reason);
            _db.SaveChange();
        }

        public void Delete(int id, int editBy, string editor, string reason)
        {
            var entity = _db.Table.Find<AdjustSalePrice>(id);
            entity.Cancel();
            entity.EditBy(editBy);
            _db.Update(entity);
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustSalePrice.ToString(), reason);
            _db.SaveChange();
        }


        public void Submit(int id, int editBy, string editor)
        {
           
            var entity = _db.Table.Find<AdjustSalePrice>(id);
            if (entity == null) { throw new Exception("单据不存在"); }
            // 根据明细修改商品价格
            var items = _db.Table.FindAll<AdjustSalePriceItem>(n => n.AdjustSalePriceId == entity.Id);
            Dictionary<int, decimal> productSalePriceDic = new Dictionary<int, decimal>();
            items.ToList().ForEach(n => productSalePriceDic.Add(n.ProductId, n.AdjustPrice));
            _productService.UpdateSalePrice(productSalePriceDic);
            // 修改单据状态
            entity.Submit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "商品价格修改生效";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustSalePrice.ToString(), reason);
            _db.SaveChange();

        }

        public void Audit(int id, int editBy, string editor)
        {
            var entity = _db.Table.Find<AdjustSalePrice>(id);
            entity.Audit();
            entity.EditBy(editBy);
            _db.Update(entity);
            var reason = "审核通过";
            _processHistoryService.Track(editBy, editor, (int)entity.Status, entity.Id, BillIdentity.AdjustSalePrice.ToString(), reason);
            _db.SaveChange();
        }
    }
}
