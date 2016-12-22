using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Query.DTO;
namespace EBS.Query
{
   public interface IReportQuery
    {
        IEnumerable<SaleOrderDto> QuerySaleOrderItems(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleSummaryDto> QuerySaleSummary(Pager page, SearchSaleOrder condition);

        IEnumerable<SaleCheckDto> QuerySaleCheck(Pager page, SearchSaleOrder condition);
    }
}
