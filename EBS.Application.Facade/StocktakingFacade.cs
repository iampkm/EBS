using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
using Newtonsoft.Json;
using EBS.Application.Facade.Mapping;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using Dapper.DBContext;
using EBS.Domain.Service;
namespace EBS.Application.Facade
{
   public class StocktakingFacade:IStocktakingFacade
    {
        IDBContext _db;
        BillSequenceService _billService;
        StocktakingService _stocktakingService;
        public StocktakingFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _billService = new BillSequenceService(_db);
            _stocktakingService = new StocktakingService(_db);
        }
        public void Create(StocktakingModel model)
        {
            var entity = model.MapTo<Stocktaking>();          
            entity.Status = StocktakingStatus.Audited;
            entity.StocktakingType = StocktakingType.Stocktaking;
            entity.Items = JsonConvert.DeserializeObject<List<StocktakingItem>>(model.ItemsJson);
            if (entity.Items.Sum(n => n.CountQuantity) == 0)
            {
                throw new Exception("盘点数不能都为0");
            }
            entity.Code = _billService.GenerateNewCode(BillIdentity.StoreStocktaking);
            _db.Insert(entity);
            _db.SaveChange();
        }

        public void Correct(StocktakingModel model)
        {
            var entity = model.MapTo<Stocktaking>();           
            entity.Status = StocktakingStatus.WaitAuditing;
            entity.StocktakingType = StocktakingType.StocktakingCorect;
            entity.Items = JsonConvert.DeserializeObject<List<StocktakingItem>>(model.ItemsJson);
            entity.Code = _billService.GenerateNewCode(BillIdentity.StoreStocktaking);
            _db.Insert(entity);
            _db.SaveChange();
        }

        public void Audit(int id)
        {
            var entity = _db.Table.Find<Stocktaking>(id);
            entity.Status = StocktakingStatus.Audited;
            _db.Update(entity);
            _db.SaveChange();
        }

        public void Edit(StocktakingModel model)
        {
            var entity = _db.Table.Find<Stocktaking>(model.Id);
           // entity = model.MapTo<Stocktaking>(entity);
           // entity.Status = StocktakingStatus.Audited;
           // entity.StocktakingType = StocktakingType.StocktakingCorect;
            entity.Items = JsonConvert.DeserializeObject<List<StocktakingItem>>(model.ItemsJson);
            if (entity.Items.Count > 0)
            {
                _db.Delete<StocktakingItem>(n => n.StocktakingId == entity.Id);
               // _db.Delete(entity.Items.ToArray());
                _db.Insert(entity.Items.ToArray());
            }
           // _db.Update(entity);           
            _db.SaveChange();
        }


        public void Cancel(int id)
        {
            var entity = _db.Table.Find<Stocktaking>(id);
            entity.Cancel();
            _db.Update(entity);
            _db.SaveChange();
        }
    }
}
