using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
   public class TransaferOrderItemDto
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
        public int PackageQuantity { get; set; }
        /// <summary>
        /// 当前商品使用件规
        /// </summary>
        public int SpecificationQuantity
        {
            get;
            set;
        }
        /// <summary>
        /// 商品件规 单件数量 1*12 1*24 （逗号分隔字符串）
        /// </summary>
        public string ProductSpecificationQuantity { get; set; }

        public int[] SpecificationQuantitys
        {
            get
            {
                int[] array = new int[] { 1 };
                if (string.IsNullOrEmpty(ProductSpecificationQuantity))
                {
                    return array;
                }
                array = ProductSpecificationQuantity.Trim().Split(',').ToIntArray();
                return array;
            }
        }

        /// <summary>
        /// 预订数量
        /// </summary>
        public int Quantity { get; set; }

        public decimal Amount
        {
            get
            {
                return Price * Quantity;
            }
        }

       
        /// <summary>
        /// 生产日期
        /// </summary>
        public string ProductionDate { get; set; }
        //public DateTime? ProductionDate { get; set; }

        //public string ProductionTime { get {
        //    return ProductionDate.HasValue? ProductionDate.Value.ToString("yyyy-MM-dd"):"";
        //     } }

        /// <summary>
        /// 保质期：单位天
        /// </summary>
        public int ShelfLife { get; set; }

        /// <summary>
        ///  计算商品需要多少件
        /// </summary>
        public void SetSpecificationQuantity()
        {
            this.SpecificationQuantity = this.SpecificationQuantitys[0];
            this.PackageQuantity = this.Quantity / this.SpecificationQuantity;
        }

       

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
        /// <summary>
        /// 库存数量：退单使用
        /// </summary>
        public int InventoryQuantity { get; set; }

        public int BatchQuantity { get; set; }

        public string BatchNo { get; set; }

        /// <summary>
        ///  前端选择使用
        /// </summary>
        public bool Checked { get; set; }
    }
}
