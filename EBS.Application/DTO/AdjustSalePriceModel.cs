using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class AdjustSalePriceModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public int UpdatedBy { get; set; }

        public string UpdatedByName { get; set; }
        /// <summary>
        ///  id,price 键值对
        /// </summary>
        public string Items { get; set; }
    }
}
