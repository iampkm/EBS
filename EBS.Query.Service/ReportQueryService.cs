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
                where += "and w.CreatedByName like @NickName ";
                param.NickName = string.Format("%{0}%", condition.NickName);
            }
            //if (!string.IsNullOrEmpty(condition.UserName))
            //{
            //    where += "and a.UserName=@UserName ";
            //    param.UserName = condition.UserName;
            //}
            if (condition.PosId > 0)
            {
                where += "and o.PosId=@PosId ";
                param.PosId = condition.PosId;
            }
            if (condition.StoreId > 0)
            {
                where += "and s.Id=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            if (condition.WrokFrom.HasValue)
            {
                where += "and w.StartDate>=@StartDate";
                param.StartDate = condition.WrokFrom.Value;
            }
            if (condition.WrokTo.HasValue)
            {
                where += "and w.EndDate<=@EndDate";
                param.EndDate = condition.WrokTo.Value;
            }
            if (condition.From.HasValue)
            {
                where += "and o.UpdatedOn>=@UpdatedOn";
                param.UpdatedOn = condition.WrokFrom.Value;
            }
            if (condition.To.HasValue)
            {
                where += "and o.UpdatedOn<=@UpdatedOn";
                param.UpdatedOn = condition.WrokTo.Value;
            }

            string sql = @"select o.`Code`,o.PosId,o.OrderType,o.`Status`,o.OrderAmount,o.PayAmount,o.OnlinePayAmount,o.PaymentWay,o.PaidDate,o.UpdatedOn
,w.CreatedByName,s.`Name` as StoreName,w.StartDate,w.EndDate
 from saleorder o inner join saleorderitem i on o.Id = i.SaleOrderId 
inner join store s on s.Id= o.StoreId 
left join workschedule w on s.CreatedBy = w.CreatedBy
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

    }
}
