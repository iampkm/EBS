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
        public IEnumerable<PurchaseContractDto> GetPageList(Pager page, SearchSupplierContract condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += @"and t0.Id in (select i.purchasecontractId from  purchasecontractitem i 
left join product p on i.ProductId = p.Id where (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode )) ";
                param.ProductCodeOrBarCode =  condition.ProductCodeOrBarCode;
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
                where += "and FIND_IN_SET(@StoreId,t0.StoreIds) ";
                param.StoreId = condition.StoreId;
            }
            if (condition.Status != 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            string sql = @"select t0.Id,t0.Name,t0.Code,t0.SupplierId,t0.Contact,t0.StartDate,t0.EndDate,t0.Status,t0.Remark,t1.Code as SupplierCode,t1.Name as SupplierName   
from PurchaseContract t0 left join supplier t1 on t0.SupplierId = t1.Id 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<PurchaseContractDto>(sql, param);
            page.Total = this._query.Count<PurchaseContract>(where, param);

            return rows;
        }

        
        /// <summary>
        /// 按照商品条码导入
        /// </summary>
        /// <param name="productCodePriceInput"></param>
        /// <returns></returns>
        public IEnumerable<PurchaseContractItemDto> GetPurchaseContractItems(string productCodePriceInput)
        {
            if (string.IsNullOrEmpty(productCodePriceInput)) throw new Exception("商品明细为空");     
            var dic = productCodePriceInput.ToDecimalDic();
            string sql = "select p.Id as ProductId,p.Code,p.`Name`,p.Specification,p.Unit,p.BarCode from Product p where p.BarCode in @BarCodes";
            var productItems= _query.FindAll<PurchaseContractItemDto>(sql, new { BarCodes = dic.Keys.ToArray() });
            foreach (var product in productItems)
            {
                if (dic.ContainsKey(product.BarCode))
                {
                    product.ContractPrice = dic[product.BarCode];
                }               
            }
            return productItems;
        }

        public IEnumerable<PurchaseContractItemDto> GetPurchaseContractItems(int purchaseContractId)
        {
            string sql = "select  p.Id as ProductId,p.Code,p.`Name`,p.Specification,p.Unit,p.BarCode ,pc.ContractPrice,pc.Status from PurchaseContractItem pc left join  Product p on pc.ProductId=p.Id  where pc.purchaseContractId = @PurchaseContractId";
            var productItems = _query.FindAll<PurchaseContractItemDto>(sql, new { PurchaseContractId = purchaseContractId });
            return productItems;
        }


        public Dictionary<int, string> GetPurchaseContractStatus()
        {
            var dic = typeof(PurchaseContractStatus).GetValueToDescription();
            return dic;
        }

        /// <summary>
        /// 根据供应商的比价结果，生成合同实体
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public PurchaseContractCreateDto QueryContractInfo(int supplierId)
        {
            string sql = @"select t0.Id as ProductId,t0.Name,t0.Code,t0.BarCode,t0.Specification,t1.FullName as CategoryName,t2.Price as ContractPrice
from product t0 inner
join category t1 on t0.CategoryId = t1.Id
inner
join SupplierProduct t2 on t0.Id = t2.ProductId
where t2.SupplierId = @SupplierId and t2.CompareStatus = @CompareStatus";
            var items = _query.FindAll<PurchaseContractItemDto>(sql, new { SupplierId = supplierId, CompareStatus = (int)ComparePriceStatus.Success });
            if (!items.Any()) {
                throw new Exception("请标记要生成合同的供应商商品");
            }
            var model = new PurchaseContractCreateDto();
            var supplier = _query.Find<Supplier>(supplierId);
            model.SupplierId = supplierId;
            model.SupplierName = supplier.Name;
            model.SupplierCode = supplier.Code;
            model.Items = items.ToList();
            return model;
        }

    }
}
