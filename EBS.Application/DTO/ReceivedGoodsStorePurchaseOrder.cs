using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
    /// <summary>
    /// 采购单收货
    /// </summary>
    public class ReceivedGoodsStorePurchaseOrder
    {
        public int Id { get; set; }
        public int ReceivedBy { get; set; }
        public string ReceivedByName { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public string SupplierBill { get; set; }
        /// <summary>
        /// 商品明细json 串
        /// </summary>
        public string Items { get; set; }
    }
}
