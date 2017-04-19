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
            if (condition.OrderLevel > 0)
            {
                where += " and o.OrderLevel=@OrderLevel ";
                param.OrderLevel = condition.OrderLevel;
            }
            if (condition.PaymentWay > 0)
            {
                where += " and o.PaymentWay=@PaymentWay ";
                param.PaymentWay = condition.PaymentWay;
            }
            string sql = @"select  o.Id, o.`Code`,o.PosId,o.OrderType,o.`Status`,o.OrderAmount,o.PayAmount,o.OnlinePayAmount,o.PaymentWay,o.PaidDate,o.UpdatedOn,o.OrderLevel,
a.NickName,s.Name as StoreName from saleorder o 
left join store s on s.Id= o.StoreId 
left join account a on a.Id = o.CreatedBy
left join workschedule w on o.WorkScheduleCode = w.Code
where 1=1 {0} ORDER BY o.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            if (string.IsNullOrEmpty(where))
            {
                page.Total = 0;
                return new List<SaleOrderDto>();
            }
            //sql = string.Format(sql, where);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<SaleOrderDto>(sql, param) as IEnumerable<SaleOrderDto>;

            string sqlCount = @"select count(*) from saleorder o 
left join store s on s.Id= o.StoreId 
left join account a on a.Id = o.CreatedBy
left join workschedule w on o.WorkScheduleCode = w.Code
where 1=1 {0} ORDER BY o.Id desc ";
            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);

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
                owhere += " and o.UpdatedOn>=@From ";
                param.From = condition.From.Value;
            }
            if (condition.To.HasValue)
            {
                owhere += " and o.UpdatedOn<@To ";
                param.To = condition.To.Value.AddDays(1);
            }
            if (condition.OrderLevel > 0)
            {
                owhere += " and o.OrderLevel=@OrderLevel ";
                param.OrderLevel = condition.OrderLevel;
            }
            if (condition.PaymentWay > 0)
            {
                owhere += " and o.PaymentWay=@PaymentWay ";
                param.PaymentWay = condition.PaymentWay;
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
where 1=1 {0} ORDER BY w.Id desc LIMIT {2},{3}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            if (string.IsNullOrEmpty(where)&&string.IsNullOrEmpty(owhere))
            {
                page.Total = 0;
                return new List<SaleSummaryDto>();
            }
            sql = string.Format(sql, where, owhere, page.PageIndex, page.PageSize);
            var rows = this._query.FindAll<SaleSummaryDto>(sql, param) as IEnumerable<SaleSummaryDto>;

            string sqlCount = @"select count(*) from WorkSchedule w inner join (
select o.WorkScheduleCode,sum(OrderAmount) as TotalAmount,sum(OnlinePayAmount) as TotalOnlineAmount,paymentWay from  saleorder o 
where o.Status = 3 {1} group by o.WorkScheduleCode,o.paymentWay
) t on t.WorkScheduleCode = w.Code
inner join Store s on s.Id = w.StoreId
where 1=1 {0}";
            sqlCount = string.Format(sqlCount, where,owhere);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
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

        public IEnumerable<SaleSyncDto> QuerySaleSync(Pager page, DateTime saleDate, string storeId)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            //客户端数据
            string csql = @"select s.`Name` as StoreName ,y.*,t.ServerOrderCount,t.ServerOrderTotalAmount from (
select o.StoreId,o.PosId,date_format(o.CreatedOn,'%Y-%m-%d') as SaleDate,count(*) ServerOrderCount,sum(OrderAmount) ServerOrderTotalAmount
 from saleorder o where o.`Status` in (-1,3) and o.UpdatedOn >= @BeginDate and o.UpdatedOn < @EndDate {0} GROUP BY o.StoreId,o.PosId,date_format(o.UpdatedOn, '%Y-%m-%d')
) t RIGHT JOIN Store s on s.Id = t.StoreId
LEFT JOIN salesync y on s.Id = y.StoreId
where t.PosId= y.PosId and y.SaleDate=@SaleDate";
            if (!string.IsNullOrEmpty(storeId) && storeId != "0")
            {
                var storeArray = storeId.Split(',').ToIntArray();
                where = " and o.StoreId in @StoreId ";
                param.StoreId = storeArray;               
            }
            csql = string.Format(csql, where);
            param.BeginDate = saleDate;
            param.EndDate = saleDate.Date.AddDays(1);
            param.SaleDate = saleDate.Date.ToString("yyyy-MM-dd");
            var rows = _query.FindAll<SaleSyncDto>(csql, param);
            string sqlCount = @"select count(*) from (
