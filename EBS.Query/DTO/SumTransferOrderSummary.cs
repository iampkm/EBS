using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumTransferOrderSummary
    {
       public int TotalCount { get; set; }
        public int OutQuantity { get; set; }

        public decimal OutAmount { get; set; }
        public int InQuantity { get; set; }

        public decimal InAmount { get; set; }

        public int DifferenceQuantity { get; set; }

        public decimal DifferenceAmount { get; set; }
    }
}
