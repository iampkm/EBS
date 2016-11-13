using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class AdjustSalePrice:BaseEntity
    {
        List<AdjustSalePriceItem> _items;
        public AdjustSalePrice()
        {
            this._items = new List<AdjustSalePriceItem>();
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Status = AdjustSalePriceStatus.InValid;
        }
        //public string Name { get; set; }
        /// <summary>
        /// 调价单
        /// </summary>
        public string Code { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public AdjustSalePriceStatus Status { get; set; }

        public virtual IEnumerable<AdjustSalePriceItem> Items
        {
            get
            {
                return this._items;
            }
        }

        public void AddItems(List<AdjustSalePriceItem> items)
        {
            foreach (var item in items)
            {
                item.AdjustSalePriceId = this.Id;
                if (!this._items.Exists(n => n.ProductId == item.ProductId))
                {
                    this._items.Add(item);
                }
            }
        }

        public void AddItem(Product product, decimal adjustPrice)
        {
            AdjustSalePriceItem item = new AdjustSalePriceItem();
            item.SalePrice = product.SalePrice;
            item.AdjustPrice = adjustPrice;
            item.ProductId = product.Id;
            this._items.Add(item); 
        }   

        public void Submit()
        {
            if (this.Status != AdjustSalePriceStatus.InValid)
            {
                throw new Exception("只能提交新单据");
            }
            this.Status = AdjustSalePriceStatus.Valid;
        }

        public void Audit()
        {
            //if (this.Status != AdjustSalePriceStatus.Valid)
            //{
            //    throw new Exception("只能审核待审单据");
            //}
            //this.Status = AdjustSalePriceStatus.Audited;
            throw new Exception("未实现");
        }

        public void Cancel()
        {
            if (this.Status == AdjustSalePriceStatus.Valid)
            {
                throw new Exception("已生效单据不能作废");
            }
            this.Status = AdjustSalePriceStatus.Cancel;
        }
        public void EditBy(int editBy)
        {
            this.UpdatedBy = editBy;
            this.UpdatedOn = DateTime.Now;
        }
    }
}
