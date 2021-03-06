﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.Message
{
   public class InputCashAmountMessage:IMessage
    {
        public InputCashAmountMessage() { }
        public InputCashAmountMessage(int id, decimal money, int posId, int storeId)
        {
            this.Id = id;
            this.Money = money;
            this.PosId = posId;
            this.StoreId = storeId;
        }

        /// <summary>
        /// 班次Id
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 收现金额
        /// </summary>
        public decimal Money { get; private set; }

        /// <summary>
        /// pos 机器ID
        /// </summary>
        public int PosId { get;private set; }
        /// <summary>
        /// 门店ID
        /// </summary>
        public int StoreId { get; private set; }
    }
}
