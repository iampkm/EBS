using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SearchStocktaking
    {
       public string Code { get; set; }

       public int Type { get; set; }

       public string ProductCodeOrBarCode { get; set; }

       public DateTime? StocktakingDate { get; set; }

       public string ShelfCode { get; set; }

       public int StoreId { get; set; }

       public int Status { get; set; }
    }
}
