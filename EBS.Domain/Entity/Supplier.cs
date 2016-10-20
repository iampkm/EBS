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
       /// 合作方式
       /// </summary>
       public CooperateWay Cooperate { get; set; }
    }
}
