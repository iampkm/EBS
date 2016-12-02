using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.SyncObject
{
    /// <summary>
    /// 库存商品同步
    /// </summary>
   public class ProductSync
    {
       public ProductSync()
       {
           UpdatedOn = DateTime.Now;
       }
       public int Id { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        public string Name { get; set; }

        public string Specification { get; set; }
        public decimal SalePrice { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
