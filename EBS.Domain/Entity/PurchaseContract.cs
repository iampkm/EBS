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
   public class PurchaseContract:BaseEntity
    {

       public PurchaseContract() {
           this.Items = new List<PurchaseContractItem>();
           this.CreatedOn = DateTime.Now;
           this.UpdatedOn = DateTime.Now;
       }

       /// <summary>
       /// 合同代码，自己录入
       /// </summary>
       public string Code { get; set; }

       public string Name { get; set; }
       public int StoreId { get; set; }
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

       public virtual List<PurchaseContractItem> Items { get; private set; }

       public void AddPurchaseContractItem(List<ProductSku> products, Dictionary<int, decimal> productPriceDic)
       {
           foreach (var product in products)
           {
                PurchaseContractItem item = new PurchaseContractItem()
                {
                   PurchaseContractId = this.Id,                 
                   CostPrice = productPriceDic[product.Id],
                   ProductSkuId = product.Id
               };
               this.Items.Add(item);
           }
       }

       public void Submit() {
           if (this.Status != PurchaseContractStatus.Create) {
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
       }

       public void Cancel()
       {
           if (this.Status == PurchaseContractStatus.Audited)
           {
               throw new Exception("已审合同不能作废");
           }
           this.Status = PurchaseContractStatus.Cancel;
       }

       public void EditBy(int editBy)
       {
           this.UpdatedBy = editBy;
           this.UpdatedOn = DateTime.Now;
       }
      
    }
}
