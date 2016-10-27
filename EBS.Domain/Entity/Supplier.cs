using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Entity
{
    /// <summary>
    /// 供应商
    /// </summary>
   public class Supplier:BaseEntity
    {

       public Supplier() {
           this.CreatedOn = DateTime.Now;
           this.UpdatedOn = DateTime.Now;
       }

       public string Name { get; set; }
       /// <summary>
       /// 简称
       /// </summary>
       public string ShortName { get; set; }
       /// <summary>
       /// 联系人
       /// </summary>
       public string Contact { get; set; }
       /// <summary>
       /// 电话
       /// </summary>
       public string Phone { get; set; }
       /// <summary>
       /// 开户行
       /// </summary>
       public string Bank { get; set; }
       /// <summary>
       /// 税号
       /// </summary>
       public string TaxNo { get; set; }
       /// <summary>
       /// 开户行账号
       /// </summary>
       public string BankAccount { get; set; }
       /// <summary>
       /// 执照编号
       /// </summary>
       public string LicenseNo { get; set; }

       /// <summary>
       /// 供货区域
       /// </summary>
       public string AreaId { get; set; }

       public DateTime CreatedOn { get; set; }

       public int CreatedBy { get; set; }

       public DateTime UpdatedOn { get; set; }

       public int UpdatedBy { get; set; }
    }
}
