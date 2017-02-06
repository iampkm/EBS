using Dapper.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Service
{
    public class StocktakingService
    {
        IDBContext _db;
        public StocktakingService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public void CheckWaittingAuditCorrect(int planId)
        {
            string sql = "select count(*) from stocktaking where StocktakingType=@StocktakingType and Status=@Status and StocktakingPlanId=@StocktakingPlanId";
            if (this._db.Table.Context.ExecuteScalar<int>(sql, new { StocktakingType = (int)StocktakingType.StocktakingCorect, Status = (int)StocktakingStatus.WaitAuditing, StocktakingPlanId = planId})>0)
            {
                throw new Exception("有未审核的盘点修正单！");
            }
        }
    }
}
