using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class StoreInventoryUpdate:BaseEntity
    {
        /// <summary>
        /// 增减数库存数 
        /// </summary>
         public int Quantity { get; set; }

         public int SaleQuantity { get; set; }

         public int OrderQuantity { get; set; }

         public decimal AvgCostPrice { get; set; }


    }


}