select o.StoreId,o.PosId,date_format(o.CreatedOn,'%Y-%m-%d') as SaleDate,count(*) ServerOrderCount,sum(OrderAmount) ServerOrderTotalAmount
 from saleorder o where o.`Status` in (-1,3) and o.UpdatedOn >= @BeginDate and o.UpdatedOn < @EndDate {0} GROUP BY o.StoreId,o.PosId,date_format(o.UpdatedOn, '%Y-%m-%d')
) t RIGHT JOIN Store s on s.Id = t.StoreId
LEFT JOIN salesync y on s.Id = y.StoreId
where t.PosId= y.PosId and y.SaleDate=@SaleDate";
            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
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


        public IEnumerable<SingleProductSaleDto> QuerySingleProductSale(Pager page, SearchSingleProductSale condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            string pwhere = "";
            //单品查询，必须输入编码或条码，否则立即返回空
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                pwhere += @"and ( p.code=@ProductCodeOrBarCode or p.barcode =@ProductCodeOrBarCode  ) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            else
            {
                return new List<SingleProductSaleDto>();
            }
            
            if (condition.StartDate.HasValue)
            {
                where += "and o.UpdatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and o.UpdatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }           

            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and o.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray(); 
            }

            

            string sql = @"select s.name as StoreName,p.Id as ProductId, p.`Name` as ProductName,p.`Code` as ProductCode,p.BarCode,SaleQuantity,SaleCostAmount,SaleAmount from (
select o.StoreId,i.ProductId,sum(i.Quantity) as SaleQuantity,sum(i.AvgCostPrice* i.Quantity) as SaleCostAmount,sum(i.RealPrice* i.Quantity) as SaleAmount
 from saleorder o inner join saleorderitem i on o.id =i.SaleOrderId
where o.`Status` = 3 {0}
GROUP BY o.StoreId,i.ProductId ) t
left join product p on p.id = t.ProductId
left join store s on s.id = t.storeid where 1=1 {3} LIMIT {1},{2}";
           
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize,pwhere);
            var rows = this._query.FindAll<SingleProductSaleDto>(sql, param);
            string sqlCount = @"select count(*) from (
select o.StoreId,i.ProductId,sum(i.Quantity) as SaleQuantity,sum(i.AvgCostPrice* i.Quantity) as SaleCostAmount,sum(i.RealPrice* i.Quantity) as SaleAmount
 from saleorder o inner join saleorderitem i on o.id =i.SaleOrderId
