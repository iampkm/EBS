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

       public static string StorePurchaseOrder
       {
           get
           {
               return "StorePurchaseOrder";
           }
       }
    }
}
