using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class ProductDetails
    {
         public int ProductId { get; set; }

        /// <summary>
        /// 商品详情描述
        /// </summary>
        public string Description { get; set; }
    }
}
