using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class VipProductDto
    {

       public int Id { get; set; }
        public int ProductId { get; set; }

        public string ProductCode { get; set; }
        public string BarCode { get; set; }

        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        public decimal SalePrice { get; set; }

        public decimal VipSalePrice { get; set; }
    }
}
