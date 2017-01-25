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
            if (condition.Status > 0)
            {
                where += "and t0.Status=@StocktakingStatus ";
                param.StocktakingStatus = condition.Status;
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

            string sql = @"select t0.`Code`,t0.ShelfCode,t0.CreatedByName,t0.StocktakingType,t1.*,t2.`Name` as StoreName,t3.StocktakingDate,p.Code as ProductCode,p.Name as ProductName,p.Specification,p.BarCode,p.Unit
from stocktaking t0 inner join stocktakingitem t1 on t0.Id = t1.StocktakingId
inner join store t2 on t2.Id = t0.StoreId
inner join stocktakingPlan t3 on t3.Id = t0.StocktakingPlanId 
left join product p on p.id = t1.productId
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";

            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
           // sql = string.Format(sql, where);
            var rows = this._query.FindAll<StocktakingListDto>(sql, param) as IEnumerable<StocktakingListDto>;
            string sqlCount = @"select count(*) 
from stocktaking t0 inner join stocktakingitem t1 on t0.Id = t1.StocktakingId
inner join store t2 on t2.Id = t0.StoreId
inner join stocktakingPlan t3 on t3.Id = t0.StocktakingPlanId 
left join product p on p.id = t1.productId
where 1=1 {0}";
            sqlCount = string.Format(sqlCount, where);
            page.Total = _query.Context.ExecuteScalar<int>(sqlCount,param);
            return rows;
        }

        public IEnumerable<StocktakingDto> GetAuditList(Pager page, SearchStocktaking condition)
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
            if (condition.Status > 0)
            {
                where += "and t0.Status=@StocktakingStatus ";
                param.StocktakingStatus = condition.Status;
            }
            if (condition.StocktakingDate.HasValue)
            {
                where += "and TIMESTAMPDIFF(DAY,t0.StocktakingDate,@StocktakingDate)=0";
                param.StocktakingDate = condition.StocktakingDate;
            }
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += " and t0.Id in (select stocktakingId from stocktakingItem t1 where t1.ProductId=@ProductIdOrBarCode or t1.BarCode=@ProductIdOrBarCode)";
                param.ProductIdOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.ShelfCode))
            {
                where += " and t0.ShelfCode=@ShelfCode";
                param.ShelfCode = condition.ShelfCode;
            }

            if (string.IsNullOrEmpty(where))
            {
                return new List<StocktakingDto>();
            }

            string sql = @"select t0.*,t2.`Name` as StoreName,t3.StocktakingDate
from stocktaking t0 inner join store t2 on t2.Id = t0.StoreId
inner join stocktakingPlan t3 on t3.Id = t0.StocktakingPlanId 
where 1=1 {0} ORDER BY t0.Id desc ";

            // sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<StocktakingDto>(sql, param) as IEnumerable<StocktakingDto>;
            page.Total = rows.Count();
            return rows;
        }


        public IEnumerable<StocktakingItemDto> QueryShelf(int planId, int storeId, string shelfCode)
        {            
            // 查询货架商品信息
            string sql = @"select g.Id,g.Code as ShelfCode,g.ProductId,p.Code as ProductCode,p.Name as ProductName,p.Specification,p.BarCode,p.Unit, p.SalePrice ,t.costprice,t.Quantity  
  from ShelfLayerProduct g inner join Product p on g.ProductId = p.Id  
  left join (select costprice,ProductId,quantity from stocktakingplanitem where stocktakingplanId = @PlanId) t on t.ProductId =p.Id
  where g.Code like @Code and g.StoreId=@StoreId order by g.Code";
            var rows = _query.FindAll<StocktakingItemDto>(sql, new { PlanId = planId, StoreId = storeId, Code = string.Format("{0}%", shelfCode) });
           return rows;
        }


        public StocktakingItemDto QueryShelfProduct(int planId, int storeId, string productCodeOrBarCode)
        {
            string sql = @"SELECT st.ProductId,p.Code as ProductCode,p.Name as ProductName,p.Specification,p.BarCode,p.Unit, p.SalePrice ,st.costprice ,st.Quantity ,sh.Code ShelfCode 
FROM StocktakingPlanItem AS st LEFT JOIN (SELECT * FROM ShelfLayerProduct WHERE StoreId=@StoreId) AS sh on st.ProductId=sh.ProductId 
left join product p on p.id = st.ProductId
 WHERE st.StocktakingPlanId=@StocktakingPlanId and (p.Code = @ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode)";
            var model = _query.Find<StocktakingItemDto>(sql, new { StocktakingPlanId = planId, StoreId = storeId, ProductCodeOrBarCode = productCodeOrBarCode });
            if (model == null) {
                throw new Exception("商品不存在");
            }
            return model;
        }


        public StocktakingItemDto QueryStocktaingItem(int planId, string productCodeOrBarCode)
        {
            string sql = @"SELECT ProductId,ProductCode,ProductName,BarCode,Specification,CostPrice,SalesPrice,Quantity,CountQuantity FROM O2O_StocktakingPlanItem  
WHERE StocktakingPlanId=@StocktakingPlanId and (ProductId = @ProductIdOrBarCode or BarCode=@ProductIdOrBarCode)";
            var model = _query.Find<StocktakingItemDto>(sql, new { StocktakingPlanId = planId, ProductCodeOrBarCode = productCodeOrBarCode });
            if (model == null)
            {
                throw new Exception("商品不存在");
            }
            return model;
        }
    }
}
