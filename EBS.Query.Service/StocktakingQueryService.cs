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

        public IEnumerable<DTO.StocktakingPlanDto> GetPageList(DTO.Pager page, DTO.SearchStocktakingPlan condition)
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
            if (condition.Status > 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            if (condition.StocktakingDate.HasValue)
            {
                where += "and t0.StocktakingDate>=@beginDate and t0.StocktakingDate<@endDate ";
                param.beginDate = condition.StocktakingDate;
                param.endDate = condition.StocktakingDate.Value.AddDays(1);
            }
            string sql = @"select t0.Id,t0.Code,t0.StocktakingDate,t0.method,t0.Status,t0.createdByName,t1.Name as StoreName  
from stocktakingplan t0 inner join store t1 on t0.StoreId = t1.Id 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
 
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StocktakingPlanDto>(sql, param);
            page.Total = this._query.Count<StocktakingPlan>(where, param);

            return rows;
        }

        public IEnumerable<StocktakingSummaryDto> GetSummaryData(Pager page, SearchStocktakingPlan condition)
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
            if (condition.Status > 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            if (condition.StocktakingDate.HasValue)
            {
                where += "and t0.StocktakingDate>=@beginDate and t0.StocktakingDate<@endDate ";
                param.beginDate = condition.StocktakingDate;
                param.endDate = condition.StocktakingDate.Value.AddDays(1);
            }
            string sql = @"select t0.Id,t0.`Code`,t2.`Name` as StoreName,t0.`Status`,t0.Method,t0.StocktakingDate,t1.TotalInventoryQuantity,t1.TotalCountQuantity,
t1.CostAmount,t1.CostCountAmount,t1.SaleAmout,t1.SaleCountAmount
from stocktakingplan t0
inner join
(
SELECT i.Id,sum(i.Quantity) as TotalInventoryQuantity,sum(i.CountQuantity) as TotalCountQuantity,
sum(i.CostPrice*i.Quantity) as CostAmount,sum(i.CostPrice*i.CountQuantity) as CostCountAmount,
sum(i.SalePrice*i.Quantity) as SaleAmout,sum(i.SalePrice*i.CountQuantity) as SaleCountAmount
 FROM stocktakingplan p 
inner join stocktakingplanitem i on p.Id = i.StocktakingPlanId
group by p.Id 
) t1 on t0.Id = t1.Id
inner join store t2 on t2.Id = t0.StoreId 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";

            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StocktakingSummaryDto>(sql, param);
            page.Total = this._query.Count<StocktakingPlan>(where, param);

            return rows;
        }

        public Dictionary<int, string> GetStocktakingPlanStatus()
        {
            var dic = typeof(StocktakingPlanStatus).GetValueToDescription();
            return dic;
        }

        public IEnumerable<StocktakingPlanItemDto> GetDetails(int planId, int? from, int? to, bool showDifference)
        {
            var rows = _query.FindAll<StocktakingPlanItem>(n => n.StocktakingPlanId == planId) as IEnumerable<StocktakingPlanItemDto>;
            if (from.HasValue)
            {
                rows = rows.Where(n => n.GetDifferenceQuantity() >= from.Value);
            }
            if (to.HasValue)
            {
                rows = rows.Where(n => n.GetDifferenceQuantity() <= to.Value);
            }
            if (showDifference)
            {
                rows = rows.Where(n => n.GetDifferenceQuantity() != 0);
            }
            return rows;
        }
    }
}
