﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class SumStorePurchaseOrder
    {
       /// <summary>
       /// 订购数合计
       /// </summary>
       public int Quantity { get; set; }
       /// <summary>
       /// 实收数合计
       /// </summary>
       public int ActualQuantity { get; set; }
       /// <summary>
       /// 实收金额合计
       /// </summary>
       public decimal Amount { get; set; }

       /// <summary>
       /// 总记录数
       /// </summary>
       public int TotalCount { get; set; }
    }
}
