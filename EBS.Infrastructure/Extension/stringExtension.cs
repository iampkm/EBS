using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
namespace EBS.Infrastructure.Extension
{
   public static class stringExtension
   {
       /// <summary>
       /// 把excel格式数据转换成 string,decimal 字典
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public static Dictionary<string, decimal> ToDecimalDic(this string value)
       {
           Dictionary<string, decimal> dicProductPrice = new Dictionary<string, decimal>(1000);
           string[] productIdArray = value.Split('\n');
           foreach (var item in productIdArray)
           {
               if (item.Contains("\t"))
               {
                   string[] parentIDAndQuantity = item.Split('\t');
                   if (!dicProductPrice.ContainsKey(parentIDAndQuantity[0].Trim()))
                   {
                       decimal quantity = 0m;
                       decimal.TryParse(parentIDAndQuantity[1], out quantity);
                       dicProductPrice.Add(parentIDAndQuantity[0].Trim(), quantity);
                   }
               }
               else
               {
                   if (!dicProductPrice.ContainsKey(item.Trim()))
                   {
                       dicProductPrice.Add(item.Trim(), 0);
                   }
               }
           }

           return dicProductPrice;
       }
       /// <summary>
       /// 把excel格式数据转换成 string,int 字典
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public static Dictionary<string, int> ToIntDic(this string value)
       {
           Dictionary<string, int> dicProductPrice = new Dictionary<string, int>(1000);
           string[] productIdArray = value.Split('\n');
           foreach (var item in productIdArray)
           {
               if (item.Contains("\t"))
               {
                   string[] parentIDAndQuantity = item.Split('\t');
                   if (!dicProductPrice.ContainsKey(parentIDAndQuantity[0].Trim()))
                   {
                       int quantity = 0;
                       int.TryParse(parentIDAndQuantity[1], out quantity);
                       dicProductPrice.Add(parentIDAndQuantity[0].Trim(), quantity);
                   }
               }
               else
               {
                   if (!dicProductPrice.ContainsKey(item.Trim()))
                   {
                       dicProductPrice.Add(item.Trim(), 0);
                   }
               }
           }

           return dicProductPrice;
       }

       public static bool IsNumeric(this string value)
       {
           return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
       }
   }
}
