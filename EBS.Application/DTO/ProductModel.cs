﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application.DTO
{
   public class ProductModel
    {
       public ProductModel()
       {
           this.UpdatedOn = DateTime.Now;
       }

       public int Id { get; set; }
        /// <summary>
        /// 商品名称，长度限制20个字符内
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string ShowName { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int BrandId { get; set; }

        public string BrandName { get; set; }

        /// <summary>
        /// 卖点描述
        /// </summary>
        public string SellingPoint { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
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
        /// 长  单位：厘米
        /// </summary>
        public decimal Length { get; set; }
        /// <summary>
        ///  宽  单位：厘米
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        ///高  单位：厘米
        /// </summary>
        public decimal Height { get; set; }
        /// <summary>
        /// 重量 单位：  千克
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 商品详情描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 件规： 默认1. 进货时整件规格数量 1*12，1*24 等 多个逗号分隔  例如 1，12，24，19
        /// </summary>
        public string SpecificationQuantity { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OldtPrice { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 批发价
        /// </summary>
        public decimal WholeSalePrice { get; set; }
        /// <summary>
        /// 移动平均成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 子SKU编码 ：  最小进货单位 拆零使用
        /// </summary>
        public string SubSkuCode { get; set; }
        /// <summary>
        /// 子SKU 可拆数量  ： 用于整件拆零，
        /// </summary>
        public int SubSkuQuantity { get; set; }

        public string Unit { get; set; }
        /// <summary>
        /// 创建时间/修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
       /// <summary>
       /// 修改人
       /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// 产地
        /// </summary>
        public string MadeIn { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Grade { get; set; }



    }
}
