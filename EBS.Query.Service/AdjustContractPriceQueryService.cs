using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using Dapper.DBContext;
using System.Dynamic;
using EBS.Infrastructure.Extension;

namespace EBS.Query.Service
{    
    public  class AdjustContractPriceQueryService:IAdjustContractPriceQuery
    {
         IQuery _query;
        public AdjustContractPriceQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<AdjustContractPriceDto> GetPageList(Pager page, SearchSupplierContract condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.Name))
            {
                where += "and t0.Name like @Name ";
                param.Name = string.Format("%{0}%", condition.Name);
            }
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and t0.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.SupplierId > 0)
            {
                where += "and t0.SupplierId=@SupplierId ";
                param.SupplierId = condition.SupplierId;
            }
            if (condition.StoreId > 0)
            {
                where += "and t0.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            if (condition.Status > 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            string sql = @"select t0.Id,t0.Name,t0.Code,t0.SupplierId,t0.Contact,t0.StartDate,t0.EndDate,t0.Status,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName  
from AdjustContractPrice t0 left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<AdjustContractPriceDto>(sql, param);
            page.Total = this._query.Count<AdjustContractPrice>(where, param);

            return rows;
        }


        public IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceItems(string productCodePriceInput)
        {
            if (string.IsNullOrEmpty(productCodePriceInput)) throw new Exception("商品明细为空");     
            var dic = GetProductDic(productCodePriceInput);
            string sql = "select p.Id as ProductId,p.Code,p.`Name`,p.Specification,c.FullName as CategoryName from Product p inner join category c on p.categoryId = c.Id where p.code in @Codes";
            var productItems= _query.FindAll<AdjustContractPriceItemDto>(sql, new { Codes = dic.Keys.ToArray() });
            foreach (var product in productItems)
            {
                product.ContractPrice = dic[product.Code];
            }
            return productItems;
        }

        public IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceItems(int AdjustContractPriceId)
        {
            string sql = "select pc.ProductId,p.Code,p.`Name`,p.Specification,c.FullName as CategoryName,pc.ContractPrice from AdjustContractPriceItem pc inner join  Product p on pc.ProductId=p.Id inner join category c on p.categoryId = c.Id where pc.AdjustContractPriceId = @AdjustContractPriceId";
            var productItems = _query.FindAll<AdjustContractPriceItemDto>(sql, new { AdjustContractPriceId = AdjustContractPriceId });
            return productItems;
        }


        public Dictionary<int, string> GetAdjustContractPriceStatus()
        {
            var dic = typeof(AdjustContractPriceStatus).GetValueToDescription();
            return dic;
        }

        private Dictionary<string, decimal> GetProductDic(string productIds)
        {
            Dictionary<string, decimal> dicProductPrice = new Dictionary<string, decimal>(1000);           
            string[] productIdArray = productIds.Split('\n');
            foreach (var item in productIdArray)
            {
                if (item.Contains("\t"))
                {
                    string[] parentIDAndQuantity = item.Split('\t');
                    if (!dicProductPrice.ContainsKey(parentIDAndQuantity[0].Trim()))
                    {                           
                        dicProductPrice.Add(parentIDAndQuantity[0].Trim(), decimal.Parse(parentIDAndQuantity[1]));
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
    }
}
