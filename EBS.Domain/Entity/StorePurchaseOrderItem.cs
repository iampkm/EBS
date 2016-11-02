﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    public class StorePurchaseOrderItem : BaseEntity
    {
        public StorePurchaseOrderItem(int productSkuId,decimal contractPrice,int storePurchaseOrderId,int quantity) {
            this.StorePurchaseOrderId = StorePurchaseOrderId;
            this.ProductSkuId = productSkuId;
            this.ContractPrice = contractPrice;
            this.Quantity = quantity; 
        }
       public int StorePurchaseOrderId {get;set;}
       /// <summary>
       /// 商品SKUID
       /// </summary>
        public int ProductSkuId {get;set;}
       /// <summary>
       /// 合同价
       /// </summary>
        public decimal ContractPrice {get;set;}
       /// <summary>
       /// 实际价格
       /// </summary>
        public decimal Price { get; set; }
       /// <summary>
       /// 预订数量
       /// </summary>
        public int Quantity {get;set;}
       /// <summary>
       /// 实际收货数量
       /// </summary>
        public int ActualQuantity {get;set;}

       /// <summary>
       /// 生产日期
       /// </summary>
        public DateTime ProductionDate {get;set;}

        /// <summary>
        /// 保质期：单位天
        /// </summary>
        public int ShelfLife {get;set;}       
    }
}