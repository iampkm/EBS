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
            if (condition.Status != 0)
            {
                where += "and t0.Status=@StocktakingStatus ";
                param.StocktakingStatus = condition.Status;
            }
            if (condition.StocktakingDate.HasValue)
            {
                where += "and datediff(t3.StocktakingDate,@StocktakingDate)=0";
                param.StocktakingDate = condition.StocktakingDate;
            }
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += " and (p.Code=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode)";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
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

            string sql = @"select t0.Id,t0.`Code`,t0.ShelfCode,t0.CreatedByName,t0.StocktakingType,t3.StocktakingDate,t1.CountQuantity,t1.CorectReason,t2.`Name` as StoreName,t3.StocktakingDate,p.Code as ProductCode,p.Name as ProductName,p.Specification,p.BarCode,p.Unit
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
            if (condition.Status != 0)
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
            string sql = @" SELECT sh.Id,sh.Code as ShelfCode,st.ProductId,p.Code as ProductCode,p.Name as ProductName,p.Specification,p.BarCode,p.Unit, st.SalePrice ,st.costprice,st.Quantity 
FROM StocktakingPlanItem AS st 
inner join stocktakingplan sp on sp.id = st.StocktakingPlanId
left join ShelfLayerProduct sh on sh.ProductId = st.ProductId and sh.StoreId = sp.StoreId
left join product p on p.Id = st.ProductId
  where sh.Code like @Code and st.StocktakingPlanId=@StocktakingPlanId order by sh.Code";
            var rows = _query.FindAll<StocktakingItemDto>(sql, new { StocktakingPlanId = planId, Code = string.Format("{0}%", shelfCode) });
           return rows;
        }


        public StocktakingItemDto QueryShelfProduct(int planId, int storeId, string productCodeOrBarCode)
        {
            string sql = @"SELECT st.ProductId,p.Code as ProductCode,p.Name as ProductName,p.Specification,p.BarCode,p.Unit, st.SalePrice ,st.costprice ,st.Quantity ,sh.Code ShelfCode 
FROM StocktakingPlanItem AS st 
inner join stocktakingplan sp on sp.id = st.StocktakingPlanId
left join ShelfLayerProduct sh on sh.ProductId = st.ProductId and sh.StoreId = sp.StoreId
left join product p on p.Id = st.ProductId
 WHERE st.StocktakingPlanId=@StocktakingPlanId and (p.Code = @ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode)";
            var model = _query.Find<StocktakingItemDto>(sql, new { StocktakingPlanId = planId, ProductCodeOrBarCode = productCodeOrBarCode });
            if (model == null) {
                throw new Exception("商品不存在");
            }
            return model;
        }


        public StocktakingItemDto QueryStocktaingItem(int planId, string productCodeOrBarCode)
        {
            string sql = @"SELECT i.ProductId,p.`Code` as ProductCode,p.`Name` as ProductName,p.Specification,p.BarCode,p.Unit, i.SalePrice ,i.CostPrice,Quantity,CountQuantity FROM StocktakingPlanItem i
LEFT JOIN product p on p.id = i.productid   
WHERE i.StocktakingPlanId=@StocktakingPlanId and (p.Code = @ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode)";
            var model = _query.Find<StocktakingItemDto>(sql, new { StocktakingPlanId = planId, ProductCodeOrBarCode = productCodeOrBarCode });
            if (model == null)
            {
                throw new Exception("商品不存在");
            }
            return model;
        }


        public StocktakingDto QueryStocktaking(int id)
        {
            string sql = @"select t0.*,t2.`Name` as StoreName,t3.StocktakingDate
from stocktaking t0 inner join store t2 on t2.Id = t0.StoreId
inner join stocktakingPlan t3 on t3.Id = t0.StocktakingPlanId 
where t0.Id =@Id";
            var model = _query.Find<StocktakingDto>(sql, new { Id = id });
            string sqlDetail = @"SELECT i.ProductId,p.`Code` as ProductCode,p.`Name` as ProductName,p.Specification,p.BarCode,p.Unit, i.SalePrice ,i.CostPrice,i.Quantity,i.CountQuantity,i.CorectQuantity,i.CorectReason FROM StocktakingItem i
LEFT JOIN product p on p.id = i.productid   
WHERE i.StocktakingId=@Id";
            var items = _query.FindAll<StocktakingItemDto>(sqlDetail, new { Id = id }).ToList();
            model.Items = items;
            return model;
        }
    }
}
