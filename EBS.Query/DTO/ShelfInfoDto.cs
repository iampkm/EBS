using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Query.DTO
{
    /// <summary>
    /// 货架信息，用来组织 货架树
    /// </summary>
   public class ShelfInfoDto
    {
        // 货架 
       public int Id { get; set; }

       public int StoreId { get; set; }

       public string Code { get; set; }

       public int Number { get; set; }

       public string Name { get; set; }

       // 货架层 
       /// <summary>
       /// 层Id
       /// </summary>
       public int LayerId { get; set; }
       public int ShelfId { get; set; }
       public int ShelfLayerNumber { get; set; }
       /// <summary>
       /// 货架码
       /// </summary>
       public string ShelfLayerCode { get; set; }

       //货架商品
       /// <summary>
       /// 货架商品Id
       /// </summary>
       public int ShelfLayerProductId { get; set; }

       public int ShelfLayerId { get; set; }

       public string ShelfLayerProductCode { get; set; }

       public int ShelfLayerProductNumber { get; set; }


    }
}
