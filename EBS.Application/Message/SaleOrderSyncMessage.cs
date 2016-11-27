using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.Message
{
   public class SaleOrderSyncMessage :IMessage
    {       
        public string Code { get; set; }

        public int StoreId { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PaymentWay { get; set; }
        /// <summary>
        /// 支付日期
        /// </summary>
        public DateTime PaidDate { get; set; }
        /// <summary>
        /// 订单金额 = 实际价格RealAmount * 数量
        /// </summary>
        public decimal OrderAmount { get; private set; }
        /// <summary>
        /// 客户支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 销售单状态
        /// </summary>

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

      //  public int IsSync { get; set; }

        public  List<SaleOrderItemSync> Items { get; set; }
    }

   public class SaleOrderItemSync
   {
       public int ProductId { get; set; }
       public string ProductName { get; set; }
       public string ProductCode { get; set; }
       /// <summary>
       /// 商品售价
       /// </summary>
       public decimal SalePrice { get; set; }
       /// <summary>
       /// 折扣
       /// </summary>
       public decimal Discount { get; set; }
       /// <summary>
       /// 实际折后价
       /// </summary>
       public decimal RealPrice { get; set; }
       public int Quantity { get; set; }

   }
}
