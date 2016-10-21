using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
   public class Product:BaseEntity
    {
       public Product() {
           this.SkuItems = new List<ProductSku>();
       }

       /// <summary>
       /// 商品名称，长度限制20个字符内
       /// </summary>
       public string Name { get; set; }
       /// <summary>
       /// 显示名称
       /// </summary>
       public string ShowName { get; set; }
       /// <summary>
       /// 卖点描述
       /// </summary>
       public string SellingPoint { get; set; }
       /// <summary>
       /// 进项税率
       /// </summary>
       public decimal InputRate { get; set; }
       /// <summary>
       /// 销项税率
       /// </summary>
       public decimal OutRate { get; set; }
   
       /// <summary>
       /// 关键字，用于SEO
       /// </summary>
       public string Keywords { get; set; }
       /// <summary>
       /// 是否赠品
       /// </summary>
       public bool IsGift { get; set; }
       /// <summary>
       /// 商品详情描述
       /// </summary>
       public string Description { get; set; }

       public virtual List<ProductSku> SkuItems { get; set; }
    }
}
