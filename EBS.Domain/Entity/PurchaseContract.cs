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

       public virtual List<PurchaseContractItem> Items { get; private set; }

       public void AddPurchaseContractItem(List<ProductSku> products, Dictionary<int, decimal> productPriceDic)
       {
           foreach (var product in products)
           {
               PurchaseContractItem item = new PurchaseContractItem()
               {                   
                   CostPrice = productPriceDic[product.Id],
                   ProductSkuId = product.Id
               };
               this.Items.Add(item);
           }
       }

       public void GenerateNewCode()
       {
           string billType = ((int)BillIdentity.PurchaseContract).ToString(); //  两位
           string date = DateTime.Now.ToString("yyyMMdd");
           Random rd = new Random(Guid.NewGuid().GetHashCode());
           string randomNumber = rd.Next(1, 1000).ToString().PadLeft(3,'0');
           StringBuilder builder = new StringBuilder(20);
           builder.AppendFormat("{0}{1}{2}", billType, date, randomNumber);
           this.Code = builder.ToString();
       }
    }
}
