using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Entity
{
    /// <summary>
    /// 采购合同
    /// </summary>
    public class PurchaseContract : BaseEntity
    {
        List<PurchaseContractItem> _items;
        public PurchaseContract()
        {
            this._items = new List<PurchaseContractItem>();
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Status = PurchaseContractStatus.Create;
        }

        /// <summary>
        /// 合同代码，自己录入
        /// </summary>
        public string Code { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 门店字符串：多个门店，逗号分隔
        /// </summary>
        public string StoreIds { get; set; }
        public int SupplierId { get; set; }
        public string Contact { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 合同开始日期
        /// </summary>
        public DateTime EndDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public PurchaseContractStatus Status { get; set; }

        public virtual IEnumerable<PurchaseContractItem> Items { get {
            return this._items;
        } }

        public void AddPurchaseContractItem(List<PurchaseContractItem> items)
        {
            this._items = items;
        }

        public void Submit()
        {
            if (this.Status != PurchaseContractStatus.Create)
            {
                throw new Exception("只能提交新合同");
            }
            this.Status = PurchaseContractStatus.WaitingAudit;
        }

        public void Audit()
        {
            if (this.Status != PurchaseContractStatus.WaitingAudit)
            {
                throw new Exception("只能审核待审合同");
            }
            this.Status = PurchaseContractStatus.Audited;
            //修改商品的状态为供货中
            this._items.ForEach(n => n.Status = SupplyStatus.Supplying);
        }

        public void Cancel()
        {
            //if (this.Status == PurchaseContractStatus.Audited)
            //{
            //    throw new Exception("已审合同不能作废");
            //}
            this.Status = PurchaseContractStatus.Cancel;
        }

        public void EditBy(int editBy)
        {
            this.UpdatedBy = editBy;
            this.UpdatedOn = DateTime.Now;
        }

        /// <summary>
        /// 门店ID 字符串
        /// </summary>
        public List<string> GetStores() {          
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(this.StoreIds)) return list;
                list.AddRange(this.StoreIds.Split(','));
            return list;           
        }

    }
}
