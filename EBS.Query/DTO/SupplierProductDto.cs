using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;
namespace EBS.Query.DTO
{
    public class SupplierProductItemDto
    {  
        public int ProductId { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string Code { get; set; }

        public string CategoryName { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }
    }

    public class SupplierProductDto
    {
        public int ProductId { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string Code { get; set; }

        public string BarCode { get; set; }

        public string SupplierName { get; set; }


        public string BrandName { get; set; }

        public string CategoryName { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }

        public string SupplyStatus
        {
            get
            {
                return Status.Description();
            }
        }

        public SupplierProductStatus Status { get; set; }

        public string NickName { get; set; }

    }

    public class ProductPriceCompare
    {
        public int ProductId { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string Name { get; set; }

        public int Id1 { get; set; }

        public int SupplierId1 { get; set; }

        public decimal Price1 { get; set; }

        public SupplierProductStatus Status1 { get; set; }
        public ComparePriceStatus CompareStatus1 { get; set; }

        public string SupplyStatus1 { get {
                return Status1 == 0 ? "" : Status1.Description();
            } }
        public string ComparePriceStatus1
        {
            get
            {
                return CompareStatus1 == 0 ? "" : CompareStatus1.Description();
            }
        }

        public int Id2 { get; set; }

        public int SupplierId2 { get; set; }

        public decimal Price2 { get; set; }
        public SupplierProductStatus Status2 { get; set; }
        public ComparePriceStatus CompareStatus2 { get; set; }

        public string SupplyStatus2 { get {
                return Status2 == 0 ? "" : Status2.Description();
            } }
        public string ComparePriceStatus2
        {
            get
            {
                return CompareStatus2 == 0 ? "" : CompareStatus2.Description();
            }
        }

        public bool textColor1 {
            get {
                return this.Price1 < this.Price2;
            }
        }

        public bool textColor2 {
            get
            {
                return this.Price2 < this.Price1;
            }
        }
    }
}
