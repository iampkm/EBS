using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;

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
           string[] productIdArray = value.Trim('\n').Split('\n');
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
           string[] productIdArray = value.Trim('\n').Split('\n');
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

        /// <summary>
        /// 创建二维码/条形码
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string CreateBarCodeUri(this string content, string type)
        {
           // LogWriter.WriteLog("content:" + content + " type:" + type);
            try
            {
                Bitmap bitmap = null;
                string enCodeString = "http://m.sjgo365.com/{0}.html";
                switch (type)
                {
                    case "barCode":
                        enCodeString = content;
                        BarCodeHelper barCodeHelper = new BarCodeHelper();
                        bitmap = barCodeHelper.GetCodeImage(content, BarCodeHelper.Encode.Code128A);
                        break;
                    case "QRCode":
                        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//二维码编码方式
                        qrCodeEncoder.QRCodeScale = 2;//每个小方格的宽度
                        qrCodeEncoder.QRCodeVersion = 5;//二维码版本号
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;//纠错码等级
                        bitmap = qrCodeEncoder.Encode(string.Format(enCodeString, content), Encoding.UTF8);
                        break;
                }
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                string strUrl = "data:image/jpeg;base64," + Convert.ToBase64String(ms.ToArray());
                bitmap.Dispose();
                ms.Dispose();
                return strUrl;
            }
            catch (Exception ex)
            {
               // LogWriter.WriteLog("异常信息：" + ex.Message);
                return null;
            }
        }
    }
}
