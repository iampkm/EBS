﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class AdjustContractPriceItemDto
    {
        public int ProductId { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        public string BarCode { get; set; }

        public string Specification { get; set; }

        public string Unit { get; set; }
        /// <summary>
        /// 原合同价
        /// </summary>
        public decimal AdjustPrice { get; set; }
        /// <summary>
        /// 合同价
        /// </summary>
        public decimal ContractPrice { get; set; }

    }
}
