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
            string sql = @"select t0.Id,t0.Name,t0.ShortName,t0.Code,t0.Contact,t0.QQ,t0.Phone,t0.Type,t0.Bank,t0.BankAccount,t0.BankAccountName   
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
                //这里只提供条码查
               var codeOrBarCodeArray= codeOrBarCode.Trim('\n').Split('\n');
               //var codeArray = codeOrBarCodeArray.Where(n => n.Length <= 10).ToArray();
               //var barCodeArray = codeOrBarCodeArray.Where(n => n.Length >= 10).ToArray();
              // where += "and (t0.Code in @codeArray or t0.BarCode in @barCodeArray) ";
               where += "and t0.BarCode in @barCodeArray ";
              // param.codeArray = codeArray;
               param.barCodeArray = codeOrBarCodeArray;
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
            string sql = @"select t0.Id as ProductId,t0.Name,t0.Code,t0.BarCode,t0.Specification,t1.FullName as CategoryName,t2.Name as BrandName, t4.Name as SupplierName ,t3.Price,t3.Status,t5.NickName
from product t0 inner join category t1 on t0.CategoryId = t1.Id
inner join brand t2 on t0.BrandId = t2.Id 
right join supplierproduct t3 on t3.ProductId = t0.Id
inner join supplier t4 on t3.SupplierId= t4.Id 
inner join account t5 on t5.Id = t3.UpdatedBy
where 1=1 {0} ORDER BY t0.Id desc ";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
           // sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<SupplierProductDto>(sql, param);
           // page.Total = this._query.Count<Supplier>(where, param);
            return rows;

        }

        public IEnumerable<ProductPriceCompare> QuerySupplierProductCompare(int supplierId1,int supplierId2,string productIds)
        {
            if (supplierId2==0) {
                return QuerySupplierProductCompare(supplierId1, productIds);
            }

            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(productIds))
            {
                where += "where p.Id in @ProductIds ";
                param.ProductIds = productIds.Split(',').ToIntArray();
            }
            param.SupplierId1 = supplierId1;
            param.SupplierId2 = supplierId2;

            //string countSql1 = "select count(*) from supplierproduct where SupplierId = @SupplierId";
            //string countSql2 = "select count(*) from supplierproduct where SupplierId = @SupplierId";
            //var result1= _query.Context.ExecuteScalar<int>(countSql1, new { SupplierId = supplierId1 });
            //var result2 = _query.Context.ExecuteScalar<int>(countSql2, new { SupplierId = supplierId2 });
            //if (result2 > result1)
            //{
            //    param.SupplierId1 = supplierId2;
            //    param.SupplierId2 = supplierId1;
            //}

            string sql = @"select p.Id as ProductId, p.code,p.`Name`,s1.Id as Id1,s1.SupplierId as SupplierId1 ,
S1.Price as Price1,s1.Status as Status1,s1.CompareStatus as CompareStatus1,S2.Id as Id2,IFNULL(S2.SupplierId,0) as SupplierId2,IFNULL(S2.Price,0) as Price2,
IFNULL(s2.Status,0) as Status2 ,IFNULL(s2.CompareStatus,0) as CompareStatus2
from(select * from supplierproduct where SupplierId = @SupplierId1 ) s1
LEFT JOIN(select * from supplierproduct where SupplierId = @SupplierId2 ) s2
on s1.ProductId = s2.ProductId
left join product p on p.Id = s1.ProductId
{0} order by p.Id ";
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<ProductPriceCompare>(sql, param);
            return rows;

        }
        private IEnumerable<ProductPriceCompare> QuerySupplierProductCompare(int supplierId1, string productIds)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(productIds))
            {
                where += "where p.Id in @ProductIds ";
                param.ProductIds = productIds.Split(',').ToIntArray();
            }
            param.SupplierId1 = supplierId1;

            string sql = @"select p.Id as ProductId, p.code,p.`Name`,s1.Id as Id1,s1.SupplierId as SupplierId1 ,
S1.Price as Price1,s1.Status as Status1,s1.CompareStatus as CompareStatus1
from(select * from supplierproduct where SupplierId = @SupplierId1 ) s1
left join product p on p.Id = s1.ProductId
{0} order by p.Id ";
            sql = string.Format(sql, where);
            var rows = this._query.FindAll<ProductPriceCompare>(sql, param);
            return rows;

        }

        public IDictionary<int, string> GetSupplierType()
        {
            return typeof(SupplierType).GetValueToDescription();
        }

       
    }
}
