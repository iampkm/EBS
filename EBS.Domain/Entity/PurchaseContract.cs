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

       public string Code { get; set; }

       public string Name { get; set; }

       public int SupplierId { get; set; }
       public string Contact { get; set; }
       public CooperateWay Cooperate { get; set; }
       /// <summary>
       /// 合同开始日期
       /// </summary>
       public DateTime StartDate { get; set; }
       /// <summary>
       /// 合同开始日期
       /// </summary>
       public DateTime EndDate { get; set; }

       public DateTime CreatedOn { get; set; }
       public string CreatedBy { get; set; }
       public DateTime UpdatedOn { get; set; }
       public string UpdatedBy { get; set; }
       public PurchaseContractStatus Status { get; set; }

       public virtual List<PurchaseContractItem> Items { get; set; }

    }
}
