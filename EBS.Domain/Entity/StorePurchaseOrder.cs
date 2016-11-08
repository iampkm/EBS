using System;
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
            foreach (var item in items)
            {
                item.StorePurchaseOrderId = this.Id;
                if (this._items.Exists(n => n.ProductId == item.ProductId))
                {
                    var productLine = this._items.Where(n => n.ProductId == item.ProductId).FirstOrDefault();
                    productLine.Quantity += item.Quantity;
                }
                else {
                    this._items.Add(item); 
                }
            }            
        }

        public void SetItems(List<StorePurchaseOrderItem> items)
        {
            this._items = items;
        }

        /// <summary>
        /// 更新收货明细中的 ，数量，生成日期，保质期
        /// </summary>
        /// <param name="items"></param>
        public string UpdateReceivedGoodsItems(List<StorePurchaseOrderItem> items)
        {
            Dictionary<int, StorePurchaseOrderItem> dic = new Dictionary<int, StorePurchaseOrderItem>();
            items.ForEach(n => dic.Add(n.Id, n));
            string result = "";
            foreach (var item in this._items)
            {
                if(dic.ContainsKey(item.Id))
                {
                    var diff = dic[item.Id].ActualQuantity - item.ActualQuantity;
                    item.ActualQuantity = dic[item.Id].ActualQuantity;
                    if (item.ActualQuantity < 0) { item.ActualQuantity = 0; }
                    if (item.ActualQuantity > item.Quantity) { item.ActualQuantity = item.Quantity; }
                    item.ProductionDate = dic[item.Id].ProductionDate;
                    item.ShelfLife = dic[item.Id].ShelfLife;
                    if (item.ShelfLife < 0) { item.ShelfLife = 0; }
                    if (diff > 0)
                    {
                        result += string.Format("收入{0},{1};", item.ProductId, diff);
                    }
                }
                           
            }
            return result;
        }

        public void Submit()
        {
            if (this.Status != PurchaseOrderStatus.Create)
            {
                throw new Exception("只能提交初始单据");
            }
            this.Status = PurchaseOrderStatus.WaitReceivedGoods;
        }

        public void ReceivedGoods()
        {
            if (this.Status != PurchaseOrderStatus.WaitingStockIn||this.Status!= PurchaseOrderStatus.WaitReceivedGoods)
            {
                throw new Exception("待收货或待入库状态才能收货");
            }
            this.Status = PurchaseOrderStatus.WaitingStockIn;
        }
        public void UpdateStatus(int editBy, string editor)
        {
            if (this.Status != PurchaseOrderStatus.WaitingStockIn)
            {
                throw new Exception("待入库状态才能入库");
            }
            this.StoragedBy = editBy;
            this.StoragedByName = editor;
            this.StoragedOn = DateTime.Now;
            this.Status = PurchaseOrderStatus.HadStockIn;
            this.GenerateBatchNo();
        }


        public void Cancel()
        {
            if (this.Status == PurchaseOrderStatus.HadStockIn)
            {
                throw new Exception("已入库单据不能作废");
            }
            this.Status = PurchaseOrderStatus.Cancel;
        }

        public void GenerateBatchNo()
        {
            var date = DateTime.Now;
            var ts = date - Convert.ToDateTime(date.ToShortDateString());
            var seconds = Math.Truncate(ts.TotalSeconds).ToString().PadLeft(6, '0');
           this.BatchNo= string.Format("{0}{1}", date.ToString("yyyyMMdd"),seconds);

        }

    }
}
