using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.Service
{
    public class SaleOrderQueryService : ISaleOrderQuery
    {
        IQuery _query;
        public SaleOrderQueryService(IQuery query)
        {
            this._query = query;
        }
        public SaleOrderDto GetById(int id)
        {
            var sql = @"select o.Id, o.`Code`,o.PosId,o.OrderType,o.`Status`,o.OrderAmount,o.PayAmount,o.OnlinePayAmount,o.PaymentWay,o.PaidDate,o.UpdatedOn,a.NickName,s.Name as StoreName 
from saleorder o 
inner join store s on o.StoreId = s.Id
inner join account a on o.CreatedBy = a.Id
where o.Id=@OrderId";
            var model = _query.Find<SaleOrderDto>(sql, new { OrderId = id});
            // 明细
            var isql = @"SELECT i.ProductId,i.ProductCode,i.ProductName,p.BarCode,p.Specification,p.Unit,i.SalePrice,i.RealPrice,i.Quantity,i.SaleOrderId
 from saleorderitem i LEFT JOIN
product p on i.ProductId=p.Id where i.SaleOrderId=@OrderId";
            var items = _query.FindAll<SaleOrderItemDto>(isql, new { OrderId = id }).ToList();
            model.Items = items;
            return model;
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

            if (condition.WrokFrom.HasValue)
            {
                where += " and w.StartDate >= @StartDate ";
                param.StartDate = condition.WrokFrom.Value;
            }
            if (condition.WrokTo.HasValue)
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

            string sql = @"select  o.Id, o.`Code`,o.PosId,o.OrderType,o.`Status`,o.OrderAmount,o.PayAmount,o.OnlinePayAmount,o.PaymentWay,o.PaidDate,o.UpdatedOn,a.NickName,s.Name as StoreName 
 from saleorder o 
inner join store s on s.Id= o.StoreId 
inner join account a on a.Id = o.CreatedBy
inner join workschedule w on o.WorkScheduleCode = w.Code
where 1=1 {0}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            if (string.IsNullOrEmpty(where))
            {
                page.Total = 0;
                return new List<SaleOrderDto>();
            }
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<SaleOrderDto>(sql, param) as IEnumerable<SaleOrderDto>;
            page.Total = rows.Count();

            return rows;
        }

        public IEnumerable<SaleSummaryDto> QuerySaleSummary(Pager page, SearchSaleOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            string owhere = "";
           
            if (!string.IsNullOrEmpty(condition.Code))
            {
                owhere += "and o.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.PosId.HasValue)
            {
                owhere += " and o.PosId=@PosId ";
                param.PosId = condition.PosId.Value;
            }
            if (condition.StoreId > 0)
            {
                owhere += " and o.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            if (condition.From.HasValue)
            {
                owhere += " and o.UpdatedOn>=@From";
                param.From = condition.From.Value;
            }
            if (condition.To.HasValue)
            {
                owhere += " and o.UpdatedOn<@To";
                param.To = condition.To.Value.AddDays(1);
            }
            if (!string.IsNullOrEmpty(condition.NickName))
            {
                where += " and w.CreatedByName like @NickName ";
                param.NickName = string.Format("%{0}%", condition.NickName);
            }
            if (condition.WrokFrom.HasValue)
            {
                where += " and w.StartDate >= @StartDate ";
                param.StartDate = condition.WrokFrom.Value;
            }
            if (condition.WrokTo.HasValue)
            {
                where += " and w.StartDate < @EndDate ";
                param.EndDate = condition.WrokTo.Value.AddDays(1);
            }
           

            string sql = @"select w.Id, w.CreatedByName,s.Name as StoreName,w.PosId,w.CashAmount,w.StartDate,w.EndDate,t.TotalAmount,t.TotalOnlineAmount,t.paymentWay from WorkSchedule w inner join (
select o.WorkScheduleCode,sum(OrderAmount) as TotalAmount,sum(OnlinePayAmount) as TotalOnlineAmount,paymentWay from  saleorder o 
where o.Status = 3 {1} group by o.WorkScheduleCode,o.paymentWay
) t on t.WorkScheduleCode = w.Code
inner join Store s on s.Id = w.StoreId
where 1=1 {0}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            if (string.IsNullOrEmpty(where)&&string.IsNullOrEmpty(owhere))
            {
                page.Total = 0;
                return new List<SaleSummaryDto>();
            }
            sql = string.Format(sql, where,owhere);
            var rows = this._query.FindAll<SaleSummaryDto>(sql, param) as IEnumerable<SaleSummaryDto>;
            page.Total = rows.Count();
            //汇总
            string sqlSum = @"select sum(w.CashAmount) as CashAmount,sum(t.TotalAmount) as TotalAmount,sum(t.TotalOnlineAmount) as TotalOnlineAmount from WorkSchedule w inner join (
select o.WorkScheduleCode,sum(OrderAmount) as TotalAmount,sum(OnlinePayAmount) as TotalOnlineAmount,paymentWay from  saleorder o 
where o.Status = 3 {1} group by o.WorkScheduleCode,o.paymentWay
) t on t.WorkScheduleCode = w.Code
inner join Store s on s.Id = w.StoreId
where 1=1 {0}";
            sqlSum = string.Format(sqlSum, where, owhere);
            var sum = this._query.Find<SumWorkSchedule>(sqlSum, param) as SumWorkSchedule;
            page.SumColumns.Add(new SumColumn("CashAmount", sum.CashAmount.ToString("F2")));
            page.SumColumns.Add(new SumColumn("TotalAmount", sum.TotalAmount.ToString("F2")));
            page.SumColumns.Add(new SumColumn("TotalOnlineAmount", sum.TotalOnlineAmount.ToString("F2")));
            return rows;
        }

        public IEnumerable<SaleCheckDto> QuerySaleCheck(Pager page, SearchSaleOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            string owhere = "";

            if (!string.IsNullOrEmpty(condition.Code))
            {
                owhere += "and o.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.PosId.HasValue)
            {
                owhere += " and o.PosId=@PosId ";
                param.PosId = condition.PosId.Value;
            }
            if (condition.StoreId > 0)
            {
                owhere += " and o.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            if (condition.From.HasValue)
            {
                owhere += " and o.UpdatedOn>=@From";
                param.From = condition.From.Value;
            }
            if (condition.To.HasValue)
            {
                owhere += " and o.UpdatedOn<@To";
                param.To = condition.To.Value.AddDays(1);
            }
            if (!string.IsNullOrEmpty(condition.NickName))
            {
                where += " and w.CreatedByName like @NickName ";
                param.NickName = string.Format("%{0}%", condition.NickName);
            }
            if (condition.WrokFrom.HasValue)
            {
                where += " and w.StartDate >= @StartDate ";
                param.StartDate = condition.WrokFrom.Value;
            }
            if (condition.WrokTo.HasValue)
            {
                where += " and w.StartDate < @EndDate ";
                param.EndDate = condition.WrokTo.Value.AddDays(1);
            }


            string sql = @"select w.Id, w.CreatedByName,s.Name as StoreName,w.PosId,w.StartDate,w.EndDate,t.TotalAmount,t.orderCount,t.Status,t.OrderType from WorkSchedule w inner join (
select o.WorkScheduleCode,sum(OrderAmount) as TotalAmount,count(*) as orderCount,o.Status,o.OrderType from  saleorder o 
where (o.Status = -1 or o.OrderType =2) {1} group by o.WorkScheduleCode,o.Status,o.OrderType
) t on t.WorkScheduleCode = w.Code
inner join Store s on s.Id = w.StoreId
where 1=1 {0}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            if (string.IsNullOrEmpty(where) && string.IsNullOrEmpty(owhere))
            {
                page.Total = 0;
                return new List<SaleCheckDto>();
            }
            sql = string.Format(sql, where, owhere);
            var rows = this._query.FindAll<SaleCheckDto>(sql, param) as IEnumerable<SaleCheckDto>;
            page.Total = rows.Count();

            return rows;
        }

        public IEnumerable<SaleSyncDto> QuerySaleSync(Pager page, DateTime saleDate)
        {
            //客户端数据
            string csql = @"select s.`Name` as StoreName ,y.*,t.ServerOrderCount,t.ServerOrderTotalAmount from (
select o.StoreId,o.PosId,date_format(o.CreatedOn,'%Y-%m-%d') as SaleDate,count(*) ServerOrderCount,sum(OrderAmount) ServerOrderTotalAmount
 from saleorder o where o.`Status` in (-1,3) and o.UpdatedOn >= @BeginDate and o.UpdatedOn < @EndDate GROUP BY o.StoreId,o.PosId,date_format(o.UpdatedOn, '%Y-%m-%d')
) t RIGHT JOIN Store s on s.Id = t.StoreId
LEFT JOIN salesync y on s.Id = y.StoreId
where t.PosId= y.PosId and y.SaleDate=@SaleDate";
            var rows = _query.FindAll<SaleSyncDto>(csql, new { BeginDate = saleDate, EndDate = saleDate.Date.AddDays(1), SaleDate = saleDate.Date.ToString("yyyy-MM-dd") }).ToList();
            return rows;

        }

        public IEnumerable<SaleOrderDto> QuerySaleOrder(Pager page, int wrokScheduleId, int status, int orderType)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            
            if (wrokScheduleId!=0)
            {
                where += "and w.id=@WrokScheduleId ";
                param.WrokScheduleId = wrokScheduleId;
            }
           
            if (status!= 0)
            {
                where += " and o.status=@Status ";
                param.Status = status;
            }

            if (orderType != 0)
            {
                where += " and o.orderType=@OrderType ";
                param.OrderType = orderType;
            }

            string sql = @"select  o.Id, o.`Code`,o.PosId,o.OrderType,o.`Status`,o.OrderAmount,o.PayAmount,o.OnlinePayAmount,o.PaymentWay,o.PaidDate,o.UpdatedOn,a.NickName,s.Name as StoreName 
 from saleorder o 
inner join store s on s.Id= o.StoreId 
inner join account a on a.Id = o.CreatedBy
inner join workschedule w on o.WorkScheduleCode = w.Code
where 1=1 {0}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            if (string.IsNullOrEmpty(where))
            {
                page.Total = 0;
                return new List<SaleOrderDto>();
            }
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<SaleOrderDto>(sql, param) as IEnumerable<SaleOrderDto>;
            page.Total = rows.Count();
            return rows;
        }
    }
}
