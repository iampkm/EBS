﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Domain.ValueObject;
using EBS.Infrastructure.Extension;

namespace EBS.Query.Service
{
   public class SupplierQueryService:ISupplierQuery
    {
       IQuery _query;
       public SupplierQueryService(IQuery query)
        {
            this._query = query;
        }
       public IEnumerable<SupplierDto> GetPageList(Pager page, string name,string code)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (!string.IsNullOrEmpty(code))
            {
                where += "and t0.Code like @Code ";
                param.Code = code+"%";
            }           
            string sql = @"select t0.Id,t0.Name,t0.ShortName,t0.Code,t0.Contact,t0.QQ,t0.Phone,t0.Type  
from supplier t0 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<SupplierDto>(sql, param);
            page.Total = this._query.Count<Supplier>(where, param);

            return rows;
        }

        public IEnumerable<SupplierProductItemDto> GetSupplierProducts(string productCodePriceInput)
        {
            if (string.IsNullOrEmpty(productCodePriceInput)) throw new Exception("商品明细为空");
            var dic = productCodePriceInput.ToDecimalDic();
            string sql = "select p.Id as ProductId,p.Code,p.`Name`,p.Specification,c.FullName as CategoryName from Product p inner join category c on p.categoryId = c.Id where p.code in @Codes";
            var productItems = _query.FindAll<SupplierProductItemDto>(sql, new { Codes = dic.Keys.ToArray() });
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.Code))
                {
                    product.Price = dic[product.Code];
                }
               
            }
            return productItems;
        }

        public IEnumerable<SupplierProductDto> QuerySupplierProducts(Pager page, string name, string codeOrBarCode, string categoryId, int brandId, string supplierIds)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", name);
            }
            if (!string.IsNullOrEmpty(codeOrBarCode))
            {
                where += "and (t0.Code=@CodeOrBarCode or t0.BarCode=@CodeOrBarCode) ";
                param.CodeOrBarCode = codeOrBarCode;
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                where += "and t0.CategoryId like @CategoryId ";
                param.CategoryId = string.Format("{0}%", categoryId);
            }
            if (brandId > 0)
            {
                where += "and t0.BrandId=@BrandId ";
                param.BrandId = brandId;
            }
            if (!string.IsNullOrEmpty(supplierIds))
            {
                where += "and t3.SupplierId in @SupplierIds ";
                param.SupplierIds = supplierIds.Split(',').ToIntArray();
            }
            if (string.IsNullOrEmpty(where)) {
                return new List<SupplierProductDto>();
            }
            string sql = @"select t0.Id,t0.Name,t0.Code,t0.BarCode,t0.Specification,t1.Name as CategoryName,t2.Name as BrandName, t4.Name as SupplierName ,t3.Price
from product t0 inner join category t1 on t0.CategoryId = t1.Id
inner join brand t2 on t0.BrandId = t2.Id 
right join supplierproduct t3 on t3.ProductId = t0.Id
inner join supplier t4 on t3.SupplierId= t4.Id 
where 1=1 {0} ORDER BY t0.Id desc ";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
           // sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<SupplierProductDto>(sql, param);
           // page.Total = this._query.Count<Supplier>(where, param);
            return rows;

//            SELECT ProductId
//MAX(CASE SupplierId WHEN 1 THEN price ELSE 0 END ) Price1,
//MAX(CASE SupplierId WHEN 3 THEN price ELSE 0 END ) Price2 
//FROM supplierproduct  
//GROUP BY ProductId

        }

        public IDictionary<int, string> GetSupplierType()
        {
            return typeof(SupplierType).GetValueToDescription();
        }
    }
}
