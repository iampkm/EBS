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
using EBS.Domain.ValueObject;
namespace EBS.Application.Facade
{
   public class StocktakingPlanFacade:IStocktakingPlanFacade
    {
        IDBContext _db;
        StocktakingPlanService _service;
        StocktakingService _stocktakingService;
        BillSequenceService _billService;
        StoreInventoryService _inventoryService;
        public StocktakingPlanFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _service = new StocktakingPlanService(_db);
            _billService = new BillSequenceService(_db);
            _inventoryService = new StoreInventoryService(_db);
            _stocktakingService = new StocktakingService(_db);
        }
        public void Create(StocktakingPlanModel model)
        {
            var entity = model.MapTo<StocktakingPlan>();
            entity.CreatedBy = model.EditedBy;
            entity.CreatedByName = model.Editor;
            entity.UpdatedBy = model.EditedBy;
            entity.UpdatedByName = model.Editor;
            entity.Code = _billService.GenerateNewCode(BillIdentity.StoreStocktakingPlan);
            _service.ValidatePlan(entity);
            _service.ValidatePlanDate(entity);
            _db.Insert(entity);
            _db.SaveChange();
        }

        public void Edit(StocktakingPlanModel model)
        {
            var entity = _db.Table.Find<StocktakingPlan>(model.Id);
            var oldStocktakingDate = entity.StocktakingDate;
            entity = model.MapTo<StocktakingPlan>(entity);
            entity.UpdatedBy = model.EditedBy;
            entity.UpdatedByName = model.Editor;
            entity.UpdatedOn = DateTime.Now;
            _service.ValidatePlan(entity);
            if (oldStocktakingDate != entity.StocktakingDate)
            {
                _service.ValidatePlanDate(entity);
            }
            _db.Update(entity);
            _db.SaveChange();
        }

        public void StartPlan(int id,int editedBy,string editor)
        {
            var entity = _db.Table.Find<StocktakingPlan>(id);
            _service.AddInventoryItems(entity);
            entity.StartPlan(editedBy,editor);
            _db.Update(entity);
            _db.SaveChange();

        }

        public void MergeDetial(int id, int editedBy, string editor)
        {
            var entity = _db.Table.Find<StocktakingPlan>(id);
            _stocktakingService.CheckWaittingAuditCorrect(id);
            _service.MergeDetial(id);
            entity.ChangeReplayStatus(editedBy, editor);
            _db.Update(entity);
            _db.SaveChange();
        }

        public void EndPlan(int id, int editedBy, string editor, string loginPassword)
        {           
            // 验证操作账号输入的密码，正确后才能进行盘点，防止误操作
            var account = _db.Table.Find<Account>(editedBy);
            if (!account.CheckPassword(loginPassword))
            {
                throw new Exception("密码错误");
            }
            var entity = _db.Table.Find<StocktakingPlan>(id);
            var items = _db.Table.FindAll<StocktakingPlanItem>(n => n.StocktakingPlanId == id);
            entity.Items = items.ToList();
            _service.ValidateEndStatus(entity);
            // 开始结转
            entity.ChangeCompleteStatus(editedBy, editor);
            _inventoryService.CheckIsExists(entity.Code);
            _inventoryService.FixedInventory(entity);           
            _db.Update(entity);
            _db.SaveChange();
        }
    }
}
