using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
using Dapper.DBContext;
using EBS.Domain.Service;
using EBS.Application.Facade.Mapping;
using EBS.Domain.Entity;
namespace EBS.Application.Facade
{
   public class StocktakingPlanFacade:IStocktakingPlanFacade
    {
        IDBContext _db;
        StocktakingPlanService _service;

        public StocktakingPlanFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new StocktakingPlanService(_db);
        }
        public void Create(StocktakingPlanModel model)
        {
            var entity = model.MapTo<StocktakingPlan>();
            entity.CreateBy = model.EditedBy;
            entity.CreateByName = model.Editor;
            entity.Status = Domain.ValueObject.StocktakingPlanStatus.FirstInventory;

            entity.GenerateNewCode();
            _service.ValidatePlan(entity);

            _db.Insert(entity);
            _db.SaveChange();
        }

        public void Edit(StocktakingPlanModel model)
        {
            throw new NotImplementedException();
        }
    }
}
