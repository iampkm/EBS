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
            this.UpdatedOn = DateTime.Now;
            _items = new List<StorePurchaseOrderItem>();
            this.Status = PurchaseOrderStatus.Create;
        }
        /// <summary>
        /// 合同单号
        /// </summary>
        public int PurchaseContractId { get; set; }
        public string Code { get; set; }
        public int SupplierId { get; set; }
        public int StoreId { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

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
