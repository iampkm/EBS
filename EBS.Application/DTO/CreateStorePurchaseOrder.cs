﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class CreateStorePurchaseOrder
    {
        public string Code { get; set; }
        public int SupplierId { get; set; }
        public int StoreId { get; set; }

        public int OrderType { get; set; }

        public bool IsGift { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
       /// <summary>
       /// 商品明细json 串
       /// </summary>
        public string Items { get; set; }
    }
}
