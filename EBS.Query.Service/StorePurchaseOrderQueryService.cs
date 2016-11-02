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
        public IEnumerable<DTO.StorePurchaseOrderDto> GetPageList(DTO.Pager page, DTO.SearchStorePurchaseOrder condition)
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
                where += "and t3.ProductSkuId in (select Id from ProductSku where Code=@ProductCodeOrBarCode or BarCode=@ProductCodeOrBarCode) ";
                param.Code = condition.Code;
            }
            string sql = @"select t0.Id,t0.Code,t0.SupplierId,t0.UpdatedOn,t0.Status,t1.Code as SupplierCode,t1.Name as SupplierName,t2.Name as StoreName ,t4.NickName as UpdatedByName 
from storepurchaseorder t0 inner join supplier t1 on t0.SupplierId = t1.Id inner join store t2 on t0.StoreId = t2.Id inner join storepurchaseorderitem t3
on t0.Id = t3.StorePurchaseOrderId  inner join account t4 on t0.UpdatedBy = t4.Id
where 1=1 {0} ORDER BY t0.Id desc LIMIT {1},{2}";
            //rows = this._query.FindPage<ProductDto>(page.PageIndex, page.PageSize).Where<ProductSku>(where, param);
            sql = string.Format(sql, where, (page.PageIndex - 1) * page.PageSize, page.PageSize);
            var rows = this._query.FindAll<StorePurchaseOrderDto>(sql, param);
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
            string sql=@"select p.id,p.`Name` as ProductName,p.Specification,p.BarCode,p.`Code` as ProductCode,p.SubSkuQuantity,i.ContractPrice 
from purchasecontract c inner join purchasecontractitem i on c.Id = i.PurchaseContractId
inner join productsku p on p.Id = i.ProductSKUId
where c.StartDate <= NOW() and c.EndDate>=NOW() and c.`Status`=3 and c.SupplierId =@SupplierId and c.storeId=@StoreId and (p.BarCode='@ProductCodeOrBarCode' or p.`Code`='@ProductCodeOrBarCode' ) order by c.Id DESC LIMIT 1";
            var item = _query.Find<StorePurchaseOrderItemDto>(sql, new { ProductCodeOrBarCode = productCodeOrBarCode, SupplierId = supplierId, StoreId = storeId });
            if (item == null) { throw new Exception("查无商品，请检查供应商合同"); }
            return item;

        }
    }
}
