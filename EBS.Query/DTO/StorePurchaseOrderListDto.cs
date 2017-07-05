using EBS.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class StorePurchaseOrderListDto
    {
       public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }

        public string Supplier
        {
            get
            {
                return string.Format("[{0}]{1}", SupplierCode, SupplierName);
            }
        }
        public string StoreName { get; set; }
        public PurchaseOrderStatus Status { get; set; }

        public string PurchaseOrderStatus
        {
            get
            {
                return Status.Description();
            }
        }
        public DateTime CreatedOn { get; set; }

        public string CreatedTime
        {
            get
            {
                return CreatedOn.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string CreatedByName { get; set; }

        /// <summary>
        /// 供应商备注：可以备注单号，其他信息
        /// </summary>
        public string SupplierBill { get; set; }


        public DateTime StoragedOn { get; set; }
        public string StoragedTime
        {
            get
            {
                return StoragedOn.ToString("yyyy-MM-dd HH:mm");
            }
        }

        // 明细数据
        public string ProductCode { get; set; }

        public string ProductCodeAndBarCode { get {
            var result = string.Format("{0} | {1}", this.ProductCode, this.BarCode);
            return result;
        } }
        public string BarCode { get; set; }

        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        public string ProductNameAndGift {
            get {
                var result = string.Format("{0} {1}", this.ProductName, this.IsGift ? "<span class='text-danger'>[赠]</span>" : "");
                return result;
            }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        public int Quantity { get; set; }

        public int ActualQuantity { get; set; }      

        public decimal ContractPrice { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
        
        /// <summary>
        /// 是否赠品
        /// </summary>
        public bool IsGift { get; set; }
    }
}
