﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class OutInOrderItemDto
    {

       public int StoreId { get; set; }
        public int ProductId { get; set; }

        public string BarCode { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        public decimal LastCostPrice { get; set; }

        public decimal CostPrice { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        public decimal Amount
        {
            get
            {
                return CostPrice * Quantity;
            }
        }

        /// <summary>
        /// 库存数量，出库单用
        /// </summary>
        public int InventoryQuantity { get; set; }
    }
}
