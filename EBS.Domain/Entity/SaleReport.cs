using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 销售分析报表
    /// </summary>
   public class SaleReport
    {
       public SaleReport() {
           this.UpdatedOn = DateTime.Now;
       }
       /// <summary>
       /// 库存流水ID ，唯一标记
       /// </summary> 
       public int StoreInventoryHistoryId { get; set; }
       public int SaleOrderId { get; set; }
       public int ProductId { get; set; }           
       public int StoreId { get; set; }
       public int SupplierId { get; set; }

       public int OrderType { get; set; }
       public PaymentWay PaymentWay { get; set; }

       public SaleOrderLevel OrderLevel { get; set; }
       /// <summary>
       /// 成本价
       /// </summary>
       public decimal CostPrice { get; set; }
       /// <summary>
       /// 售价
       /// </summary>
       public decimal SalePrice { get; set; }
       /// <summary>
       /// 实际售价
       /// </summary>
       public decimal RealPrice { get; set; }
       /// <summary>
       /// 销售数量
       /// </summary>
       public int Quantity { get; set; }

       public DateTime CreatedOn { get; set; }
       public int CreatedBy { get; set; }
       public DateTime UpdatedOn { get; set; }
    }
}
