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
        IEnumerable<SaleOrderDto> QuerySaleOrderItems(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleSummaryDto> QuerySaleSummary(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleCheckDto> QuerySaleCheck(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleSyncDto> QuerySaleSync(Pager page, DateTime saleDate);

        IEnumerable<SaleOrderDto> QuerySaleOrder(Pager page, int wrokScheduleId, int status, int orderType);

        IEnumerable<SingleProductSaleDto> QuerySingleProductSale(Pager page, SearchSingleProductSale condition);

        //IEnumerable<SingleProductSaleDto> QuerySaleAnalysis(Pager page, SearchSingleProductSale condition);

    }
}
