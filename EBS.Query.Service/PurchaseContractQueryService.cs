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
   public class PurchaseContractQueryService:IPurchaseContractQuery
    {
        IQuery _query;
        public PurchaseContractQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<PurchaseContractDto> GetPageList(Pager page, string code, string name, int supplierId)
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
                where += "and t0.Code=@Code ";
                param.Code = code;
            }           
            if (supplierId > 0)
            {
                where += "and t0.SupplierId=@SupplierId ";
                param.SupplierId = supplierId;
            }
            string sql = @"select t0.Id,t0.Name,t0.Code,t0.SupplierId,t0.Contact,t0.Cooperate,t0.StartDate,t0.EndDate,t0.Status,t1.Name as SupplierName  
from PurchaseContract t0 inner join supplier t1 on t0.SupplierId = t1.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<PurchaseContractDto>(sql, param);
            page.Total = this._query.Count<PurchaseContract>(where, param);

            return rows;
        }


        public IEnumerable<PurchaseContractItemDto> GetPurchaseContractItems(string productCodePriceInput)
        {
            if (string.IsNullOrEmpty(productCodePriceInput)) throw new Exception("商品明细为空");     
            var dic = GetProductDic(productCodePriceInput);
            string sql = "select p.Id,p.Code,p.`Name`,p.Specification,c.FullName as CategoryName from productsku p inner join category c on p.categoryId = c.Id where p.code in @Codes";
            var productItems= _query.FindAll<PurchaseContractItemDto>(sql, new { Codes = dic.Keys.ToArray() });
            foreach (var product in productItems)
            {
                product.Price = dic[product.Code];
            }
            return productItems;
        }

        private Dictionary<string, decimal> GetProductDic(string productIds)
        {
            Dictionary<string, decimal> dicProductPrice = new Dictionary<string, decimal>(1000);
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception("粘贴的格式不正确");
            }
            return dicProductPrice;
        }



        public IDictionary<int, string> GetCooperateWay()
        {
            return typeof(CooperateWay).GetValueToDescription();
        }
    }
}
