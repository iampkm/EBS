using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 调拨单 明细
    /// </summary>
   public class TransferOrder:BaseEntity
    {

       public TransferOrder()
       {
           this.CreatedOn = DateTime.Now;
           this.UpdatedOn = DateTime.Now;
           this.Status = TransferOrderStatus.Create;
       }

        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public string Code { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public string UpdatedByName { get; set; }
        public TransferOrderStatus Status { get; set; }

        public virtual List<TransferOrderItem> Items { get; set; }

        public void Audit(int editBy,string editByName)
        {
            if (this.Status != TransferOrderStatus.WaitAudit)
            {
                throw new Exception("必须是待审调拨单");
            }
            this.Status = TransferOrderStatus.Audited;
            EditBy(editBy, editByName);
        }

        public void Cancel(int editBy,string editByName)
        {
            if (this.Status == TransferOrderStatus.Audited)
            {
                throw new Exception("已审调拨单不能作废");
            }
            this.Status = TransferOrderStatus.Cancel;
            EditBy(editBy, editByName);
        }

        public void Submit(int editBy, string editByName)
        {
            if (this.Status != TransferOrderStatus.Create)
            {
                throw new Exception("只能提交初始状态的单据");
            }
            this.Status = TransferOrderStatus.WaitAudit;
            EditBy(editBy, editByName);
        }

        public void Reject(int editBy, string editByName)
        {
            if (this.Status != TransferOrderStatus.WaitAudit)
            {
                throw new Exception("只能驳回待审单据");
            }
            this.Status = TransferOrderStatus.Create;
            EditBy(editBy, editByName);
        }

        private void EditBy(int editBy, string editByName)
        {
            this.UpdatedBy = editBy;
            this.UpdatedByName = editByName;
            this.UpdatedOn = DateTime.Now;
        }
    }
}
