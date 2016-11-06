﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Entity
{
    public class StorePurchaseOrder : BaseEntity
    {
        private List<StorePurchaseOrderItem> _items;
        public StorePurchaseOrder()
        {
            this.CreatedOn = DateTime.Now;
            _items = new List<StorePurchaseOrderItem>();
            this.Status = PurchaseOrderStatus.Create;
        }

        public string Code { get; set; }
        public int SupplierId { get; set; }
        public string SupplierBill { get; set; }
        public int StoreId { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public bool IsGift { get; set; }
        /// <summary>
        /// 入库批次
        /// </summary>
        public string BatchNo { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public int ReceivedBy { get; set; }
        public string ReceivedByName { get; set; }
        public DateTime? ReceivedOn { get; set; }
        /// <summary>
        /// 入库人
        /// </summary>
        public int StoragedBy { get; set; }
        public string StoragedByName { get; set; }
        /// <summary>
        /// 入库人时间
        /// </summary>
        public DateTime? StoragedOn { get; set; }

        public virtual IEnumerable<StorePurchaseOrderItem> Items
        {
            get
            {
                return _items;
            }
        }

        //public void AddItem(int productSkuId, decimal contractPrice, int storePurchaseOrderId, int quantity)
        //{
        //    StorePurchaseOrderItem item = new StorePurchaseOrderItem(productSkuId, contractPrice, this.Id, quantity);
        //    this._items.Add(item);
        //}
        public void AddItems(List<StorePurchaseOrderItem> items)
        {
            this._items = items;
        }
        /// <summary>
        /// 更新收货明细中的 ，数量，生成日期，保质期
        /// </summary>
        /// <param name="items"></param>
        public void UpdateReceivedGoodsItems(List<StorePurchaseOrderItem> items)
        {
            Dictionary<int, StorePurchaseOrderItem> dic = new Dictionary<int, StorePurchaseOrderItem>();
            items.ForEach(n => dic.Add(n.Id, n));
            foreach (var item in this.Items)
            {
                item.ActualQuantity = dic[item.Id].ActualQuantity;
                item.ProductionDate = dic[item.Id].ProductionDate;
                item.ShelfLife = dic[item.Id].ShelfLife;
            }
        }

        public void Submit()
        {
            if (this.Status != PurchaseOrderStatus.Create)
            {
                throw new Exception("只能提交初始单据");
            }
            this.Status = PurchaseOrderStatus.WaitingStockIn;
        }

        public void ReceivedGoods()
        {
            if (this.Status != PurchaseOrderStatus.WaitingStockIn)
            {
                throw new Exception("待入库状态才能收货");
            }          
        }
       

        public void Cancel()
        {
            if (this.Status == PurchaseOrderStatus.HadStockIn)
            {
                throw new Exception("已入库单据不能作废");
            }
            this.Status = PurchaseOrderStatus.Cancel;
        }

    }
}
