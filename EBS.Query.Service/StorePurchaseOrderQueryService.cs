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
    public class StorePurchaseOrderQueryService : IStorePurchaseOrderQuery
    {
        IQuery _query;
        public StorePurchaseOrderQueryService(IQuery query)
        {
            this._query = query;
        }
        public IEnumerable<DTO.StorePurchaseOrderQueryDto> GetPageList(DTO.Pager page, DTO.SearchStorePurchaseOrder condition)
        {
            dynamic param = new ExpandoObject();
            string where = "";
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
            if (!string.IsNullOrEmpty(condition.ProductCodeOrBarCode))
            {
                where += "and t3.ProductId in (select Id from Product where Code=@ProductCodeOrBarCode or BarCode=@ProductCodeOrBarCode) ";
                param.Code = condition.Code;
            }
            string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.CreatedOn,t0.CreatedByName,t0.Status,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName  
from storepurchaseorder t0 inner join supplier t1 on t0.SupplierId = t1.Id inner join store t2 on t0.StoreId = t2.Id 
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<Product>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StorePurchaseOrderQueryDto>(sql, param);
            page.Total = this._query.Count<StorePurchaseOrder>(where, param);

            return rows;
        }


        public Dictionary<int, string> GetStorePurchaseOrderStatus()
        {
            var dic = typeof(PurchaseOrderStatus).GetValueToDescription();
            return dic;
        }


        public StorePurchaseOrderItemDto GetPurchaseOrderItem(string productCodeOrBarCode, int supplierId, int storeId)
        {
            if (supplierId == 0) { throw new Exception("请选择供应商"); }
            if (string.IsNullOrEmpty(productCodeOrBarCode)) { throw new Exception("请输入商品编码或条码"); }
            string sql = @"select p.id as ProductId,p.`Name` as ProductName,p.Specification,p.`Code` as ProductCode,p.BarCode,p.Unit,p.SpecificationQuantity,i.ContractPrice,i.ContractPrice as Price,p.Unit 
from purchasecontract c inner join purchasecontractitem i on c.Id = i.PurchaseContractId 
left join Product p on p.Id = i.ProductId 
where (p.BarCode=@ProductCodeOrBarCode or p.`Code`=@ProductCodeOrBarCode ) and c.SupplierId =@SupplierId and c.storeId=@StoreId and c.StartDate <= @Today and c.EndDate>=@Today and c.`Status`=3 order by c.Id DESC LIMIT 1";            
            var item = _query.Find<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, SupplierId = supplierId, StoreId = storeId, Today = DateTime.Now });
            if (item == null) { throw new Exception("查无商品，请检查供应商合同"); }
            return item;

        }
        public IEnumerable<StorePurchaseOrderItemDto> GetPurchaseOrderItemList(string inputProducts, int supplierId, int storeId)
        {
            if (string.IsNullOrEmpty(inputProducts)) throw new Exception("商品明细为空");
            var dic = GetProductDic(inputProducts);
            string sql = @"select p.id as ProductId,p.`Name` as ProductName,p.Specification,p.`Code` as ProductCode,p.BarCode,p.Unit,p.SpecificationQuantity,i.ContractPrice,i.ContractPrice as Price,p.Unit 
from purchasecontract c inner join purchasecontractitem i on c.Id = i.PurchaseContractId
left join Product p on p.Id = i.ProductId
where  p.`Code` in @ProductCode  and c.SupplierId = @SupplierId and c.storeId = @StoreId and c.StartDate <= @Today and c.EndDate >= @Today and c.`Status`= 3 order by c.Id DESC";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sql, new { ProductCode = dic.Keys.ToArray(), SupplierId = supplierId, StoreId = storeId, Today = DateTime.Now });
            foreach (var product in productItems)
            {
                product.Quantity = dic[product.ProductCode];
            }
            return productItems;
        }
        private Dictionary<string, int> GetProductDic(string productIds)
        {
            Dictionary<string, int> dicProductPrice = new Dictionary<string, int>(1000);
            string[] productIdArray = productIds.Split('\n');
            foreach (var item in productIdArray)
            {
                if (item.Contains("\t"))
                {
                    string[] parentIDAndQuantity = item.Split('\t');
                    if (!dicProductPrice.ContainsKey(parentIDAndQuantity[0].Trim()))
                    {
                        dicProductPrice.Add(parentIDAndQuantity[0].Trim(), int.Parse(parentIDAndQuantity[1]));
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
        public StorePurchaseOrderDto GetById(int id)
        {
           string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.StoreId,t0.CreatedOn,t0.CreatedByName,t0.ReceivedOn,t0.ReceivedByName,t0.StoragedOn,t0.StoragedByName,
t0.IsGift,t0.Status,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName,t0.SupplierBill
from storepurchaseorder t0 inner
join supplier t1 on t0.SupplierId = t1.Id inner
join store t2 on t0.StoreId = t2.Id
where t0.Id= @Id  LIMIT 1";
            var model = _query.Find<StorePurchaseOrderDto>(sql, new { Id = id});
            string sqlitem = @"select i.*,p.Name as ProductName,p.`Code` as ProductCode,p.BarCode,p.Specification,p.Unit,p.SpecificationQuantity
 from storepurchaseorderitem i left join product p on i.productId = p.Id
where i.storepurchaseorderid= @Id";
            var productItems = _query.FindAll<StorePurchaseOrderItemDto>(sqlitem, new { Id = id }).ToList();

            model.Items = productItems;
            return model;
        }

       
    }
}
