using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    /// <summary>
    /// 打印货架信息
    /// </summary>
    public class PrintShelfDto
    {
        public PrintShelfDto()
        {
            Items = new List<ShelfLayerProductDto>();

        }
        /// <summary>
        /// 货架码
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        public string StoreName{ get; set; }

        public int StoreId { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public IEnumerable<ShelfLayerProductDto> Items { get; set; }
    }
    /// <summary>
    /// 打印棚格图信息
    /// </summary>
   public class PrintShelfGridDto
    {
       public PrintShelfGridDto()
       {
           Layers = new List<ShelfLayerGridDto>();  
       }

        /// <summary>
        /// 货架码
        /// </summary>
        public string Code { get; set; }
        public string StoreName { get; set; }
        public string Name { get; set; }
        public int StoreId { get; set; }

        public int Id { get; set; }

        public IEnumerable<ShelfLayerGridDto> Layers { get; set; }
        /// <summary>
        /// 该货架的最大列数
        /// </summary>
        public int MaxColumn { get; set; }
    }

     /// <summary>
    /// 打印货架棚格图类
    /// </summary>
   public class ShelfLayerGridDto
   {
       public ShelfLayerGridDto()
       {
           Items = new List<ShelfLayerProductDto>();  
       }
       public int Id { get; set; }

       public int ShelfId { get; set; }

       public string Code { get; set; }

       public int Number { get; set; }

       public IEnumerable<ShelfLayerProductDto> Items { get; set; }
   }

   public class ShelfLayerProductDto
   {
       public int Id { get; set; }
       public int ProductId { get; set; }
       [Description("货架码")]
       public string Code { get; set; }
       [Description("商品名称")]
       public string ProductName { get; set; }
       [Description("商品编码")]
       public string ProductCode { get; set; }
       [Description("规格")]
       public string Specification { get; set; }
       [Description("商品条码")]
       public string BarCode { get; set; }
       public decimal SalesPrice { get; set; }
   }
}