where o.`Status` = 3 {0}
GROUP BY o.StoreId,i.ProductId ) t
left join product p on p.id = t.ProductId
left join store s on s.id = t.storeid where 1=1 {1}";
            sqlCount = string.Format(sqlCount, where,pwhere);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);

            return rows;
        }


        public IEnumerable<SaleReportDto> QuerySaleReport(Pager page, SearchSaleReport condition)
        {
                  var  result = QuerySaleReportGroupByStore(page, condition);

            return result;
        }

        private IEnumerable<SaleReportDto> QuerySaleReportGroupByStore(Pager page, SearchSaleReport condition)
        {
            IEnumerable<SaleReportDto> result = new List<SaleReportDto>();
            if ((int)condition.GroupBy == 0)
            {
                return result;
            }
            

            dynamic param = new ExpandoObject();
            string where = "";

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += @"and ( p.code=@ProductCodeOrBarCode or p.barcode =@ProductCodeOrBarCode  ) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }           

            if (condition.StartDate.HasValue)
            {
                where += "and r.CreatedOn >=@StartDate ";
                param.StartDate = condition.StartDate.Value;
            }
            if (condition.EndDate.HasValue)
            {
                where += "and r.CreatedOn < @EndDate ";
                param.EndDate = condition.EndDate.Value.AddDays(1);
            }

            if (!string.IsNullOrEmpty(condition.StoreId) && condition.StoreId != "0")
            {
                where += "and r.StoreId in @StoreId ";
                param.StoreId = condition.StoreId.Split(',').ToIntArray();
            }
            if (!string.IsNullOrEmpty(condition.CategoryId))
            {
                where += "and c.Id like @CategoryId ";
                param.CategoryId = condition.CategoryId+"%"; 
            }
            if (condition.BrandId > 0) {
                where += "and b.Id = @BrandId ";
                param.BrandId = condition.BrandId; 
            }
            if (condition.OrderLevel > 0)
            {
                where += "and r.OrderLevel = @OrderLevel ";
                param.OrderLevel = condition.OrderLevel;
            }

            //按分组组装sql
            var sql = "";
            var sqlCount = "";
            var sqlSum = "";
            switch (condition.GroupBy)
            {
                case GroupByMethod.Store:
                    sql = @"select s.`name`,OrderCount,SaleQuantity,SaleCostAmount,SaleAmount from (
select r.storeId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.StoreId LIMIT {1},{2}  ) t
LEFT JOIN store s on s.id = t.StoreId";
                    sqlCount = @"select count(*) from (
select r.storeId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.StoreId ) t
LEFT JOIN store s on s.id = t.StoreId";
                    sqlSum = @"select sum(OrderCount) as OrderCount ,sum(SaleQuantity) as SaleQuantity,sum(SaleCostAmount) as SaleCostAmount,sum(SaleAmount) as SaleAmount from (
select r.storeId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.StoreId  ) t
LEFT JOIN store s on s.id = t.StoreId";

                    break;
                case GroupByMethod.Product:
                    sql = @"select s.`name`,OrderCount,SaleQuantity,SaleCostAmount,SaleAmount from (
select r.productId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.productId LIMIT {1},{2}  ) t
LEFT JOIN product s on s.id = t.productId";
                    sqlCount = @"select count(*) from (
select r.productId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.productId ) t
LEFT JOIN product s on s.id = t.productId";
                    sqlSum = @"select sum(OrderCount) as OrderCount ,sum(SaleQuantity) as SaleQuantity,sum(SaleCostAmount) as SaleCostAmount,sum(SaleAmount) as SaleAmount  from (
select r.productId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.productId  ) t
LEFT JOIN product s on s.id = t.productId";
                    break;
                case GroupByMethod.Category:                  
                      param.CategoryLevel = condition.CategoryLevel <= 0 ? 1 : condition.CategoryLevel; 
                                     
                     sql = @"select s.`FullName` as Name ,OrderCount,SaleQuantity,SaleCostAmount,SaleAmount from (
select left(c.Id,2*@CategoryLevel) as CategoryId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY left(c.Id,2*@CategoryLevel) LIMIT {1},{2}  ) t
LEFT JOIN Category s on s.id = t.CategoryId";
                    sqlCount = @"select count(*) from (
select c.Id CategoryId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY left(c.Id,2*@CategoryLevel) ) t
LEFT JOIN Category s on s.id = t.CategoryId";
                    sqlSum = @"select sum(OrderCount) as OrderCount ,sum(SaleQuantity) as SaleQuantity,sum(SaleCostAmount) as SaleCostAmount,sum(SaleAmount) as SaleAmount from (
select left(c.Id,2*@CategoryLevel) as CategoryId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY left(c.Id,2*@CategoryLevel) ) t
LEFT JOIN Category s on s.id = t.CategoryId";
                    break;
                case GroupByMethod.Supplier:
                    sql = @"select s.`name`,OrderCount,SaleQuantity,SaleCostAmount,SaleAmount from (
