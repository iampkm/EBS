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
            this.OrderType = ValueObject.OrderType.Order;
        }

        public string Code { get; set; }
        public int SupplierId { get; set; }
        public string SupplierBill { get; set; }

        public OrderType OrderType { get; set; }
        public int StoreId { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public bool IsGift { get; set; }
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

        public void ReceivedGoods(int editBy, string editor)
        {
            if (this.Status == PurchaseOrderStatus.Create || this.Status == PurchaseOrderStatus.WaitStockIn || this.Status == PurchaseOrderStatus.WaitStockOut)
            {
                this.Status = this.OrderType == ValueObject.OrderType.Order ? PurchaseOrderStatus.WaitStockIn : PurchaseOrderStatus.WaitStockOut;
                this.ReceivedBy = editBy;
                this.ReceivedByName = editor;
                this.ReceivedOn = DateTime.Now;
            }
            else
            {
                throw new Exception("拣货状态异常");
            }
        }

        public void Finished(int editBy, string editor)
        {
            if (this.Status == PurchaseOrderStatus.WaitStockIn || this.Status == PurchaseOrderStatus.WaitStockOut)
            {               
                this.StoragedBy = editBy;
                this.StoragedByName = editor;
                this.StoragedOn = DateTime.Now;
                this.Status = PurchaseOrderStatus.Finished;
            }
            else
            {
                throw new Exception("请先捡货！");
            }           
        }

        public void FinanceAuditd(int editBy, string editor)
        {
            if (this.Status == PurchaseOrderStatus.Finished)
            {
                this.Status = PurchaseOrderStatus.FinanceAuditd;
            }
            else {
                throw new Exception("单据未结束，不能进行财务审核");
            }
        }

        /// <summary>
        /// 取消已审
        /// </summary>
        /// <param name="editBy"></param>
        /// <param name="editor"></param>
        public void CancelAudited(int editBy, string editor)
        {
            if (this.Status == PurchaseOrderStatus.FinanceAuditd)
            {
                this.Status = PurchaseOrderStatus.Finished;
            }
            else
            {
                throw new Exception("财务审核已审单据才能撤销");
            }
        }
       
        /// <summary>
        /// 更新收货明细中的 ，数量，生成日期，保质期
        /// </summary>
        /// <param name="items"></param>
        public void UpdateReceivedGoodsItems(List<StorePurchaseOrderItem> items)
        {
            Dictionary<int, StorePurchaseOrderItem> dic = new Dictionary<int, StorePurchaseOrderItem>();
            items.ForEach(n => dic.Add(n.Id, n));
            foreach (var item in this._items)
            {
                if (dic.ContainsKey(item.Id))
                {                   
                    item.ActualQuantity = dic[item.Id].ActualQuantity;
                    if (item.ActualQuantity < 0) { item.ActualQuantity = 0; }
                    if (item.ActualQuantity > item.Quantity) { item.ActualQuantity = item.Quantity; }
                    item.ProductionDate = dic[item.Id].ProductionDate;
                    item.ShelfLife = dic[item.Id].ShelfLife;
                    if (item.ShelfLife < 0) { item.ShelfLife = 0; }
                }

            }
        }

        public void Cancel()
        {
            if (this.Status == PurchaseOrderStatus.Finished)
            {
                throw new Exception("已完成单据不能作废");
            }
            this.Status = PurchaseOrderStatus.Cancel;
        }

    }
}
