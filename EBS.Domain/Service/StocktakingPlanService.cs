using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using System.Security.Cryptography;
using EBS.Infrastructure.Extension;
using EBS.Domain.Entity;
namespace EBS.Domain.Service
{
   public class StocktakingPlanService
    {
        IDBContext _db;
        public StocktakingPlanService(IDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public bool ValidatePlan(StocktakingPlan model)
        {
            if (model.StoreId == 0)
                throw new Exception("请选择门店。");
            if (model.StocktakingDate < DateTime.Now.AddDays(-1))
                throw new Exception("盘点日期不能在当前日期之前。");
            if (_db.Table.Exists<StocktakingPlan>(n => n.StoreId==model.StoreId&& n.StocktakingDate == model.StocktakingDate.Date))
                throw new Exception( "【" + model.StocktakingDate.ToString("yyyy-MM-dd") + "】已经存在一个盘点计划。");
            return true;
        }
    }
}
