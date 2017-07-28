using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface ISaleOrderQuery
    {
        SaleOrderDto GetById(int id);
        IEnumerable<SaleOrderListDto> QuerySaleOrderItems(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleSummaryDto> QuerySaleSummary(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleCheckDto> QuerySaleCheck(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleSyncDto> QuerySaleSync(Pager page, DateTime saleDate,string storeId);

        IEnumerable<SaleOrderDto> QuerySaleOrder(Pager page, int wrokScheduleId, int status, int orderType);

        IEnumerable<SingleProductSaleDto> QuerySingleProductSale(Pager page, SearchSingleProductSale condition);

        IEnumerable<SaleReportDto> QuerySaleReport(Pager page, SearchSaleReport condition);
        IEnumerable<RealTimeSaleReportDto> QueryRealTimeSaleReport(Pager page, SearchSaleReport condition);

    }
}
