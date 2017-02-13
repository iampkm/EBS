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
           
            string sql = @"select t.name as StoreName,ifnull(t1.qty,0) as PreInventoryQuantity,ifnull(t1.amount,0) as PreInventoryAmount,
ifnull(t2.qty,0) as PurchaseQuantity,ifnull(t2.amount,0) as PurchaseAmount,
ifnull(t4.qty,0) as SaleQuantity,ifnull(t4.amount,0) as SaleAmount, ifnull(t4.costAmount,0) as CostAmount,
ifnull(t3.qty,0) as EndInventoryQuantity ,ifnull(t3.amount,0) as EndInventoryAmount from 
store t LEFT JOIN 
 (
	select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn <@StartDate
group by storeid
) t1 on t.Id = t1.storeid left join 
(
select d.storeid,sum(d.qty) as qty,sum(d.amount) as amount from 
		(
		select s.storeid,sum(i.actualQuantity) qty,sum(i.price*i.actualQuantity) amount from storepurchaseorder s inner join storepurchaseorderitem i
		on s.id = i.storepurchaseorderid 
		where s.`Status`>=4 and s.ordertype=1 and  s.StoragedOn BETWEEN @StartDate and @EndDate
		group by s.storeid
		union 
		select s.storeid,sum(-i.actualQuantity) qty,sum(i.price*-i.actualQuantity) amount from storepurchaseorder s inner join storepurchaseorderitem i
		on s.id = i.storepurchaseorderid 
		where s.`Status`>=4 and s.ordertype=2 and s.StoragedOn BETWEEN @StartDate and @EndDate
		group by s.storeid
		union	
	select t.toStoreId as storeid,sum(i.quantity) qty,sum(i.price*i.quantity) amount
	 from transferorder t inner join transferorderitem i on t.id = i.transferorderid  and t.`status` = 2
   where t.UpdatedOn BETWEEN @StartDate and @EndDate
	 group by t.toStoreId
	UNION
	select t.fromStoreId as storeid,sum(-i.quantity) qty,sum(i.price*-i.quantity) amount
	 from transferorder t inner join transferorderitem i on t.id = i.transferorderid  and t.`status` = 2
   where t.UpdatedOn BETWEEN @StartDate and @EndDate
	 group by t.toStoreId
	) d group by d.storeid
) t2 on t.Id = t2.storeid
left  join 
(
select storeid,SUM(changequantity) as qty,sum(changequantity*price) as amount from storeinventoryhistory
where createdOn <@EndDate
group by storeid
) t3 on t.Id = t3.storeid
left join 
(
	select s.storeid,sum(i.Quantity) qty,sum(i.realprice*i.Quantity) amount ,sum(i.AvgCostPrice) costAmount from saleorder s inner join saleorderitem i 
on s.id= i.saleorderid
where s.`Status` = 3 and s.UpdatedOn BETWEEN @StartDate and @EndDate
group by s.storeid
) t4 on t.Id = t4.StoreId
where 1=1 {0} ORDER BY t.Id LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<PurchaseSaleInventoryDto>(sql, param);
            page.Total = rows.Count;
            return rows;
        }
    }
}