select r.SupplierId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.SupplierId LIMIT {1},{2}  ) t
LEFT JOIN supplier s on s.id = t.SupplierId";
                    sqlCount = @"select count(*) from (
select r.SupplierId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.SupplierId  ) t
LEFT JOIN supplier s on s.id = t.SupplierId";
                    sqlSum = @"select sum(OrderCount) as OrderCount ,sum(SaleQuantity) as SaleQuantity,sum(SaleCostAmount) as SaleCostAmount,sum(SaleAmount) as SaleAmount from (
select r.SupplierId,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.SupplierId ) t
LEFT JOIN supplier s on s.id = t.SupplierId";
                    break;
                case GroupByMethod.Creator:
                    sql = @"select CONCAT(s.NickName,'(',s.`UserName`,')')  as `Name`,OrderCount,SaleQuantity,SaleCostAmount,SaleAmount from (
select r.createdBy,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.createdBy LIMIT {1},{2}  ) t
LEFT JOIN account s on s.id = t.createdBy";
                    sqlCount = @"select count(*) from (
select r.createdBy,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.createdBy   ) t
LEFT JOIN account s on s.id = t.createdBy";
                    sqlSum = @"select sum(OrderCount) as OrderCount ,sum(SaleQuantity) as SaleQuantity,sum(SaleCostAmount) as SaleCostAmount,sum(SaleAmount) as SaleAmount from (
select r.createdBy,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY r.createdBy ) t
LEFT JOIN account s on s.id = t.createdBy";
                    break;
                case GroupByMethod.Day:
                    sql = @"select t.`Name`,OrderCount,SaleQuantity,SaleCostAmount,SaleAmount from (
select DATE_FORMAT(r.createdOn,'%Y-%m-%d') as `Name`,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY DATE_FORMAT(r.createdOn,'%Y-%m-%d')  LIMIT {1},{2}  ) t";
                    sqlCount = @"select count(*) from (
select DATE_FORMAT(r.createdOn,'%Y-%m-%d') as `Name`,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY DATE_FORMAT(r.createdOn,'%Y-%m-%d')  ) t";
                    sqlSum = @"select sum(OrderCount) as OrderCount ,sum(SaleQuantity) as SaleQuantity,sum(SaleCostAmount) as SaleCostAmount,sum(SaleAmount) as SaleAmount from (
select DATE_FORMAT(r.createdOn,'%Y-%m-%d') as `Name`,count(DISTINCT r.saleorderId) as OrderCount,sum(r.Quantity) as SaleQuantity,sum(r.CostPrice*r.Quantity) as SaleCostAmount,sum(r.SalePrice*r.Quantity) as SaleAmount 
from salereport r left join product p on p.id = r.productId 
left join category c on c.Id = p.CategoryId
left join brand b on b.Id = p.BrandId
where 1=1 {0} GROUP BY DATE_FORMAT(r.createdOn,'%Y-%m-%d')  ) t"; 
                    break;
                default:
                    break;
            }
            

            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<SaleReportDto>(sql, param);

            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);

            //汇总数据
            sqlSum = string.Format(sqlSum, where);
            var sumStoreInventory = this._query.Find<SumSaleReport>(sqlSum, param) as SumSaleReport;
            page.SumColumns.Add(new SumColumn("OrderCount", sumStoreInventory.OrderCount.ToString()));
            page.SumColumns.Add(new SumColumn("SaleQuantity", sumStoreInventory.SaleQuantity.ToString()));
            page.SumColumns.Add(new SumColumn("SaleCostAmount", sumStoreInventory.SaleCostAmount.ToString("F4")));
            page.SumColumns.Add(new SumColumn("SaleAmount", sumStoreInventory.SaleAmount.ToString("F2")));
            page.SumColumns.Add(new SumColumn("ProfitAmount", sumStoreInventory.ProfitAmount.ToString("F2")));
            page.SumColumns.Add(new SumColumn("ProfitPercent", sumStoreInventory.ProfitPercent.ToString("F2")+"%"));
            return rows;
        }
    }
}
