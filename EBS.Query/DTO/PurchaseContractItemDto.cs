using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.Extension;
using EBS.Domain.ValueObject;
namespace EBS.Query.DTO
{
   public class PurchaseContractItemDto
    {
         public int ProductId { get; set; }
         /// <summary>
         /// 商品名
         /// </summary>
         public string Name { get; set; }
       /// <summary>
       /// 商品编码
       /// </summary>
         public string Code { get; set; }

         public string BarCode { get; set; }

         public string CategoryName { get; set; }

        public string Specification { get; set; }

        public string Unit { get; set; }
       /// <summary>
       /// 合同价
       /// </summary>
         public decimal ContractPrice { get; set; }

         public SupplyStatus Status { get; set; }

         public string StatusName {
             get { return Status.Description(); }
         }
    }
}
