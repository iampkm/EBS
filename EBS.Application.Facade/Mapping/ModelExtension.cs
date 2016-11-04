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
       public static List<PurchaseContractItem> ConvertJsonToPurchaseContractItem(this CreatePurchaseContract source)
        {
            if (string.IsNullOrEmpty(source.Items)) throw new Exception("商品明细为空");
            var productPriceList = JsonConvert.DeserializeObject<List<PurchaseContractItem>>(source.Items);
            return productPriceList;
        }
       public static List<PurchaseContractItem> ConvertJsonToPurchaseContractItem(this EditPurchaseContract source)
        {
            if (string.IsNullOrEmpty(source.Items)) throw new Exception("商品明细为空");
            var productPriceList = JsonConvert.DeserializeObject<List<PurchaseContractItem>>(source.Items);
            return productPriceList;
        }
       public static List<StorePurchaseOrderItem> ConvertJsonToItem(this CreateStorePurchaseOrder source)
        {
            if (string.IsNullOrEmpty(source.Items)) throw new Exception("商品明细为空");
            var productPriceList = JsonConvert.DeserializeObject<List<StorePurchaseOrderItem>>(source.Items);
            return productPriceList;
        }
       public static List<StorePurchaseOrderItem> ConvertJsonToItem(this EditStorePurchaseOrder source)
       {
           if (string.IsNullOrEmpty(source.Items)) throw new Exception("商品明细为空");
           var productPriceList = JsonConvert.DeserializeObject<List<StorePurchaseOrderItem>>(source.Items);
           return productPriceList;
       }

       
       
    }
}
