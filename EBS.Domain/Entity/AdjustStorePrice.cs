using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class AdjustStorePrice:BaseEntity
    {
       List<AdjustStorePriceItem> _items;
       public AdjustStorePrice()
        {
            this._items = new List<AdjustStorePriceItem>();
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Status = AdjustStorePriceStatus.Create;
        }
        public int StoreId { get; set; }
        /// <summary>
        /// 调价单
        /// </summary>
        public string Code { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public AdjustStorePriceStatus Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public virtual IEnumerable<AdjustStorePriceItem> Items
        {
            get
            {
                return this._items;
            }
        }

        public void AddItems(List<AdjustStorePriceItem> items)
        {
            foreach (var item in items)
            {
                item.AdjustStorePriceId = this.Id;
                if (!this._items.Exists(n => n.ProductId == item.ProductId))
                {
                    this._items.Add(item);
                }
            }
        }

        public void AddItem(Product product, decimal adjustPrice)
        {
            AdjustStorePriceItem item = new AdjustStorePriceItem();
            item.StoreSalePrice = product.SalePrice;
            item.AdjustPrice = adjustPrice;
            item.ProductId = product.Id;
            this._items.Add(item); 
        }   

        public void Submit()
        {
            if (this.Status != AdjustStorePriceStatus.Create)
            {
                throw new Exception("只能提交新单据");
            }
            this.Status = AdjustStorePriceStatus.WaitAudit;
        }

        public void Audit()
        {
            if (this.Status != AdjustStorePriceStatus.WaitAudit)
            {
                throw new Exception("只能审核待审单据");
            }
            this.Status = AdjustStorePriceStatus.Audited;         
        }

        public void Reject()
        {
            if (this.Status != AdjustStorePriceStatus.WaitAudit)
            {
                throw new Exception("只能驳回待审单据");
            }
            this.Status = AdjustStorePriceStatus.Create;       
        }

        public void Cancel()
        {
            if (this.Status == AdjustStorePriceStatus.Audited)
            {
                throw new Exception("已生效单据不能作废");
            }
            this.Status = AdjustStorePriceStatus.Cancel;
        }
        public void EditBy(int editBy)
        {
            this.UpdatedBy = editBy;
            this.UpdatedOn = DateTime.Now;
        }
    }
}
