using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 货架
    /// </summary>
   public class Shelf:BaseEntity
    {
        public int StoreId { get; set; }
        public int Number { get; set; }
        /// <summary>
        /// 货架码
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
