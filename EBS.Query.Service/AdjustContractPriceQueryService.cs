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
        public IEnumerable<AdjustContractPriceDto> GetPageList(Pager page, SearchAdjustContractPrice condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and (p.`Code`=@ProductCodeOrBarCode or p.BarCode=@ProductCodeOrBarCode)";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
            }
            if (!string.IsNullOrEmpty(condition.Code))
            {
                where += "and a.Code=@Code ";
                param.Code = condition.Code;
            }
            if (condition.SupplierId > 0)
            {
                where += "and a.SupplierId=@SupplierId ";
                param.SupplierId = condition.SupplierId;
            }
            if (condition.StoreId > 0)
            {
                where += "and a.StoreId=@StoreId ";
                param.StoreId = condition.StoreId;
            }
            if (condition.Status !=0)
            {
                where += "and a.Status=@Status ";
                param.Status = condition.Status;
            }
            string sql = @"select a.*,c.NickName,r.`Name` as StoreName ,s.`Name` as SupplierName,s.`Code` as SupplierCode
from adjustcontractprice a 
left join account c on c.Id = a.CreatedBy
left join Supplier s on s.Id = a.SupplierId
left join store r on r.Id = a.StoreId
where 1=1 {0} ORDER BY a.Id desc LIMIT {1},{2}";

            string sqlCount = @"select count(*)
from adjustcontractprice a 
left join account c on c.Id = a.CreatedBy
left join Supplier s on s.Id = a.SupplierId
left join store r on r.Id = a.StoreId
where 1=1 {0} ";
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sqlCount = string.Format(sqlCount, where);
            var rows = this._query.FindAll<AdjustContractPriceDto>(sql, param);
            page.Total = this._query.Context.ExecuteScalar<int>(sqlCount, param);

            return rows;
        }
       

        public IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceItems(int AdjustContractPriceId)
        {
            string sql = @"select pc.ProductId,p.Code as ProductCode,p.`Name` as ProductName,p.BarCode,p.Specification,p.Unit,pc.AdjustPrice,pc.ContractPrice 
from AdjustContractPriceItem pc left join  Product p on pc.ProductId=p.Id  
            where pc.AdjustContractPriceId = @AdjustContractPriceId";
            var productItems = _query.FindAll<AdjustContractPriceItemDto>(sql, new { AdjustContractPriceId = AdjustContractPriceId });
            return productItems;
        }

        public IEnumerable<AdjustContractPriceItemDto> GetItems(int AdjustContractPriceId, int supplierId, int storeId)
        {
            string sql = @"select p.id as ProductId,p.`Name` as ProductName,p.Specification,p.`Code` as ProductCode,p.BarCode,p.Unit,p.Specification ,t.ContractPrice
from (select c.Id,i.ProductId,i.ContractPrice,i.AdjustPrice from purchasecontract c 
inner join purchasecontractitem i on c.Id = i.PurchaseContractId
left join supplier s on s.Id = c.SupplierId 
where c.`Status` = 3 and c.EndDate>= CURDATE() and s.Id=@SupplierId and FIND_IN_SET(@StoreId,c.StoreIds) and i.AdjustContractPriceId = @AdjustContractPriceId
) t left join product p on p.Id = t.ProductId ";
            var result = _query.FindAll<AdjustContractPriceItemDto>(sql, new { AdjustContractPriceId = AdjustContractPriceId, SupplierId = supplierId, StoreId = storeId, Today = DateTime.Now });
            return result;
        }


        public AdjustContractPriceItemDto GetAdjustContractPriceItem(string productCodeOrBarCode, int supplierId, int storeId)
        {
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            string sql = @"select p.id as ProductId,p.`Name` as ProductName,p.Specification,p.`Code` as ProductCode,p.BarCode,p.Unit,p.Specification ,t.ContractPrice
from product p left join (select c.Id,i.ProductId,i.ContractPrice from purchasecontract c 
inner join purchasecontractitem i on c.Id = i.PurchaseContractId
left join supplier s on s.Id = c.SupplierId 
where c.`Status` = 3 and c.EndDate>= CURDATE() and s.Id=@SupplierId and FIND_IN_SET(@StoreId,c.StoreIds) 
) t on p.Id = t.ProductId 
where (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode )
 order by t.Id desc LIMIT 1";
            var item = _query.Find<AdjustContractPriceItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId, SupplierId = supplierId });
            return item;
        }
        public IEnumerable<AdjustContractPriceItemDto>  GetAdjustContractPriceList(string inputProducts, int supplierId, int storeId)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            var dic = inputProducts.ToDecimalDic();
            string sql = @"select p.id as ProductId,p.`Name` as ProductName,p.Specification,p.`Code` as ProductCode,p.BarCode,p.Unit,p.Specification ,i.ContractPrice ,c.StartDate,C.EndDate  
from purchasecontract c inner join purchasecontractitem i on c.Id = i.PurchaseContractId 
left join Product p on p.Id = i.ProductId 
where  p.`Code` in @ProductCode  and c.SupplierId = @SupplierId and c.storeId = @StoreId and c.StartDate <= @Today and c.EndDate >= @Today and c.`Status`= 3 order by c.Id DESC";
            var productItems = _query.FindAll<AdjustContractPriceItemDto>(sql, new { ProductCode = dic.Keys.ToArray(), SupplierId = supplierId, StoreId = storeId, Today = DateTime.Now });
            foreach (var product in productItems)
            {
                product.ContractPrice = dic[product.ProductCode];
            }
            return productItems;
        }

        public IEnumerable<SupplierDto> QuerySupplier(string productCodeOrBarCode, int storeId)
        {
            string sql = @"select s.Id,s.Code,s.Name,s.Type,t.Id as PurchaseContractId,t.code as PurchaseContractCode  from supplier s left join (
  select c.Id,c.code, c.SupplierId,c.StoreIds , i.ProductId from purchasecontract c inner join purchasecontractitem i on c.Id = i.PurchaseContractId
where c.`Status` = 3 and c.EndDate > NOW() 
) t on s.Id = t.SupplierId
left join product p on p.Id = t.ProductId 
where (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode ) and FIND_IN_SET(@StoreId,t.StoreIds)
order by t.Id desc
";
            var result = _query.FindAll<SupplierDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, StoreId = storeId });
            return result;
        }
    }
}
