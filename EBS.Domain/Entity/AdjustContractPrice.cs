using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EBS.Domain.Entity
{
    public class AdjustContractPrice : BaseEntity
    {
        List<AdjustContractPriceItem> _items;
        public AdjustContractPrice()
        {
            this._items = new List<AdjustContractPriceItem>();
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Status = AdjustContractPriceStatus.WaitingAudit;
        }

        /// <summary>
        /// 调价单
        /// </summary>
        public string Code { get; set; }
        public int StoreId { get; set; }
        public int SupplierId { get; set; }  
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public AdjustContractPriceStatus Status { get; set; }

        public string Remark { get; set; }

        public virtual IEnumerable<AdjustContractPriceItem> Items
        {
            get
            {
                return this._items;
            }
        }
        public void AddItems(List<AdjustContractPriceItem> items)
        {
            foreach (var item in items)
            {
                item.AdjustContractPriceId = this.Id;
                if (!this._items.Exists(n => n.ProductId == item.ProductId))
                {
                    this._items.Add(item);
                }               
            }
        }

        public void SetItems(List<AdjustContractPriceItem> items)
        {
            this._items = items;
        }

        public void Submit()
        {
            if (this.Status != AdjustContractPriceStatus.WaitingAudit)
            {
                throw new Exception("只能提交新合同");
            }
            this.Status = AdjustContractPriceStatus.WaitingAudit;
        }

        public void CheckEditStatus()
        {
            if (this.Status != AdjustContractPriceStatus.WaitingAudit)
            {
                throw new Exception("只能编辑待审合同调价单");
            }
        }

        public void Audit()
        {
            if (this.Status != AdjustContractPriceStatus.WaitingAudit)
            {
                throw new Exception("只能审核待审合同调价单");
            }
            this.Status = AdjustContractPriceStatus.Audited;
        }

        public void Cancel()
        {
            if (this.Status == AdjustContractPriceStatus.Audited)
            {
                throw new Exception("已审合同调价单不能作废");
            }
            this.Status = AdjustContractPriceStatus.Cancel;
        }
        public void EditBy(int editBy)
        {
            this.UpdatedBy = editBy;
            this.UpdatedOn = DateTime.Now;
        }
    }
}
