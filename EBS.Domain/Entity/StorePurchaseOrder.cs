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
        public string SupplierCode { get; set; }
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
        public DateTime ReceivedOn { get; set; }
        /// <summary>
        /// 入库人
        /// </summary>
        public int StoragedBy { get; set; }
        public string StoragedByName { get; set; }
        /// <summary>
        /// 入库人时间
        /// </summary>
        public DateTime StoragedOn { get; set; }

        public virtual IEnumerable<StorePurchaseOrderItem> Items
        {
            get
            {
                return _items;
            }
        }

        public void AddItem(int productSkuId, decimal contractPrice, int storePurchaseOrderId, int quantity)
        {
            StorePurchaseOrderItem item = new StorePurchaseOrderItem(productSkuId, contractPrice, this.Id, quantity);
            this._items.Add(item);
        }



    }
}
