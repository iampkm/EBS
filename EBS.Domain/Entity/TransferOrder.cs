﻿using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 调拨单 明细
    /// </summary>
   public class TransferOrder:BaseEntity
    {

       public TransferOrder()
       {
           this.CreatedOn = DateTime.Now;
           this.UpdatedOn = DateTime.Now;
           this.Status = TransferOrderStatus.WaitAudit;
       }

        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public string Code { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public TransferOrderStatus Status { get; set; }

        public virtual List<TransferOrderItem> Items { get; set; }
    }
}
