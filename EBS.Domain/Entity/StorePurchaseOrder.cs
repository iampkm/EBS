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
        public StorePurchaseOrder() {
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Items = new List<StorePurchaseOrderItem>();
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

        public virtual List<StorePurchaseOrderItem> Items { get; private set; }

        public void AddItems(StorePurchaseOrderItem item)        
        {
            this.Items.Add(item);
            this.Total = this.Items.Sum(n => n.Price * n.ActualQuantity);
        }

    }
}
