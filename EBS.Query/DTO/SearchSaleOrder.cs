﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
  public  class SearchSaleOrder
    {
      public string Code { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName  { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 收银机
        /// </summary>
        public int? PosId { get; set; }
        public string StoreId { get; set; }
        /// <summary>
        /// 班次日期起
        /// </summary>
        public DateTime? WrokFrom { get; set; }
        public DateTime? WrokTo { get; set; }
        /// <summary>
        /// 自然日期起
        /// </summary>
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        /// <summary>
        /// 订单级别
        /// </summary>
        public int OrderLevel { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PaymentWay { get; set; }

        public string ProductCodeOrBarCode { get; set; }

        public string ProductName { get; set; }

    }
}
