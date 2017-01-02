using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.Service
{
   public class ReportQueryService:IReportQuery
    {
        IQuery _query;
        public ReportQueryService(IQuery query)
        {
            this._query = query;
        }
       
        public IEnumerable<SaleOrderDto> QuerySaleOrderItems(Pager page, SearchSaleOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.NickName))
            {
                where += " and a.NickName like @NickName ";
                param.NickName = string.Format("%{0}%", condition.NickName);
            }
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and o.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.PosId.HasValue)
            {
                where += " and o.PosId=@PosId ";
                param.PosId = condition.PosId.Value;
            }
            if (condition.StoreId > 0)
            {
                where += " and s.Id=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            
            if(condition.WrokFrom.HasValue )
            {
                where +=" and w.StartDate >= @StartDate ";
                param.StartDate = condition.WrokFrom.Value;
            }
            if(condition.WrokTo.HasValue)
            {
                where += " and w.StartDate < @EndDate ";
                param.EndDate = condition.WrokTo.Value.AddDays(1);
            }
            if (condition.From.HasValue)
            {
                where += " and o.UpdatedOn>=@From";
                param.From = condition.From.Value;
            }
            if (condition.To.HasValue)
            {
                where += " and o.UpdatedOn<@To";
                param.To = condition.To.Value.AddDays(1);
            }

            string sql = @"select o.`Code`,o.PosId,o.OrderType,o.`Status`,o.OrderAmount,o.PayAmount,o.OnlinePayAmount,o.PaymentWay,o.PaidDate,o.UpdatedOn,a.NickName,s.Name as StoreName 
 from saleorder o 
left join store s on s.Id= o.StoreId 
left join account a on a.Id = o.CreatedBy
left join workschedule w on o.WorkScheduleCode = w.Code
where 1=1 {0}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            if (string.IsNullOrEmpty(where))
            {
                page.Total= 0;
                return new List<SaleOrderDto>();
            }
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<SaleOrderDto>(sql, param) as IEnumerable<SaleOrderDto>;
            page.Total =  rows.Count();

            return rows;
        }

        public IEnumerable<SaleSummaryDto> QuerySaleSummary(Pager page, SearchSaleOrder condition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SaleCheckDto> QuerySaleCheck(Pager page, SearchSaleOrder condition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SaleSyncDto> QuerySaleSync(Pager page, DateTime saleDate)
        {
            //客户端数据
            string csql = @"select s.`Name` as StoreName ,y.*,t.ServerOrderCount,t.ServerOrderTotalAmount from (
select o.StoreId,o.PosId,date_format(o.CreatedOn,'%Y-%m-%d') as SaleDate,count(*) ServerOrderCount,sum(OrderAmount) ServerOrderTotalAmount
 from saleorder o where o.`Status` in (-1,3) and o.CreatedOn >= @BeginDate and o.CreatedOn < @EndDate GROUP BY o.StoreId,o.PosId,date_format(o.CreatedOn, '%Y-%m-%d')
) t RIGHT JOIN Store s on s.Id = t.StoreId
LEFT JOIN salesync y on s.Id = y.StoreId
where t.PosId= y.PosId";
            var rows = _query.FindAll<SaleSyncDto>(csql, new { BeginDate = saleDate, EndDate = saleDate.Date.AddDays(1) }).ToList();
            return rows;

         }
    }
}
