using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
   public class StorePurchaseOrderItemDto
    {
       public int productSkuId { get; set; }

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
       /// 预订数量
       /// </summary>
       public int Quantity { get; set; }

       /// <summary>
       /// 预订件数
       /// </summary>
       public int WholeQuantity { get; set; }

       /// <summary>
       /// 单件数量
       /// </summary>
       public int SubSkuQuantity { get; set; }
       /// <summary>
       /// 实收件数
       /// </summary>
       public int ActualWholeQuantity { get; set; }
       /// <summary>
       /// 实收数量
       /// </summary>
       public int ActualQuantity { get; set; }
       /// <summary>
       /// 生产日期
       /// </summary>
       public DateTime ProductionDate { get; set; }

       /// <summary>
       /// 保质期：单位天
       /// </summary>
       public int ShelfLife { get; set; }    


    }
}
