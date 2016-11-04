using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class StorePurchaseOrderItemDto
    {
       public int ProductId { get; set; }

       public string BarCode { get; set; }

       public string ProductCode { get; set; }

       public string ProductName { get; set; }
       /// <summary>
       /// 规格
       /// </summary>
       public string Specification { get; set; }
       /// <summary>
       /// 单位
       /// </summary>
       public string Unit { get; set; }      

       public decimal ContractPrice { get; set; }

       public decimal Price { get; set; }
      
        /// <summary>
        /// 件数： 1件，2件
        /// </summary>
       public  int PackageQuantity { get; set;}
        /// <summary>
        /// 单件数量 1*12 1*24
        /// </summary>
       public string SpecificationQuantity { get; set; }

       public int[] SpecificationQuantitys
       {
           get
           {
               int[] array = new int[] { 1 };
               if (string.IsNullOrEmpty(SpecificationQuantity))
               {
                   return array;
               }
               array = SpecificationQuantity.Trim().Split(',').ToIntArray();
               return array;
           }
       }

        /// <summary>
        /// 预订数量
        /// </summary>
        public int Quantity { get; set; }
      
       /// <summary>
       /// 实收件数
       /// </summary>
       public int ActualPackageQuantity { get; set; }
       /// <summary>
       /// 实收数量
       /// </summary>
       public int ActualQuantity { get; set; }
       /// <summary>
       /// 生产日期
       /// </summary>
       public string ProductionDate { get; set; }

       /// <summary>
       /// 保质期：单位天
       /// </summary>
       public int ShelfLife { get; set; }    


    }
}
