using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using Dapper.DBContext;
namespace EBS.Domain.Service
{
    public class SaleReportService
    {
        IDBContext _db;
        public SaleReportService(IDBContext dbContext)
        {
            this._db = dbContext;
        }
        public void CreateSaleReport(DateTime beginDate, DateTime endDate)
        {            
            var sql = @"REPLACE INTO SaleReport (StoreInventoryHistoryId,SaleOrderId,ProductId,OrderType,PaymentWay,OrderLevel,StoreId,SupplierId,CostPrice,SalePrice,RealPrice,Quantity,CreatedOn,CreatedBy,UpdatedOn)
select h.Id as StoreInventoryHistoryId, i.SaleOrderId, h.ProductId,o.OrderType,o.PaymentWay,o.OrderLevel,h.StoreId,h.SupplierId,h.Price as CostPrice,i.SalePrice,i.RealPrice, h.ChangeQuantity*-1 as Quantity,h.CreatedOn,h.CreatedBy,now()
from StoreInventoryHistory h 
left join saleorderitem i on i.SaleOrderId = h.BillId  and i.ProductId = h.ProductId
left JOIN saleorder o on   o.Id = i.SaleOrderId  
where h.BillType in (1,2)  and h.CreatedOn between @BeginDate and @EndDate";
            _db.Command.Execute(sql, new { BeginDate = beginDate, EndDate = endDate });
        }

        public int GetDiffDay(DateTime beginDate, DateTime endDate) {
            if (beginDate > endDate) throw new Exception("开始日期不能大于结束日期");
            var diffDay = (endDate - beginDate).Days;
            return diffDay;
        }
    }
}
