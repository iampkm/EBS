using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
namespace EBS.Domain.Entity
{
   public class StoreInventoryHistory:BaseEntity
    {
        /// <summary>
        /// 商品SKUID
        /// </summary>
        public int ProductSkuId { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
         public int StoreId { get; set; }
       /// <summary>
       /// 库存数
       /// </summary>
         public int Quantity { get; set; }
       /// <summary>
       /// 变动数
       /// </summary>
         public int ChangeQuantity { get; set; }
       /// <summary>
       /// 批次价格
       /// </summary>
         public decimal Price { get; set; }
       /// <summary>
       /// 批次号
       /// </summary>
         public string BatchNo { get; set; }  
       /// <summary>
       /// 单据编号
       /// </summary>
         public int BillId { get; set; }
       /// <summary>
       /// 单据编码
       /// </summary>
         public string BillCode { get; set; }
         public BillIdentity BillType { get; set; }
         public DateTime CreatedOn { get; set; }
         public int CreatedBy { get; set; }

       
    }
}
