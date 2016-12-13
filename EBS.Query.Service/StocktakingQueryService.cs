using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Infrastructure.Extension;
namespace EBS.Query.Service
{
   public class StocktakingQueryService:IStocktakingQuery
    {
        IQuery _query;
        public StocktakingQueryService(IQuery query)
        {
            this._query = query;
        }

        public IEnumerable<StocktakingListDto> GetPageList(Pager page, SearchStocktaking condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and t0.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.StoreId > 0)
            {
                where += "and t0.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            if (condition.Type > 0)
            {
                where += "and t0.StocktakingType=@StocktakingType ";
                param.StocktakingType = condition.Type;
            }
            if (condition.StocktakingDate.HasValue)
            {
                where += "and TIMESTAMPDIFF(DAY,t0.StocktakingDate,@StocktakingDate)=0";
                param.StocktakingDate = condition.StocktakingDate;
            }
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where+=" and (t1.ProductID=@ProductIdOrBarCode or t1.BarCode=@ProductIdOrBarCode)";
                param.ProductIdOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.ShelfCode))
            {
                where+=" and t0.ShelfCode=@ShelfCode";
                param.ShelfCode = condition.ShelfCode;
            }

            if (string.IsNullOrEmpty(where))
            {
                return new List<StocktakingListDto>();
            }

            string sql = @"select t0.`Code`,t0.ShelfCode,t0.CreatedByName,t0.StocktakingType,t1.*,t2.`Name` as StoreName
from stocktaking t0 inner join stocktakingitem t1 on t0.Id = t1.StocktakingId
inner join store t2 on t2.Id = t0.StoreId 
where 1=1 {0} ORDER BY t0.Id desc ";

           // sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<StocktakingListDto>(sql, param) as IEnumerable<StocktakingListDto>;
            page.Total = rows.Count();
            return rows;
        }


        public StocktakingPlan GetRunningPlan(int storeId)
        {
            var model = this._query.Find<StocktakingPlan>(n => n.StoreId == storeId && n.Status == StocktakingPlanStatus.FirstInventory);
            if (model == null) {
                throw new Exception("没有开始盘点计划");
            }
            return model;
        }
    }
}
