using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public class FormType
    {
       /// <summary>
       /// 采购合同
       /// </summary>
       public static string PurchaseContract { get {
           return "PurchaseContract";
       } }
       /// <summary>
       /// 门店采购单
       /// </summary>
       public static string StorePurchaseOrder
       {
           get
           {
               return "StorePurchaseOrder";
           }
       }
       /// <summary>
       /// 调整合同价单
       /// </summary>
       public static string AdjustContractPrice
       {
           get
           {
               return "AdjustContractPrice";
           }
       } 
    }
}
