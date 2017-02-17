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
        public IEnumerable<PurchaseSaleInventoryDto> QueryPurchaseSaleInventorySummary(Pager page, PurchaseSaleInventorySearch condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (condition.StoreId!=0)
            {
                where += "and t.StoreId = @StoreId ";
                param.StoreId = condition.StoreId;
            }
            
            param.StartDate = condition.StartDate;

            param.EndDate = condition.EndDate.AddDays(1);
            // 此处多显示起止时间，主要是为了让前端表格框架，连接明细时能传递时间参数
            string sql = @"select t.Id,t.name as StoreName,ifnull(t1.qty,0) as PreInventoryQuantity,ifnull(t1.amount,0) as PreInventoryAmount,
ifnull(t2.qty,0) as PurchaseQuantity,ifnull(t2.amount,0) as PurchaseAmount,
ifnull(t4.qty,0) as SaleQuantity,ifnull(t4.amount,0) as SaleCostAmount,
ifnull(t5.amount,0) as SaleAmount,
ifnull(t3.qty,0) as EndInventoryQuantity ,ifnull(t3.amount,0) as EndInventoryAmount,'{3}' as StartDate,'{4}' as EndDate from 
store t LEFT JOIN 
 (
	select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn <@StartDate
group by storeid
) t1 on t.Id = t1.storeid left join 
(
		select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn BETWEEN @StartDate and @EndDate   and BillType in (51,52,53,60)
group by storeid
) t2 on t.Id = t2.storeid
left  join 
(
select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn <@EndDate
group by storeid
) t3 on t.Id = t3.storeid
left join 
(
	select storeid,abs(SUM(changequantity)) as qty,abs(sum(changequantity*price)) as amount from storeinventoryhistory 
where createdOn BETWEEN @StartDate and @EndDate  and BillType in (1,2)
group by storeid
) t4 on t.Id = t4.StoreId
left join 
(
	select s.storeid,sum(i.Quantity) qty,sum(i.realprice*i.Quantity) amount from saleorder s inner join saleorderitem i 
on s.id= i.saleorderid
where s.`Status` = 3 and s.UpdatedOn BETWEEN @StartDate and @EndDate 
group by s.storeid
) t5 on t.Id = t5.StoreId
where 1=1 {0} ORDER BY t.Id LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize, condition.StartDate.ToString("yyyy-MM-dd"), condition.EndDate.ToString("yyyy-MM-dd"));
            var rows = this._query.FindAll<PurchaseSaleInventoryDto>(sql, param);
            page.Total = rows.Count;
            return rows;
        }


        public IEnumerable<PurchaseSaleInventoryDetailDto> QueryPurchaseSaleInventoryDetail(Pager page, PurchaseSaleInventoryDetailSearch condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
           
            where += "and s.StoreId = @StoreId ";
            param.StoreId = condition.StoreId;
            param.StartDate = condition.StartDate;
            param.EndDate = condition.EndDate.AddDays(1);

            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (t.`code` = @ProductCodeOrBarCode or t.BarCode =@ProductCodeOrBarCode) ";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }

            string sql = @"select t.`code` as productCode,t.BarCode, t.name as ProductName,ifnull(t1.qty,0) as PreInventoryQuantity,ifnull(t1.amount,0) as PreInventoryAmount,
ifnull(t2.qty,0) as PurchaseQuantity,ifnull(t2.amount,0) as PurchaseAmount,
ifnull(t4.qty,0) as SaleQuantity,ifnull(t4.amount,0) as SaleCostAmount,
ifnull(t5.amount,0) as SaleAmount,
ifnull(t3.qty,0) as EndInventoryQuantity ,ifnull(t3.amount,0) as EndInventoryAmount from 
storeinventory s left join product t on t.Id = s.ProductId
 LEFT JOIN 
 (
	select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and  createdOn <@StartDate
group by productid
) t1 on t.Id = t1.productid left join 
(
		select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate    and BillType in (51,52,53,60)
group by productid
) t2 on t.Id = t2.productid
left  join 
(
select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and createdOn <@EndDate
group by productid
) t3 on t.Id = t3.productid
left join 
(
	select productid,abs(SUM(changequantity)) as qty,abs(sum(changequantity*price)) as amount from storeinventoryhistory 
where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate   and BillType in (1,2)
group by productid
) t4 on t.Id = t4.productid
left join 
(
	select i.productid,sum(i.Quantity) qty,sum(i.realprice*i.Quantity) amount from saleorder s inner join saleorderitem i 
on s.id= i.saleorderid
where s.storeid=@StoreId and s.`Status` =3 and s.UpdatedOn BETWEEN @StartDate and @EndDate 
group by i.productid
) t5 on t.Id = t5.productid
where 1=1 {0} ORDER BY t.Id LIMIT {1},{2}";           
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<PurchaseSaleInventoryDetailDto>(sql, param);
            string sqlCount = @"select count(*) from 
storeinventory s left join product t on t.Id = s.ProductId
 LEFT JOIN 
 (
	select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and  createdOn <@StartDate
group by productid
) t1 on t.Id = t1.productid left join 
(
		select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate    and BillType in (51,52,53,60)
group by productid
) t2 on t.Id = t2.productid
left  join 
(
select productid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where storeid=@StoreId and createdOn <@EndDate
group by productid
) t3 on t.Id = t3.productid
left join 
(
	select productid,abs(SUM(changequantity)) as qty,abs(sum(changequantity*price)) as amount from storeinventoryhistory 
where storeid=@StoreId and createdOn BETWEEN @StartDate and @EndDate   and BillType in (1,2)
group by productid
) t4 on t.Id = t4.productid
left join 
(
	select i.productid,sum(i.Quantity) qty,sum(i.realprice*i.Quantity) amount from saleorder s inner join saleorderitem i 
on s.id= i.saleorderid
where s.storeid=@StoreId and s.`Status` = 3 and s.UpdatedOn BETWEEN @StartDate and @EndDate 
group by i.productid
) t5 on t.Id = t5.productid
where 1=1 {0} ORDER BY t.Id ";

            sqlCount = string.Format(sqlCount, where);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);
           
            return rows;
        }
    }
}
