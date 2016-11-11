using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class AdjustContractPriceItem:BaseEntity
    {
       public int AdjustContractPriceId { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 合同价
        /// </summary>
        public decimal ContractPrice { get; set; } 
       /// <summary>
        /// 调整价
        /// </summary>
        public decimal AdjustPrice { get; set; }
        
    }
}
