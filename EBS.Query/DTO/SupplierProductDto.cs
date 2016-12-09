using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SupplierProductItemDto
    {
        public int ProductId { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string Code { get; set; }
       
        public string CategoryName { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }
    }

   public class SupplierProductDto
   {
       public int ProductId { get; set; }
       /// <summary>
       /// 商品名
       /// </summary>
       public string Name { get; set; }
       /// <summary>
       /// 商品编码
       /// </summary>
       public string Code { get; set; }

       public string BarCode { get; set; }

       public string SupplierName { get; set; }


       public string BrandName { get; set; }

       public string CategoryName { get; set; }

       public string Specification { get; set; }

       public decimal Price { get; set; }
      
   }
}
