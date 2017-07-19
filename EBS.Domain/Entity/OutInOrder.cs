using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Entity
{
    /// <summary>
    /// 其他出入库单
    /// </summary>
    public class OutInOrder : BaseEntity
    {
        public OutInOrder() {
            this.Items = new List<OutInOrderItem>();
            this.Status = OutInOrderStatus.Create;
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
        }
        public string Code { get; set; }

        public int SupplierId { get; set; }

        public int StoreId { get; set; }

        public int OutInOrderTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public string UpdatedByName { get; set; }
        public OutInOrderStatus Status { get; set; }

        public virtual List<OutInOrderItem> Items { get; set; }

        public void SetList(List<OutInOrderItem> items,OutInOrderType orderType)
        {
            foreach (var line in items)
            {
                line.SetPlusMinus(orderType);
                if (this.Items.Exists(n => n.ProductId == line.ProductId))
                {
                    var first = this.Items.FirstOrDefault(n => n.ProductId == line.ProductId);
                    first.Quantity += line.Quantity;
                }
                else {
                    this.Items.Add(line);
                }
            }           
        }

        public void Audit(int editBy, string editByName)
        {
            if (this.Status != OutInOrderStatus.WaitAudit)
            {
                throw new Exception("必须是待审单据");
            }
            this.Status = OutInOrderStatus.Audited;
            EditBy(editBy, editByName);
        }

        public void Cancel(int editBy, string editByName)
        {
            if (this.Status == OutInOrderStatus.Audited)
            {
                throw new Exception("已审单据不能作废");
            }
            this.Status = OutInOrderStatus.Cancel;
            EditBy(editBy, editByName);
        }

        public void Submit(int editBy, string editByName)
        {
            if (this.Status != OutInOrderStatus.Create)
            {
                throw new Exception("只能提交初始状态的单据");
            }
            this.Status = OutInOrderStatus.WaitAudit;
            EditBy(editBy, editByName);
        }

        public void Reject(int editBy, string editByName)
        {
            if (this.Status != OutInOrderStatus.WaitAudit)
            {
                throw new Exception("只能驳回待审单据");
            }
            this.Status = OutInOrderStatus.Create;
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
