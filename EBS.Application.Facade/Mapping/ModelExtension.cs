using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
using EBS.Domain.Entity;
using Newtonsoft.Json;

namespace EBS.Application.Facade.Mapping
{
   public static class ModelExtension
    {
        public static void toDic(this CreatePurchaseContract source)
        {
            if (string.IsNullOrEmpty(source.Items)) throw new Exception("商品明细为空");
            var productPriceList = JsonConvert.DeserializeObject<List<ProductPriceModel>>(source.Items);
            Dictionary<int, decimal> productPriceDic = new Dictionary<int, decimal>();
            productPriceList.ForEach(n => productPriceDic.Add(n.Id, n.Price));
            source.ProductPriceDic =  productPriceDic;
        }
       
    }
}
