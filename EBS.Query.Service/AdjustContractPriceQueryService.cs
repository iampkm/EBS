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
                where += "and (t4.`Code`=@ProductCodeOrBarCode or t4.BarCode=@ProductCodeOrBarCode)";
                param.ProductCodeOrBarCode = condition.ProductCodeOrBarCode;
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
            if (condition.Status != 0)
            {
                where += "and t0.Status=@Status ";
                param.Status = condition.Status;
            }
            string sql = @"select t0.Id,t0.`Code`,t0.StartDate,t0.EndDate,t1.`Code` as SupplierCode,t1.`Name` as SupplierName,t2.`Name` as StoreName,t3.ProductId,t4.`Name` as ProductName,t4.`Code` as ProductCode,t4.BarCode,t4.Specification,t4.Unit,t3.AdjustPrice,t3.ContractPrice  from AdjustContractPrice t0 
left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id
inner join AdjustContractPriceItem t3 on t0.Id = t3.AdjustContractPriceId
inner join product t4 on t3.productid = t4.id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            string sqlCount = @"select count(*) from AdjustContractPrice t0 
left join supplier t1 on t0.SupplierId = t1.Id left join store t2 on t0.StoreId = t2.Id
inner join AdjustContractPriceItem t3 on t0.Id = t3.AdjustContractPriceId
inner join product t4 on t3.productid = t4.id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            sqlCount = string.Format(sqlCount, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<AdjustContractPriceDto>(sql, param);
            page.Total = this._query.FindScalar<int>(sqlCount, param);

            return rows;
        }
       

        public IEnumerable<AdjustContractPriceItemDto> GetAdjustContractPriceItems(int AdjustContractPriceId)
        {
            string sql = @"select pc.ProductId,p.Code as ProductCode,p.`Name` as ProductName,p.BarCode,p.Specification,c.FullName as CategoryName,pc.AdjustPrice,pc.ContractPrice from AdjustContractPriceItem pc inner join  Product p on pc.ProductId=p.Id inner join category c on p.categoryId = c.Id 
            where pc.AdjustContractPriceId = @AdjustContractPriceId";
            var productItems = _query.FindAll<AdjustContractPriceItemDto>(sql, new { AdjustContractPriceId = AdjustContractPriceId });
            return productItems;
        }

        public IEnumerable<AdjustContractPriceItemDto> GetItems(int AdjustContractPriceId, int supplierId, int storeId)
        {
            string sql = @"select pc.ProductId,p.`Code` as ProductCode,p.`Name` as ProductName,p.BarCode,p.Specification,p.Unit, pc.AdjustPrice,pc.ContractPrice ,c.startDate,c.endDate
from AdjustContractPriceItem pc
inner join  Product p on pc.ProductId=p.Id 
left join  purchasecontractitem i on i.ProductId =  pc.ProductId
left join  purchasecontract c  on c.Id = i.PurchaseContractId 
where pc.AdjustContractPriceId = @AdjustContractPriceId and c.SupplierId =@SupplierId and c.storeId=@StoreId and c.`Status`=3 and c.StartDate <= @Today and c.EndDate>=@Today order by c.Id DESC";
            var result = _query.FindAll<AdjustContractPriceItemDto>(sql, new { AdjustContractPriceId = AdjustContractPriceId, SupplierId = supplierId, StoreId = storeId, Today = DateTime.Now });
            return result;
        }


        public AdjustContractPriceItemDto GetAdjustContractPriceItem(string productCodeOrBarCode, int supplierId, int storeId)
        {
            if (supplierId == 0) { throw new Exception("请选择供应商"); }
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            string sql = @"select p.id as ProductId,p.`Name` as ProductName,p.Specification,p.`Code` as ProductCode,p.BarCode,p.Unit,p.Specification ,i.ContractPrice ,c.StartDate,C.EndDate 
from purchasecontract c inner join purchasecontractitem i on c.Id = i.PurchaseContractId 
left join Product p on p.Id = i.ProductId 
where (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode ) and c.SupplierId =@SupplierId and c.storeId=@StoreId and c.StartDate <= @Today and c.EndDate>=@Today and c.`Status`=3 order by c.Id DESC LIMIT 1";
            var item = _query.Find<AdjustContractPriceItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, SupplierId = supplierId, StoreId = storeId, Today = DateTime.Now });
            //设置当前件规            
            if (item == null) { throw new Exception("查无商品，请检查供应商合同"); }
            return item;

        }
        public IEnumerable<AdjustContractPriceItemDto>  GetAdjustContractPriceList(string inputProducts, int supplierId, int storeId)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            var dic = GetProductDic(inputProducts);
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
