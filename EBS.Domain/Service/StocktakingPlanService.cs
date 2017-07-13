using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Service
{
    public class StocktakingPlanService
    {
        IDBContext _db;
        public StocktakingPlanService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public void ValidatePlan(StocktakingPlan model)
        {
            if (model.StoreId == 0)
                throw new Exception("请选择门店。");
            if (model.StocktakingDate < DateTime.Now.AddDays(-1))
                throw new Exception("盘点日期不能在当前日期之前。");           
        }

        public void ValidatePlanDate(StocktakingPlan model)
        {
            if (_db.Table.Exists<StocktakingPlan>(n => n.StoreId == model.StoreId && n.StocktakingDate == model.StocktakingDate.Date))
                throw new Exception("【" + model.StocktakingDate.ToString("yyyy-MM-dd") + "】已经存在一个盘点计划。");
        }

        public void AddInventoryItems(StocktakingPlan model)
        {
            string checkSql = "select count(*) from StocktakingPlan where StoreId=@StoreId and (Status=@ToBeInventory or Status=@Replay )";
            //存在初盘和复盘的 计划，就不能开始一个新的盘点计划
            if (_db.Table.Context.ExecuteScalar<int>(checkSql, new { StoreId = model.StoreId, ToBeInventory = StocktakingPlanStatus.FirstInventory, Replay = StocktakingPlanStatus.Replay }) > 0)
            {
                throw new Exception("有未完成的盘点计划，不能开启新盘点");
            }
            //把库存明细导入盘点表
            string sql = @"insert into stocktakingPlanItem( StocktakingPlanId,ProductId,CostPrice,SalePrice,Quantity,CountQuantity )
  SELECT  s.Id,p.Id as ProductId ,i.LastCostPrice ,case when i.StoreSalePrice>0 then i.StoreSalePrice else p.SalePrice END as SalePrice  ,i.Quantity ,0  
FROM    StoreInventory i 
INNER JOIN StocktakingPlan s ON s.StoreId = i.StoreId 
INNER JOIN Product p ON p.Id=i.ProductId  
WHERE s.Id=@StocktakingPlanId and i.IsQuit=0";
            _db.Command.AddExecute(sql, new { StocktakingPlanId = model.Id });
        }

        public void MergeDetial(int planId)
        {
            string sql = @"UPDATE  StocktakingPlanItem si left join 
(
SELECT   i.ProductId ,SUM(i.CountQuantity) CountQuantity
 FROM   Stocktaking s
	INNER JOIN stocktakingItem i ON i.Stocktakingid = s.id
 WHERE    s.StocktakingPlanId = @StocktakingPlanId AND s.`Status`=@Status
 GROUP BY i.Productid
)t ON t.ProductId = si.ProductId
SET  si.CountQuantity = IFNULL(t.CountQuantity,0)  
 WHERE   si.StocktakingPlanId = @StocktakingPlanId ";
            _db.Command.AddExecute(sql, new { StocktakingPlanId = planId, Status = StocktakingStatus.Audited });
        }

        public void ValidateEndStatus(StocktakingPlan model)
        {
            if (model.Status !=  StocktakingPlanStatus.Replay)
                throw new Exception("还没执行过合并盘点，不能结束。");
        }
        /// <summary>
        /// 修正库存
        /// </summary>
        public void FixedInventory(StocktakingPlan model)
        {
            //若是小盘，则只修改盘点计划表。不修改库存
            if (model.Method == StocktakingPlanMethod.SmallCap)
            {               
                return;
            }
            // 大盘：

        }
    }
}
