﻿using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class StorePurchaseOrderQueryDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }

        public string Supplier
        {
            get
            {
                return string.Format("[{0}]{1}", SupplierCode, SupplierName);
            }
        }
        public string StoreName { get; set; }
        public PurchaseOrderStatus Status { get; set; }

        public string PurchaseOrderStatus
        {
            get
            {
                return Status.Description();
            }
        }
        public DateTime CreatedOn { get; set; }

        public string CreatedTime
        {
            get
            {
                return CreatedOn.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string CreatedByName { get; set; }

        public int Quantity { get; set; }

        public int ActualQuantity { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// 供应商备注：可以备注单号，其他信息
        /// </summary>
        public string SupplierBill { get; set; }


        public DateTime StoragedOn { get; set; }
        public string StoragedTime
        {
            get
            {
                return StoragedOn.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditName { get; set; }
    }
}
